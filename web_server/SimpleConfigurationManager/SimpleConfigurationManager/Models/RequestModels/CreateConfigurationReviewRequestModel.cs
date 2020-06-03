using SimpleConfigurationManager.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleConfigurationManager.Models.RequestModels
{
    public class CreateConfigurationReviewRequestModel
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public int ConfigurationId { get; set; }

        public ConfigurationReview ToDbModel()
        {
            return new ConfigurationReview()
            {
                Rating = this.Rating,
                TimeOfCreation = DateTime.UtcNow,
                Comment = this.Comment,
                UserId = this.UserId,
                ConfigurationId = this.ConfigurationId
            };
        }
    }
}
