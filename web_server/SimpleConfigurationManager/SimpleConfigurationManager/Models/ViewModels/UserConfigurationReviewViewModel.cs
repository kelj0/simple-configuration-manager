using SimpleConfigurationManager.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleConfigurationManager.Models.ViewModels
{
    public class UserConfigurationReviewViewModel
    {
        public int IdConfigurationReview { get; set; }
        public DateTime TimeOfCreation { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public int UserId { get; set; }
        public int ConfigurationId { get; set; }
        public bool? Deleted { get; set; }

        public ConfigurationReview ToDbModel()
        {
            return new ConfigurationReview()
            {
                IdConfigurationReview = this.IdConfigurationReview,
                TimeOfCreation = this.TimeOfCreation,
                Rating = this.Rating,
                Comment = this.Comment,
                UserId = this.UserId,
                ConfigurationId = this.ConfigurationId,
                Deleted = this.Deleted
            };
        }

        public UserConfigurationReviewViewModel FromDbModel(ConfigurationReview configReview)
        {
            return new UserConfigurationReviewViewModel()
            {
                IdConfigurationReview = configReview.IdConfigurationReview,
                TimeOfCreation = configReview.TimeOfCreation,
                Rating = configReview.Rating,
                Comment = configReview.Comment,
                UserId = configReview.UserId,
                ConfigurationId = configReview.ConfigurationId,
                Deleted = configReview.Deleted
            };
        }
    }
}
