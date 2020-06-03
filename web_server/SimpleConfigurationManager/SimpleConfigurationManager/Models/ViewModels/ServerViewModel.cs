using SimpleConfigurationManager.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OperatingSystem = SimpleConfigurationManager.Models.DbModels.OperatingSystem;

namespace SimpleConfigurationManager.Models.ViewModels
{
    public class ServerViewModel
    {
        public int IdServer { get; set; }
        public string ServerName { get; set; }
        public string IpAddress { get; set; }
        public bool IsOnline { get; set; }
        public int OperatingSystemId { get; set; }
        public int UserId { get; set; }
        public bool? Deleted { get; set; }
        public OperatingSystem OperatingSystem { get; set; }

        public Server ToDbModel()
        {
            return new Server()
            {
                IdServer = this.IdServer,
                ServerName = this.ServerName,
                IpAddress = this.IpAddress,
                IsOnline = this.IsOnline,
                OperatingSystemId = this.OperatingSystemId,
                UserId = this.UserId,
                Deleted = this.Deleted,
                OperatingSystem = this.OperatingSystem
            };
        }

        public ServerViewModel FromDbModel(Server server)
        {
            return new ServerViewModel()
            {
                IdServer = server.IdServer,
                ServerName = server.ServerName,
                IpAddress = server.IpAddress,
                IsOnline = server.IsOnline,
                OperatingSystemId = server.OperatingSystemId,
                UserId = server.UserId,
                Deleted = server.Deleted,
                OperatingSystem = server.OperatingSystem
            };
        }
    }
}
