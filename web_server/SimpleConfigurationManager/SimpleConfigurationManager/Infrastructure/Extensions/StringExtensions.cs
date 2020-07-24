﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleConfigurationManager.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string GetSha1(this string value)
        {
            var data = Encoding.ASCII.GetBytes(value);
            var hashData = new SHA1Managed().ComputeHash(data);
            var hash = string.Empty;

            foreach (var b in hashData)
            {
                hash += b.ToString("X2");
            }
            return hash;
        }
    }
}
