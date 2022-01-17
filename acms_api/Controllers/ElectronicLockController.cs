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
    public class ElectronicLockController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly LinkGenerator _linkGenerator;

        public ElectronicLockController(ILoggerManager logger, IRepositoryWrapper repository, LinkGenerator linkGenerator)
        {
            _logger = logger;
            _repository = repository;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetElectronicLocks([FromQuery] ElectronicLockParameters electronicLockParameters)
        {
            try
            {
                var electronicLocks = _repository.ElectronicLock.GetAllElectronicLocks(electronicLockParameters);

                var metadata = new
                {
                    electronicLocks.TotalCount,
                    electronicLocks.PageSize,
                    electronicLocks.CurrentPage,
                    electronicLocks.TotalPages,
                    electronicLocks.HasNext,
                    electronicLocks.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                _logger.LogInfo($"Returned {electronicLocks.Count} electronicLocks from database.");

                var shapedElectronicLocks = electronicLocks.Select(i => i.Entity).ToList();

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    return Ok(shapedElectronicLocks);
                }

                for (var index = 0; index < electronicLocks.Count; index++)
                {
                    var brandLinks = CreateLinksForElectronicLock(((NormalShapedEntity)electronicLocks[index]).Id, electronicLockParameters.Fields);
                    shapedElectronicLocks[index].Add("Links", brandLinks);
                }

                var electronicLocksWrapper = new LinkCollectionWrapper<Entity>(shapedElectronicLocks);

                return Ok(CreateLinksForElectronicLocks(electronicLocksWrapper));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllElectronicLocks action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}", Name = "ElectronicLockById")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public IActionResult GetElectronicLockById(Guid id, [FromQuery] string fields)
        {
            try
            {
                var electronicLock = (NormalShapedEntity)_repository.ElectronicLock.GetElectronicLockById(id, fields);

                if (electronicLock.Id == Guid.Empty)
                {
                    _logger.LogError($"ElectronicLock with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                var mediaType = (MediaTypeHeaderValue)HttpContext.Items["AcceptHeaderMediaType"];

                if (!mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogInfo($"Returned shaped electronicLock with id: {id}");
                    return Ok(electronicLock.Entity);
                }

                electronicLock.Entity.Add("Links", CreateLinksForElectronicLock(electronicLock.Id, fields));

                return Ok(electronicLock.Entity);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wring inside GetElectronicLockById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult CreateElectronicLock([FromBody] ElectronicLock electronicLock)
        {
            try
            {
                if (electronicLock.IsObjectNull())
                {
                    _logger.LogError("ElectronicLock object sent from client is null.");
                    return BadRequest("ElectronicLock object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid electronicLock object sent from client.");
                    return BadRequest("Invalid model object");
                }

                _repository.ElectronicLock.CreateElectronicLock(electronicLock);
                _repository.Save();

                return CreatedAtRoute("ElectronicLockById", new { id = electronicLock.Id }, electronicLock);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateElectronicLock action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateElectronicLock(Guid id, [FromBody] ElectronicLock electronicLock)
        {
            try
            {
                if (electronicLock.IsObjectNull())
                {
                    _logger.LogError("ElectronicLock object sent from client is null.");
                    return BadRequest("ElectronicLock object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid electronicLock object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var dbElectronicLock = _repository.ElectronicLock.GetElectronicLockById(id);
                if (dbElectronicLock.IsEmptyObject())
                {
                    _logger.LogError($"ElectronicLock with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.ElectronicLock.UpdateElectronicLock(dbElectronicLock, electronicLock);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateElectronicLock action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteElectronicLock(Guid id)
        {
            try
            {
                var electronicLock = _repository.ElectronicLock.GetElectronicLockById(id);
                if (electronicLock.IsEmptyObject())
                {
                    _logger.LogError($"ElectronicLock with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                _repository.ElectronicLock.DeleteElectronicLock(electronicLock);
                _repository.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteElectronicLock action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        private IEnumerable<Link> CreateLinksForElectronicLock(Guid id, string fields = "")
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetElectronicLockById), values: new {id, fields}), "self", "GET"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(DeleteElectronicLock), values: new {id}), "delete_electronicLock", "DELETE"),
                new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(UpdateElectronicLock), values: new {id}), "update_electronicLock", "PUT")
            };

            return links;
        }

        private LinkCollectionWrapper<Entity> CreateLinksForElectronicLocks(LinkCollectionWrapper<Entity> electronicLocksWrapper)
        {
            electronicLocksWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(HttpContext, nameof(GetElectronicLocks), values: new { }), "self", "GET"));

            return electronicLocksWrapper;
        }
    }
}
