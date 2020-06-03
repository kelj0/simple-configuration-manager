using SimpleConfigurationManager.Models.DbModels;

namespace SimpleConfigurationManager.Models.ViewModels
{
    public class UserBasicInfoViewModel
    {
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public UserBasicInfoViewModel FromDbModel(User user)
        {
            return new UserBasicInfoViewModel()
            {
                IdUser = user.IdUser,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
        }
    }
}
