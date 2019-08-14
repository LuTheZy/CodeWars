using CodeWars.BusinessLayer;
using CodeWars.JaggedArrays;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeWars
{
    class Program
    {
        static void Main(string[] args)
        {
            string userName = "";
            string domainName = ConfigurationClient.DomainName;
            var principalContext = new PrincipalContext(ContextType.Domain, domainName);
            var user = UserPrincipal.FindByIdentity(principalContext, userName);
            Console.WriteLine(user.EmployeeId);
            Console.ReadLine();
        }
    }
}
