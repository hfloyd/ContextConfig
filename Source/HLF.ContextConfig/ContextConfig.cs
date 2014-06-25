using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLF.ContextConfig
{
    static class ContextConfig
    {
        static string GetValue(string ConfigKey, string EnvironmentName)
        {
            string ReturnValue ="";

            //Get environment in config file
            ContextEnvironment ConfigEnvironment = (ContextConfigEnvironments)ContextConfigSettings.Environments;
            //find matching key

            return ReturnValue;

        }

        internal static ContextConfigSettings GetSettings()
        {
            return new ContextConfigSettings();
        }

        //static void GetDomains()
        //{
        //    try
        //    {
        //        var Settings = ConfigurationManager.GetSection("ContextConfig");
        //        var Domains = Settings

        //        if (appSettings.Count == 0)
        //        {
        //            Console.WriteLine("AppSettings is empty.");
        //        }
        //        else
        //        {
        //            foreach (var key in appSettings.AllKeys)
        //            {
        //                Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
        //            }
        //        }
        //    }
        //    catch (ConfigurationErrorsException)
        //    {
        //        Console.WriteLine("Error reading app settings");
        //    }
        //}

    }
}
