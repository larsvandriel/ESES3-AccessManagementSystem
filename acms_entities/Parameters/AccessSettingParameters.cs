using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessManagementSystem.Entities.Parameters
{
    public class AccessSettingParameters: QueryStringParameters
    {
        public AccessSettingParameters()
        {
            OrderBy = "Lock";
        }
    }
}
