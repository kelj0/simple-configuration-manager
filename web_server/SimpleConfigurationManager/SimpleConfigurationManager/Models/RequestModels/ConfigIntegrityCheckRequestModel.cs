using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleConfigurationManager.Models.RequestModels
{
    public class ConfigIntegrityCheckRequestModel
    {
        public string ConfigName { get; set; }
        public string Hash { get; set; }
    }
}
