using System;
using System.DirectoryServices.AccountManagement;
using System.IO;

namespace CodeWars
{
    class Program
    {
        static void Main(string[] args)
        {
            string userName = "alexanderc";
            string domainName = ConfigurationClient.DomainName;
            var principalContext = new PrincipalContext(ContextType.Domain, domainName);
            var user = UserPrincipal.FindByIdentity(principalContext, userName);
            Console.WriteLine(user.EmployeeId);
            var unitPricing = CompassAccessService.GetUnitPricing();
            var count = unitPricing.Count;
            using (var writer = new StreamWriter(@"C:\Users\Alexanderc\UnitPricing.txt", true))
            {
                writer.WriteLine("Unit Pricing Data");
                writer.WriteLine($"IndexID|BalanceDate|ExpiryDate|Amount");
                foreach (var item in unitPricing)
                {
                    count--;
                    Console.WriteLine(item.ToString());
                    Console.WriteLine($"Items left: {count}");
                    writer.Write(item.ToString());
                    writer.WriteLine();
                }
            }
            Console.WriteLine("Process Complete");
            Console.ReadLine();
        }
    }
}
