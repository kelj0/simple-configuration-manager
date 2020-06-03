using System;
using System.Collections.Generic;

namespace SimpleConfigurationManager.Models.DbModels
{
    public partial class Server
    {
        public Server()
        {
            ServerConfiguration = new HashSet<ServerConfiguration>();
        }

        public int IdServer { get; set; }
        public string ServerName { get; set; }
        public string IpAddress { get; set; }
        public bool IsOnline { get; set; }
        public int OperatingSystemId { get; set; }
        public int UserId { get; set; }
        public bool? Deleted { get; set; }

        public virtual OperatingSystem OperatingSystem { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ServerConfiguration> ServerConfiguration { get; set; }
    }
}
