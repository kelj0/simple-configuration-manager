using System;
using System.Collections.Generic;

namespace SimpleConfigurationManager.Models.DbModels
{
    public partial class OperatingSystem
    {
        public OperatingSystem()
        {
            Configuration = new HashSet<Configuration>();
            Server = new HashSet<Server>();
        }

        public int IdOperatingSystem { get; set; }
        public string OperatingSystemName { get; set; }

        public virtual ICollection<Configuration> Configuration { get; set; }
        public virtual ICollection<Server> Server { get; set; }
    }
}
