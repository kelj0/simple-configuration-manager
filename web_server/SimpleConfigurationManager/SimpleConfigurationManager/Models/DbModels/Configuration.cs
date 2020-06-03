using System;
using System.Collections.Generic;

namespace SimpleConfigurationManager.Models.DbModels
{
    public partial class Configuration
    {
        public Configuration()
        {
            ConfigurationReview = new HashSet<ConfigurationReview>();
            ServerConfiguration = new HashSet<ServerConfiguration>();
        }

        public int IdConfiguration { get; set; }
        public DateTime TimeOfCreation { get; set; }
        public DateTime? TimeOfLastUpdate { get; set; }
        public string Hash { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string ConfigurationScript { get; set; }
        public bool IsPublic { get; set; }
        public int OperatingSystemId { get; set; }
        public int UserId { get; set; }
        public bool? Deleted { get; set; }

        public virtual OperatingSystem OperatingSystem { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ConfigurationReview> ConfigurationReview { get; set; }
        public virtual ICollection<ServerConfiguration> ServerConfiguration { get; set; }
    }
}
