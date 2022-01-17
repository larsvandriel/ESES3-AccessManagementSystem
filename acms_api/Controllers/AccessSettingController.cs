using AccessManagementSystem.API.Filters;
using AccessManagementSystem.Contracts;
using AccessManagementSystem.Entities.Extensions;
using AccessManagementSystem.Entities.Models;
using AccessManagementSystem.Entities.Parameters;
using AccessManagementSystem.Entities.ShapedEntities;
using LoggingService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace AccessManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessSettingController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly LinkGenerator _linkGenerator;

        public AccessSettingController(ILoggerManager logger, IRepositoryWrapper repository, LinkGenerator linkGenerator)
        {
            _logger = logger;
            _repository = repository;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetAccessSettings([FromQuery] AccessSettingParameters accessSettingParameters)
        {
            try
            {
                var accessSettings = _repository.AccessSetting.GetAllAccessSettings(accessSettingParameters);

                var metadata = new
                {
                    accessSettings.TotalCount,
                    accessSettings.PageSize,
                    accessSettings.CurrentPage,
                    accessSettings.TotalPages,
                    accessSettings.HasNext,
                    accessSettings.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                _logger.LogInfo($"Returned {accessSettings.Count} accessSettings from database.");

                var shapedAccessSettings = accessSettings.Select(i => i.Entity).ToList();

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Ok(shapedAccessSettings);
                }

                for (var index = 0; index < accessSettings.Count; index++)
                {
                    var brandLinks = CreateLinksForAccessSetting(((ShapedAccessSettingEntity)accessSettings[index]).ElectronicLockId, ((ShapedAccessSettingEntity)accessSettings[index]).AccessorId, accessSettingParameters.Fields);
                    shapedAccessSettings[index].Add("Links", brandLinks);
                }

                var accessSettingsWrapper = new LinkCollectionWrapper<Entity>(shapedAccessSettings);

                return Ok(CreateLinksForAccessSettings(accessSettingsWrapper));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllAccessSettings action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{electronicLockId}/{accessorId}", Name = "AccessSettingByElectronicLockAndAccessor")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetAccessSettingByElectronicLockAndAccessor(Guid electronicLockId, Guid accessorId, [FromQuery] string fields)
        {
            try
            {
                var accessSetting = (ShapedAccessSettingEntity)_repository.AccessSetting.GetAccessSettingByElectronicLockAndAccessor(electronicLockId, accessorId, fields);

                if (accessSetting.ElectronicLockId == Guid.Empty)
                {
                    _logger.LogError($"AccessSetting with ElectronicId: {electronicLockId}, hasn't been found in db.");
                    return NotFound();
                }

                if (accessSetting.AccessorId == Guid.Empty)
                {
                    _logger.LogError($"AccessSetting with AccessorId: {accessorId}, hasn't been found in db.");
                    return NotFound();
                }

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogInfo($"Returned shaped accessSetting with electronicLockId: {electronicLockId} and accessorId: {accessorId}");
                    return Ok(accessSetting.Entity);
                }

                accessSetting.Entity.Add("Links", CreateLinksForAccessSetting(accessSetting.ElectronicLockId, accessSetting.AccessorId, fields));

                return Ok(accessSetting.Entity);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wring inside GetAccessSettingById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateAccessSetting([FromBody] AccessSetting accessSetting)
        {
            try
            {
                if (accessSetting.IsObjectNull())
                {
                    _logger.LogError("AccessSetting object sent from client is null.");
                    return BadRequest("AccessSetting object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid accessSetting object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _repository.AccessSetting.CreateAccessSetting(accessSetting);
                _repository.Save();

                return CreatedAtRoute("AccessSettingById", new { electronicLockId = accessSetting.ElectricalLockId, accessorId = accessSetting.AccessorId }, accessSetting);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateAccessSetting action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{electronicLockId}/{accessorId}")]
        public IActionResult UpdateAccessSetting(Guid electronicLockId, Guid accessorId, [FromBody] AccessSetting accessSetting)
        {
            try
            {
                if (accessSetting.IsObjectNull())
                {
                    _logger.LogError("AccessSetting object sent from client is null.");
                    return BadRequest("AccessSetting object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid accessSetting object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbAccessSetting = _repository.AccessSetting.GetAccessSettingByElectronicLockAndAccessor(electronicLockId, accessorId);
                if (dbAccessSetting.IsEmptyObject())
                {
                    _logger.LogError($"AccessSetting with electronicLockId: {electronicLockId} and accessorId {accessorId}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.AccessSetting.UpdateAccessSetting(dbAccessSetting, accessSetting);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateAccessSetting action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{electronicLockId}/{accessorId}")]
        public IActionResult DeleteAccessSetting(Guid electronicLockId, Guid accessorId)
        {
            try
            {
                var accessSetting = _repository.AccessSetting.GetAccessSettingByElectronicLockAndAccessor(electronicLockId, accessorId);
                if (accessSetting.IsEmptyObject())
                {
                    _logger.LogError($"AccessSetting with electronicLockId: {electronicLockId} and accessorId: {accessorId}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.AccessSetting.DeleteAccessSetting(accessSetting);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteAccessSetting action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private IEnumerable<Link> CreateLinksForAccessSetting(Guid electronicLockId, Guid accessorId, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetAccessSettingByElectronicLockAndAccessor), values: new {electronicLockId, accessorId, fields}), "self", "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteAccessSetting), values: new {electronicLockId, accessorId}), "delete_accessSetting", "DELETE"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateAccessSetting), values: new {electronicLockId, accessorId}), "update_accessSetting", "PUT")
            };

            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForAccessSettings(LinkCollectionWrapper<Entity> accessSettingsWrapper)
        {
            accessSettingsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetAccessSettings), values: new { }), "self", "GET"));

            return accessSettingsWrapper;
        }
    }
}
