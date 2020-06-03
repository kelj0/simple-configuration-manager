using System;
using System.Collections.Generic;

namespace SimpleConfigurationManager.Models.DbModels
{
    public partial class ConfigurationReview
    {
        public int IdConfigurationReview { get; set; }
        public DateTime TimeOfCreation { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public int ConfigurationId { get; set; }
        public bool? Deleted { get; set; }

        public virtual Configuration Configuration { get; set; }
        public virtual User User { get; set; }
    }
}
