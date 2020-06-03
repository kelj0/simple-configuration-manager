using System;
using System.Collections.Generic;

namespace SimpleConfigurationManager.Models.DbModels
{
    public partial class ServerConfiguration
    {
        public int ServerId { get; set; }
        public int ConfigurationId { get; set; }
        public bool? Deleted { get; set; }

        public virtual Configuration Configuration { get; set; }
        public virtual Server Server { get; set; }
    }
}
