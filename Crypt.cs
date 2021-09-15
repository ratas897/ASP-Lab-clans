using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace ASP_Lab_clans
{
    public static class Crypt
    {
        public static string Hash(string value)
        {
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(value)));
        }
    }
}