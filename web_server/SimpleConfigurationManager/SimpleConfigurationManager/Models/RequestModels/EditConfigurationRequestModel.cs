using SimpleConfigurationManager.Infrastructure.Extensions;
using SimpleConfigurationManager.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleConfigurationManager.Models.RequestModels
{
    public class EditConfigurationRequestModel
    {
        public int IdConfiguration { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public byte[] ConfigurationScript { get; set; }
        public bool IsPublic { get; set; }
        public int OperatingSystemId { get; set; }
        public int UserId { get; set; }

        public Configuration ToDbModel()
        {
            return new Configuration()
            {
                IdConfiguration = this.IdConfiguration,
                TimeOfLastUpdate = DateTime.UtcNow,
                Hash = Convert.ToBase64String(this.ConfigurationScript).GetSha1(),
                Name = this.Name,
                ShortDescription = this.ShortDescription,
                FullDescription = this.FullDescription,
                ConfigurationScript = this.ConfigurationScript,
                IsPublic = this.IsPublic,
                OperatingSystemId = this.OperatingSystemId,
                UserId = this.UserId
            };
        }
    }
}
