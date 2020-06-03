using SimpleConfigurationManager.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OperatingSystem = SimpleConfigurationManager.Models.DbModels.OperatingSystem;

namespace SimpleConfigurationManager.Models.ViewModels
{
    public class ConfigurationViewModel
    {
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
        public OperatingSystem OperatingSystem { get; set; }

        public Configuration ToDbModel()
        {
            return new Configuration()
            {
                IdConfiguration = this.IdConfiguration,
                TimeOfCreation = this.TimeOfCreation,
                TimeOfLastUpdate = this.TimeOfLastUpdate,
                Hash = this.Hash,
                Name = this.Name,
                ShortDescription = this.ShortDescription,
                FullDescription = this.FullDescription,
                ConfigurationScript = this.ConfigurationScript,
                IsPublic = this.IsPublic,
                OperatingSystemId = this.OperatingSystemId,
                UserId = this.UserId,
                Deleted = this.Deleted,
                OperatingSystem = this.OperatingSystem
            };
        }

        public ConfigurationViewModel FromDbModel(Configuration configuration)
        {
            return new ConfigurationViewModel()
            {
                IdConfiguration = configuration.IdConfiguration,
                TimeOfCreation = configuration.TimeOfCreation,
                TimeOfLastUpdate = configuration.TimeOfLastUpdate,
                Hash = configuration.Hash,
                Name = configuration.Name,
                ShortDescription = configuration.ShortDescription,
                FullDescription = configuration.FullDescription,
                ConfigurationScript = configuration.ConfigurationScript,
                IsPublic = configuration.IsPublic,
                OperatingSystemId = configuration.OperatingSystemId,
                UserId = configuration.UserId,
                Deleted = configuration.Deleted,
                OperatingSystem = configuration.OperatingSystem
            };
        }
    }
}
