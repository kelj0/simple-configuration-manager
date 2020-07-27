using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleConfigurationManager.Models.RequestModels
{
    public class VerifyAuthRequestModel
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
