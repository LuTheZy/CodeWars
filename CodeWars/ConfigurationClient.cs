using System.Configuration;

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
