﻿using SimpleConfigurationManager.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleConfigurationManager.Models.RequestModels
{
    public class EditServerRequestModel
    {
        public int IdServer { get; set; }
        public string ServerName { get; set; }
        public string IpAddress { get; set; }
        public bool IsOnline { get; set; }
        public int OperatingSystemId { get; set; }
        public int UserId { get; set; }

        public Server ToDbModel()
        {
            return new Server()
            {
                IdServer = this.IdServer,
                ServerName = this.ServerName,
                IpAddress = this.IpAddress,
                IsOnline = this.IsOnline,
                OperatingSystemId = this.OperatingSystemId,
                UserId = this.UserId
            };
        }
    }
}
