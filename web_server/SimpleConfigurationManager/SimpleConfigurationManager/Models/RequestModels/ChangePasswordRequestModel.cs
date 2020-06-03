using SimpleConfigurationManager.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleConfigurationManager.Models.RequestModels
{
    public class ChangePasswordRequestModel
    {
        public int IdUser { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

        public User ToDbModel(User user)
        {
            user.Password = this.NewPassword;
            return user;
        }
    }
}
