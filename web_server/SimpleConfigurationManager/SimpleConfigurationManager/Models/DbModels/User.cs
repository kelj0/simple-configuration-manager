using System;
using System.Collections.Generic;

namespace SimpleConfigurationManager.Models.DbModels
{
    public partial class User
    {
        public User()
        {
            Configuration = new HashSet<Configuration>();
            ConfigurationReview = new HashSet<ConfigurationReview>();
            Server = new HashSet<Server>();
        }

        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? Deleted { get; set; }
        public string Token { get; set; }
        public DateTime? TimeOfLastLogin { get; set; }

        public virtual ICollection<Configuration> Configuration { get; set; }
        public virtual ICollection<ConfigurationReview> ConfigurationReview { get; set; }
        public virtual ICollection<Server> Server { get; set; }
    }
}
