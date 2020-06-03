using SimpleConfigurationManager.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleConfigurationManager.Models.ViewModels
{
    public class UserViewModel
    {
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public IEnumerable<ConfigurationViewModel> UserConfigurations { get; set; }
        public IEnumerable<UserConfigurationReviewViewModel> UserReviews { get; set; }
        public IEnumerable<ServerViewModel> UserServers { get; set; }

        public UserViewModel()
        {
            this.UserConfigurations = new List<ConfigurationViewModel>();
            this.UserReviews = new List<UserConfigurationReviewViewModel>();
            this.UserServers = new List<ServerViewModel>();
        }

        public UserViewModel FromDbModel(User user)
        {
            return new UserViewModel()
            {
                IdUser = user.IdUser,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserConfigurations = (user.Configuration.Select(config => new ConfigurationViewModel().FromDbModel(config))).ToList(),
                UserReviews = (user.ConfigurationReview.Select(review => new UserConfigurationReviewViewModel().FromDbModel(review))).ToList(),
                UserServers = (user.Server.Select(server => new ServerViewModel().FromDbModel(server))).ToList()
        };
        }
    }
}
