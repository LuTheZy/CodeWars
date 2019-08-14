using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars.BusinessLayer
{
    public class ConfigurationClient
    {
        public static string DomainName
        {
            get
            {
               return ConfigurationManager.AppSettings["DomainName"];
            }
        }
    }
}
