using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HLF.ContextConfig
{
    /// <summary>
    /// The 'ContextConfig' static class includes useful functions to test and return data about domains, environments, and configured key/value pairs.
    /// </summary>
    public static class ContextConfig
    {

        #region Domain Info

        /// <summary>
        /// Current active domain url
        /// </summary>
        public static string CurrentDomain = HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString();

        /// <summary>
        /// Check whether the current domain exists in the Domains list
        /// </summary>
        /// <param name="AcceptWildcard">If there is a wildcard (*) domain specified, return true? (Choose false to explicitly search for this url)</param>
        /// <returns></returns>
        public static bool DomainIsConfigured(bool AcceptWildcard = true)
        {
            return DomainIsConfigured(CurrentDomain, AcceptWildcard);
        }

        /// <summary>
        /// Check whether the URL exists in the Domains list
        /// </summary>
        /// <param name="DomainUrl">Url to lookup</param>
        /// <param name="AcceptWildcard">If there is a wildcard (*) domain specified, return true? (Choose false to explicitly search for this url)</param>
        /// <returns></returns>
        public static bool DomainIsConfigured(string DomainUrl, bool AcceptWildcard = true)
        {
            bool ReturnValue = false;

            //Try to get domain from config file
            DomainElement Domain = ConfigSettings.Settings.Domains[DomainUrl];

            if (Domain != null)
            {
                ReturnValue = true;
            }
            else
            {
                if (AcceptWildcard)
                {
                    DomainElement DomainWild = ConfigSettings.Settings.Domains["*"];
                    if (DomainWild != null)
                    {
                        ReturnValue = true;
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Get the Environment name for the current domain
        /// </summary>
        /// <returns></returns>
        public static string DomainEnvironmentName()
        {
            string ReturnValue = "";

            //Get domain from config file
            DomainElement Domain = ConfigSettings.Settings.Domains[CurrentDomain];

            ReturnValue = Domain.Environment;

            return ReturnValue;
        }

        /// <summary>
        /// Get the Environment name for the provided domain url
        /// </summary>
        /// <param name="DomainUrl">Url to lookup</param>
        /// <returns></returns>
        public static string DomainEnvironmentName(string DomainUrl)
        {
            string ReturnValue = "";

            //Get domain from config file
            DomainElement Domain = ConfigSettings.Settings.Domains[DomainUrl];

            if (Domain != null)
            {
                ReturnValue = Domain.Environment;
            }
            else
            {
                //look for wildcard domain
                DomainElement DomainWild = ConfigSettings.Settings.Domains["*"];
                if (DomainWild != null)
                {
                    ReturnValue = DomainWild.Environment;
                }
                else
                {
                    string ErrorMsg = string.Format("Domain '{0}' is not configured, and there is no wildcard (*) domain configured", DomainUrl);
                    throw new MissingDomainConfigException(ErrorMsg);
                }
            }
            
            return ReturnValue;
        }

        /// <summary>
        /// Get the Site Name for the current domain
        /// </summary>
        /// <returns></returns>
        public static string DomainSiteName()
        {
            string ReturnValue = "";

            //Get domain from config file
            DomainElement Domain = ConfigSettings.Settings.Domains[CurrentDomain];

            ReturnValue = Domain.SiteName;

            return ReturnValue;
        }

        /// <summary>
        /// Get the Site Name for the provided domain url
        /// </summary>
        /// <param name="DomainUrl">Url to lookup</param>
        /// <returns></returns>
        public static string DomainSiteName(string DomainUrl)
        {
            string ReturnValue = "";

            //Get domain from config file
            DomainElement Domain = ConfigSettings.Settings.Domains[DomainUrl];

            if (Domain != null)
            {
                ReturnValue = Domain.SiteName;
            }
            else
            {
                //look for wildcard domain
                DomainElement DomainWild = ConfigSettings.Settings.Domains["*"];
                if (DomainWild != null)
                {
                    ReturnValue = DomainWild.SiteName;
                }
                else
                {
                    string ErrorMsg = string.Format("Domain '{0}' is not configured, and there is no wildcard (*) domain configured", DomainUrl);
                    throw new MissingDomainConfigException(ErrorMsg);
                }
            }

            return ReturnValue;
        }

        #endregion

        #region Environment Info

        /// <summary>
        /// Check whether the current environment exists in the Environments list
        /// </summary>
        /// <param name="AcceptDefault">If there is a "default" domain specified, return true? (Choose false to explicitly search for this environment)</param>
        /// <returns></returns>
        public static bool EnvironmentIsConfigured(bool AcceptDefault = true)
        {
            return EnvironmentIsConfigured(DomainEnvironmentName(), AcceptDefault);
        }

        /// <summary>
        /// Check whether the current environment exists in the Environments list
        /// </summary>
        /// <param name="EnvironmentName">Name to lookup</param>
        /// <param name="AcceptDefault">If there is a "default" domain specified, return true? (Choose false to explicitly search for this environment)</param>
        /// <returns></returns>
        public static bool EnvironmentIsConfigured(string EnvironmentName, bool AcceptDefault = true)
        {
            bool ReturnValue = false;

            //Try to get the environment from config file
            EnvironmentElement Env = ConfigSettings.Settings.Environments[EnvironmentName];

            if (Env != null)
            {
                ReturnValue = true;
            }
            else
            {
                if (AcceptDefault)
                {
                    EnvironmentElement EnvDefault = ConfigSettings.Settings.Environments["default"];
                    if (EnvDefault != null)
                    {
                        ReturnValue = true;
                    }
                }
            }

            return ReturnValue;
        }

        internal static EnvironmentElement GetDomainEnvironment(string DomainUrl)
        {
            string EnvName = "";
            try
            {
                //Get domain from config file
                DomainElement Domain = ConfigSettings.Settings.Domains[DomainUrl];
                EnvName = Domain.Environment;
            }
            catch (Exception )
            {
                try
                {
                    DomainElement Domain = ConfigSettings.Settings.Domains["*"];
                    EnvName = Domain.Environment;
                }
                catch (Exception Exception2)
                {
                    string ErrorMsg = string.Format("Domain '{0}' is not configured, and there is no wildcard (*) domain configured", DomainUrl);
                    throw new MissingDomainConfigException(ErrorMsg, Exception2);
                }
            }

            if (EnvName != "")
            {
                EnvironmentElement ReturnValue = ConfigSettings.Settings.Environments[EnvName];
                return ReturnValue;
            }
            else
            {
                return null;
            }
        }

        internal static EnvironmentElement GetEnvironmentByName(string EnvironmentName)
        {
            EnvironmentElement ReturnValue;

            try
            {
                //Get environment from config file
                ReturnValue = ConfigSettings.Settings.Environments[EnvironmentName];
            }
            catch (Exception )
            {
                try
                {
                    //look for default environment
                    ReturnValue = ConfigSettings.Settings.Environments["default"];
                }
                catch (Exception Exception2)
                {
                    string ErrorMsg = string.Format("Environment '{0}' is not configured, and there is no 'default' environment configured", EnvironmentName);
                    throw new MissingEnvironmentConfigException(ErrorMsg, Exception2);
                }
            }

            return ReturnValue;
        }

        #endregion

        #region Key/Value Configs

        /// <summary>
        /// Get the value for a given key on the current domain
        /// </summary>
        /// <param name="ConfigKey">Key name</param>
        /// <returns></returns>
        public static string GetValue(string ConfigKey)
        {
            string ReturnValue = "";
            ReturnValue = GetValue(ConfigKey, DomainEnvironmentName());

            return ReturnValue;
        }

        /// <summary>
        /// Get the value when providing a key and environment name
        /// </summary>
        /// <param name="ConfigKey">Key name</param>
        /// <param name="EnvironmentName">Environment name</param>
        /// <returns></returns>
        public static string GetValue(string ConfigKey, string EnvironmentName)
        {
            string ReturnValue = "";

            //Get environment from config file
            EnvironmentElement Env = GetEnvironmentByName(EnvironmentName);

            try
            {
                //find matching key
                KeyValueElement KeyValue = Env.Configs[ConfigKey];

                if (KeyValue != null)
                {
                    ReturnValue = KeyValue.Value;
                }
                else
                {
                    //Look for 'default' environment
                    EnvironmentElement EnvDefault = GetEnvironmentByName("default");

                    //find matching key
                    KeyValueElement KeyValueDefault = EnvDefault.Configs[ConfigKey];

                    ReturnValue = KeyValueDefault.Value;
                }
            }
            catch (Exception Exception1)
            {
                string ErrorMsg = string.Format("Key '{0}' is not configured for Environment '{1}', and there is no default key configured", ConfigKey, EnvironmentName);
                throw new MissingConfigKeyException(ErrorMsg, Exception1);
            }

            return ReturnValue;
        }

        
        /// <summary>
        /// Get all the KeyValue elements for the current Environment
        /// </summary>
        /// <param name="IncludeDefaults">Include all ‘default’ KeyValue configs for keys not specifically defined for the environment.</param>
        /// <returns></returns>
        public static List<KeyValueElement> AllEnvironmentConfigs(bool IncludeDefaults = true)
        {
            return AllEnvironmentConfigs(DomainEnvironmentName(), IncludeDefaults);
        }

        /// <summary>
        /// Get all the KeyValue elements for the current Environment
        /// </summary>
        /// <param name="EnvironmentName">Environment to get values from</param>
        /// <param name="IncludeDefaults">Include all ‘default’ KeyValue configs for keys not specifically defined for the environment.</param>
        /// <returns></returns>
        public static List<KeyValueElement> AllEnvironmentConfigs(string EnvironmentName, bool IncludeDefaults = true)
        {
            List<KeyValueElement> ReturnList = new List<KeyValueElement>();

            EnvironmentElement Env = ConfigSettings.Settings.Environments[EnvironmentName];
            KeyValueElementCollection KvColl = Env.Configs;

            foreach (KeyValueElement KeyValue in Env.Configs)
            {
                ReturnList.Add(KeyValue);
            }

            if (IncludeDefaults)
            {
                EnvironmentElement EnvDefault = ConfigSettings.Settings.Environments["default"];

                foreach (KeyValueElement KeyValueDef in EnvDefault.Configs)
                {
                    if (KvColl[KeyValueDef.Key]==null)
                    {
                        ReturnList.Add(KeyValueDef);
                    }
                }
            }

            return ReturnList;
        }

        #endregion

        #region SMTP settings
        /// <summary>
        /// Get the SMTP Settings for the current domain
        /// </summary>
        /// <param name="IncludeDefaults">Include all ‘default’ smtp configs for properties not specifically defined for the current domain.</param>
        /// <returns></returns>
        public static SmtpSettingsElement GetSmtpSettings(bool IncludeDefaults = true)
        {
            SmtpSettingsElement ReturnValue = null;
            ReturnValue = GetSmtpSettings(DomainEnvironmentName(), IncludeDefaults);

            return ReturnValue;
        }

        /// <summary>
        /// Get the SMTP Settings for a given environment
        /// </summary>
        /// <param name="EnvironmentName">Environment to get smtp config from</param>
        /// <param name="IncludeDefaults">Include all ‘default’ smtp configs for properties not specifically defined for the current domain.</param>
        /// <returns></returns>
        public static SmtpSettingsElement GetSmtpSettings(string EnvironmentName, bool IncludeDefaults = true)
        {
            SmtpSettingsElement ReturnValue = new SmtpSettingsElement();

            EnvironmentElement Env = ConfigSettings.Settings.Environments[EnvironmentName];
            EnvironmentElement DefaultEnv = ConfigSettings.Settings.Environments["default"];

            // fills the return value with the values from the requested environment
            if (Env != null)
                FillUndefinedSmtpSettings(ReturnValue, Env.SmtpSettings);
            // fills the still undefined properties of the return value with the values from the default environment
            if (IncludeDefaults && DefaultEnv != null)
                FillUndefinedSmtpSettings(ReturnValue, DefaultEnv.SmtpSettings);

            return ReturnValue;
        }

        /// <summary>
        /// Performs a property per property fill of undefined properties of an SmtpSettingsElement based on the values of another one
        /// </summary>
        /// <param name="Target">The SmtpSettingsElement to fill</param>
        /// <param name="Source">The SmtpSettingsElement to get values from</param>
        private static void FillUndefinedSmtpSettings(SmtpSettingsElement Target, SmtpSettingsElement Source)
        {
            if (Target.IsPropertyUndefined(SmtpSettingsElement.DeliveryFormatPropertyName) && !Source.IsPropertyUndefined(SmtpSettingsElement.DeliveryFormatPropertyName))
                Target.DeliveryFormat= Source.DeliveryFormat;

            if (Target.IsPropertyUndefined(SmtpSettingsElement.DeliveryMethodPropertyName) && !Source.IsPropertyUndefined(SmtpSettingsElement.DeliveryMethodPropertyName))
                Target.DeliveryMethod= Source.DeliveryMethod;

            if (Target.IsPropertyUndefined(SmtpSettingsElement.FromPropertyName) && !Source.IsPropertyUndefined(SmtpSettingsElement.FromPropertyName))
                Target.From = Source.From;

            if (Target.Network.IsPropertyUndefined(SmtpNetworkElement.ClientDomainPropertyName) && !Source.Network.IsPropertyUndefined(SmtpNetworkElement.ClientDomainPropertyName))
                Target.Network.ClientDomain = Source.Network.ClientDomain;

            if (Target.Network.IsPropertyUndefined(SmtpNetworkElement.DefaultCredentialsPropertyName) && !Source.Network.IsPropertyUndefined(SmtpNetworkElement.DefaultCredentialsPropertyName))
                Target.Network.UseDefaultCredentials = Source.Network.UseDefaultCredentials;

            if (Target.Network.IsPropertyUndefined(SmtpNetworkElement.EnableSslPropertyName) && !Source.Network.IsPropertyUndefined(SmtpNetworkElement.EnableSslPropertyName))
                Target.Network.EnableSsl = Source.Network.EnableSsl;

           if (Target.Network.IsPropertyUndefined(SmtpNetworkElement.HostPropertyName) && !Source.Network.IsPropertyUndefined(SmtpNetworkElement.HostPropertyName))
                Target.Network.Host = Source.Network.Host;

            if (Target.Network.IsPropertyUndefined(SmtpNetworkElement.PasswordPropertyName) && !Source.Network.IsPropertyUndefined(SmtpNetworkElement.PasswordPropertyName))
                Target.Network.Password = Source.Network.Password;

            if (Target.Network.IsPropertyUndefined(SmtpNetworkElement.PortPropertyName) && !Source.Network.IsPropertyUndefined(SmtpNetworkElement.PortPropertyName))
                Target.Network.Port = Source.Network.Port;

            if (Target.Network.IsPropertyUndefined(SmtpNetworkElement.TargetNamePropertyName) && !Source.Network.IsPropertyUndefined(SmtpNetworkElement.TargetNamePropertyName))
                Target.Network.TargetName = Source.Network.TargetName;

            if (Target.Network.IsPropertyUndefined(SmtpNetworkElement.UserNamePropertyName) && !Source.Network.IsPropertyUndefined(SmtpNetworkElement.UserNamePropertyName))
                Target.Network.UserName = Source.Network.UserName;

            if (Target.SpecifiedPickupDirectory.IsPropertyUndefined(SmtpSpecifiedPickupDirectoryElement.PickupDirectoryLocationPropertyName) && !Source.SpecifiedPickupDirectory.IsPropertyUndefined(SmtpSpecifiedPickupDirectoryElement.PickupDirectoryLocationPropertyName))
                Target.SpecifiedPickupDirectory.PickupDirectoryLocation = Source.SpecifiedPickupDirectory.PickupDirectoryLocation;
        }
        #endregion
    }

    #region *** Custom Exceptions ***

    [Serializable]
    internal class MissingDomainConfigException : Exception
    {
        // Use the default ApplicationException constructors
        public MissingDomainConfigException() : base()
        {
        }

        public MissingDomainConfigException(string s) : base(s)
        {
        }

        public MissingDomainConfigException(string s, Exception ex) : base(s, ex)
        {
        }
    }

    [Serializable]
    internal class MissingEnvironmentConfigException : Exception
    {
        // Use the default ApplicationException constructors
        public MissingEnvironmentConfigException() : base()
        {
        }

        public MissingEnvironmentConfigException(string s) : base(s)
        {
        }

        public MissingEnvironmentConfigException(string s, Exception ex) : base(s, ex)
        {
        }
    }

    [Serializable]
    internal class MissingConfigKeyException : Exception
    {
        // Use the default ApplicationException constructors
        public MissingConfigKeyException() : base()
        {
        }

        public MissingConfigKeyException(string s) : base(s)
        {
        }

        public MissingConfigKeyException(string s, Exception ex) : base(s, ex)
        {
        }
    }

    #endregion

}

