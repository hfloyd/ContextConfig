using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;

namespace HLF.ContextConfig
{
    /// <summary>
    /// The 'ConfigSettings' class give you direct access to all the configured data using collections and dot(.) notation.
    /// *Note: you will need to explicitly declare objects of the element types in order to use their properties.
    /// example:
    /// foreach (DomainElement Domain in ConfigSettings.Settings.Domains)
    /// { string EnvironmentName =  Domain.Environment;}
    /// </summary>
    public class ConfigSettings : ConfigurationSection
    {
        //Config File Object
        private static string _configfile = "ContextConfig.config";
        private static ConfigSettings _Config;
        private static Configuration config;

        /// <summary>
        /// Represents all the ConfigSettings
        /// </summary>
        public static ConfigSettings Settings = GetSettings();

        internal static ConfigSettings GetSettings()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = HttpContext.Current.Server.MapPath(string.Format("~/config/{0}", _configfile));

            if (! System.IO.File.Exists(fileMap.ExeConfigFilename))
            {
                //if file doesn't exist in /config/ folder, check in site root.
                fileMap.ExeConfigFilename = HttpContext.Current.Server.MapPath(string.Format("~/{0}", _configfile));
            }

            if (System.IO.File.Exists(fileMap.ExeConfigFilename))
                {
                    // load the settings file
                    config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                    if (config != null)
                    {
                        _Config = (ConfigSettings)config.GetSection("ContextConfig");
                    }
                }
            else
            {
                string ErrorMsg = string.Format("The 'ContextConfig.config' file cannot be found. Please add it to the '~/config/' folder or site root.");
                throw new MissingConfigFileException(ErrorMsg);
            }

            if (_Config == null)
            {
                string ErrorMsg = string.Format("There is something wrong with the 'ContextConfig.config' file. Please check that a properly formatted file is located in the '~/config/' folder or site root.");
                throw new MissingConfigFileException(ErrorMsg);
                //_Config = new ConfigSettings(); // default config - won't be savable mind?
            }

            return _Config;
        }

        #region *** ConfigurationProperties ***

        /// <summary>
        /// &lt;ContextConfig&gt; 'version' attribute
        /// </summary>
        [ConfigurationProperty("version")]
        public string Version
        {
            get { return (string)base["version"]; }
        }

        /// <summary>
        /// &lt;ContextConfig&gt; 'OverrideConfigurationManager' attribute
        /// </summary>
        [ConfigurationProperty("OverrideConfigurationManager")]
        public bool OverrideConfigurationManager
        {
            get
            {
                //string ConfigValue = (string)base["OverrideConfigurationManager"];
                //return Convert.ToBoolean(ConfigValue);
                return (bool)base["OverrideConfigurationManager"];
            }
        }

        /// <summary>
        /// &lt;ContextConfig&gt; &lt;Domains&gt; collection
        /// </summary>
        [ConfigurationProperty("Domains")]
        public DomainElementCollection Domains
        {
            get { return (DomainElementCollection) base["Domains"]; }
        }

        /// <summary>
        /// &lt;ContextConfig&gt; &lt;Environments&gt; collection
        /// </summary>
        [ConfigurationProperty("Environments")]
        public EnvironmentElementCollection Environments
        {
            get { return (EnvironmentElementCollection) base["Environments"]; }
        }

        #endregion

    }

    #region *** Element Objects ***

    /// <summary>
    /// Represents a Domain from the config file
    /// </summary>
    public class DomainElement : ConfigurationElement
    {
        /// <summary>
        /// &lt;Domain&gt; 'url' attribute
        /// </summary>
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get { return (string) base["url"]; }
        }

        /// <summary>
        /// &lt;Domain&gt; 'environment' attribute
        /// </summary>
        [ConfigurationProperty("environment", IsRequired = true)]
        public string Environment
        {
            get { return (string) base["environment"]; }
        }

        /// <summary>
        /// &lt;Domain&gt; 'sitename' attribute
        /// </summary>
        [ConfigurationProperty("sitename", IsRequired = false)]
        public string SiteName
        {
            get { return (string)base["sitename"]; }
        }
    }

    /// <summary>
    /// Represents a defined Environment from the config file
    /// </summary>
    public class EnvironmentElement : ConfigurationElement
    {
      
        /// <summary>
        /// &lt;Environment&gt; 'name' attribute
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string) base["name"]; }
        }

        /// <summary>
        /// &lt;Environment&gt; &lt;Configs&gt; (key/values) collection
        /// </summary>
        [ConfigurationProperty("Configs")]
        public KeyValueElementCollection Configs
        {
            get { return (KeyValueElementCollection) base["Configs"]; }
        }

        /// <summary>
        /// &lt;Environment&gt; &lt;SmtpSettings&gt; element
        /// </summary>
        [ConfigurationProperty("SmtpSettings", IsRequired=false)]
        public SmtpSettingsElement SmtpSettings
        {
            get { return (SmtpSettingsElement)base["SmtpSettings"]; }
        }
    }

    /// <summary>
    /// Represents a Key/Value pair defined for an environment in the config file
    /// </summary>
    public class KeyValueElement : ConfigurationElement
    {
        
        /// <summary>
        /// &lt;Environment&gt; &lt;add&gt; 'key' attribute
        /// </summary>
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return (string) base["key"]; }
        }
        
        /// <summary>
        /// &lt;Environment&gt; &lt;add&gt;'value' attribute
        /// </summary>
        [ConfigurationProperty("value", IsRequired = false)]
        public string Value
        {
            get { return (string) base["value"]; }
        }
    }

    public abstract class SmtpConfigurationElement : ConfigurationElement
    {
        System.Net.Mail.SmtpClient _smtpDefaults;

        /// <summary>
        /// Returns an smtp client object. Helper object used to return default smtp config values in case they are
        /// not provided in this section.
        /// </summary>
        protected System.Net.Mail.SmtpClient SmtpDefaults
        {
            get
            {
                if (_smtpDefaults == null)
                    _smtpDefaults = new System.Net.Mail.SmtpClient();
                return _smtpDefaults;
            }
        }

        /// <summary>
        /// Checks whether a specific configuration property was provided or was absent from the configuration element
        /// </summary>
        /// <param name="PropertyName">Name of the property to check on</param>
        /// <returns></returns>
        internal bool IsPropertyUndefined(string PropertyName)
        {
            var property = this.ElementInformation.Properties[PropertyName];

            return property == null || property.ValueOrigin != PropertyValueOrigin.SetHere;
        }
    } 

    /// <summary>
    /// Represents the SmtpSettings defined for an environment in the config file
    /// </summary>
    public class SmtpSettingsElement : SmtpConfigurationElement
    {
        #region const
        internal const string DeliveryMethodPropertyName = "deliveryMethod", DeliveryFormatPropertyName = "deliveryFormat", FromPropertyName = "from";
        protected const string NetworkPropertyName = "network", SpecifiedPickupDirectoryPropertyName = "specifiedPickupDirectory";
        #endregion


        /// <summary>
        /// &lt;SmtpSettings&gt; 'deliveryMethod' attribute.
        /// Defaults to the base smtp settings value if not provided in this config section.
        /// </summary>
        [ConfigurationProperty(DeliveryMethodPropertyName, IsRequired = false)]
        public System.Net.Mail.SmtpDeliveryMethod DeliveryMethod
        {
            get
            {
                if (IsPropertyUndefined(DeliveryMethodPropertyName))
                    return SmtpDefaults.DeliveryMethod;  // return default value

                return (System.Net.Mail.SmtpDeliveryMethod)base[DeliveryMethodPropertyName];
            }
            set
            {
                base[DeliveryMethodPropertyName] = value;
            }
        }

        /// <summary>
        /// &lt;SmtpSettings&gt; 'deliveryFormat' attribute.
        /// Defaults to the base smtp settings value if not provided in this config section.
        /// </summary>
        [ConfigurationProperty(DeliveryFormatPropertyName, IsRequired = false)]
        public System.Net.Mail.SmtpDeliveryFormat DeliveryFormat
        {
            get
            {
                if (IsPropertyUndefined(DeliveryFormatPropertyName))
                    return SmtpDefaults.DeliveryFormat; // return default value

                return (System.Net.Mail.SmtpDeliveryFormat)base[DeliveryFormatPropertyName];
            }
            set
            {
                base[DeliveryFormatPropertyName] = value;
            }
        }

        /// <summary>
        /// &lt;SmtpSettings&gt; 'from' attribute.
        /// Defaults to the base smtp settings value if not provided in this config section.
        /// </summary>
        [ConfigurationProperty(FromPropertyName, IsRequired = false)]
        public string From
        {
            get
            {
                if (IsPropertyUndefined(FromPropertyName))
                {
                    // return default value, if provided
                    var defaultFrom = new System.Net.Mail.MailMessage().From;
                    if (defaultFrom != null)
                        return defaultFrom.Address; // we have a default from address
                }

                return (string)base[FromPropertyName];
            }
            set
            {
                base[FromPropertyName] = value;
            }
        }

        /// <summary>
        /// &lt;SmtpSettings&gt; 'network' element.
        /// Defaults to the base smtp network settings value if not provided in this config section.
        /// </summary>
        [ConfigurationProperty(NetworkPropertyName, IsRequired = false)]
        public SmtpNetworkElement Network
        {
            get
            {
                return (SmtpNetworkElement)base[NetworkPropertyName];
            }
        }

        /// <summary>
        /// &lt;SmtpSettings&gt; 'specifiedPickupDirectory' element.
        /// Defaults to the base smtp specifiedPickupDirectory settings value if not provided in this config section.
        /// </summary>
        [ConfigurationProperty(SpecifiedPickupDirectoryPropertyName, IsRequired = false)]
        public SmtpSpecifiedPickupDirectoryElement SpecifiedPickupDirectory
        {
            get { return (SmtpSpecifiedPickupDirectoryElement)base[SpecifiedPickupDirectoryPropertyName]; }
        }
    }

    /// <summary>
    /// Represents the Network configuration for SmtpSettings defined for an environment in the config file
    /// </summary>
    public class SmtpNetworkElement : SmtpConfigurationElement
    {
        #region const
        internal const string ClientDomainPropertyName = "clientDomain", UserNamePropertyName = "userName", PasswordPropertyName = "password",
                                DefaultCredentialsPropertyName = "defaultCredentials", EnableSslPropertyName = "enableSsl", HostPropertyName = "host",
                                PortPropertyName = "port", TargetNamePropertyName = "targetName";
        #endregion

        /// <summary>
        /// &lt;Network&gt; 'clientDomain' attribute.
        /// Defaults to the base smtp settings value if not provided in this config section.
        /// </summary>
        [ConfigurationProperty(ClientDomainPropertyName, IsRequired = false)]
        public string ClientDomain
        {
            get
            {
                if (UseDefaultCredentials)
                    return string.Empty;

                if (IsPropertyUndefined(ClientDomainPropertyName))
                {
                    var defaultCredentials = (System.Net.NetworkCredential)SmtpDefaults.Credentials;

                    if (defaultCredentials != null)
                        return defaultCredentials.Domain;
                }

                return (string)base[ClientDomainPropertyName];
            }
            set
            {
                base[ClientDomainPropertyName] = value;
            }
        }

        /// <summary>
        /// &lt;Network&gt; 'userName' attribute.
        /// Defaults to the base smtp settings value if not provided in this config section.
        /// </summary>
        [ConfigurationProperty(UserNamePropertyName, IsRequired = false)]
        public string UserName
        {
            get
            {
                if (UseDefaultCredentials)
                    return string.Empty;

                if (IsPropertyUndefined(UserNamePropertyName))
                {
                    var defaultCredentials = (System.Net.NetworkCredential)SmtpDefaults.Credentials;

                    if (defaultCredentials != null)
                        return defaultCredentials.UserName;
                }

                return (string)base[UserNamePropertyName];
            }
            set
            {
                base[UserNamePropertyName] = value;
            }
        }

        /// <summary>
        /// &lt;Network&gt; 'password' attribute.
        /// Defaults to the base smtp settings value if not provided in this config section.
        /// </summary>
        [ConfigurationProperty(PasswordPropertyName, IsRequired = false)]
        public string Password
        {
            get
            {
                if (UseDefaultCredentials)
                    return string.Empty;

                if (IsPropertyUndefined(PasswordPropertyName))
                {
                    var defaultCredentials = (System.Net.NetworkCredential)SmtpDefaults.Credentials;

                    if (defaultCredentials != null)
                        return defaultCredentials.Password;
                }

                return (string)base[PasswordPropertyName];
            }
            set
            {
                base[PasswordPropertyName] = value;
            }
        }

        /// <summary>
        /// &lt;Network&gt; 'defaultCredentials' attribute.
        /// Defaults to the base smtp settings value if not provided in this config section.
        /// </summary>
        [ConfigurationProperty(DefaultCredentialsPropertyName, IsRequired = false)]
        public bool UseDefaultCredentials
        {
            get
            {
                if (IsPropertyUndefined(DefaultCredentialsPropertyName))
                    return SmtpDefaults.UseDefaultCredentials;

                return (bool)base[DefaultCredentialsPropertyName];
            }
            set
            {
                base[DefaultCredentialsPropertyName] = value;
            }
        }

        /// <summary>
        /// &lt;Network&gt; 'enableSsl' attribute.
        /// Defaults to the base smtp settings value if not provided in this config section.
        /// </summary>
        [ConfigurationProperty(EnableSslPropertyName, IsRequired = false)]
        public bool EnableSsl
        {
            get
            {
                if (IsPropertyUndefined(EnableSslPropertyName))
                    return SmtpDefaults.EnableSsl;

                return (bool)base[EnableSslPropertyName];
            }
            set
            {
                base[EnableSslPropertyName] = value;
            }
        }

        /// <summary>
        /// &lt;Network&gt; 'host' attribute.
        /// Defaults to the base smtp settings value if not provided in this config section.
        /// </summary>
        [ConfigurationProperty(HostPropertyName, IsRequired = false)]
        public string Host
        {
            get
            {
                if (IsPropertyUndefined(HostPropertyName))
                    return SmtpDefaults.Host;

                return (string)base[HostPropertyName];
            }
            set
            {
                base[HostPropertyName] = value;
            }
        }

        /// <summary>
        /// &lt;Network&gt; 'port' attribute.
        /// Defaults to the base smtp settings value if not provided in this config section.
        /// </summary>
        [ConfigurationProperty(PortPropertyName, IsRequired = false)]
        public int Port
        {
            get
            {
                if (IsPropertyUndefined(PortPropertyName))
                    return SmtpDefaults.Port;

                return (int)base[PortPropertyName];
            }
            set
            {
                base[PortPropertyName] = value;
            }
        }

        /// <summary>
        /// &lt;Network&gt; 'targetName' attribute.
        /// Defaults to the base smtp settings value if not provided in this config section.
        /// </summary>
        [ConfigurationProperty(TargetNamePropertyName, IsRequired = false)]
        public string TargetName
        {
            get
            {
                if (IsPropertyUndefined(TargetNamePropertyName))
                    return SmtpDefaults.TargetName;

                return (string)base[TargetNamePropertyName];
            }
            set
            {
                base[TargetNamePropertyName] = value;
            }
        }
    }

    /// <summary>
    /// Represents the SpecifiedPickupDirectory configuration for SmtpSettings defined for an environment in the config file
    /// </summary>
    public class SmtpSpecifiedPickupDirectoryElement : SmtpConfigurationElement
    {
        #region const
        internal const string PickupDirectoryLocationPropertyName = "pickupDirectoryLocation";
        #endregion

        /// <summary>
        /// &lt;SpecifiedPickupDirectory&gt; 'pickupDirectoryLocation' attribute.
        /// Defaults to the base smtp settings value if not provided in this config section.
        /// </summary>
        [ConfigurationProperty(PickupDirectoryLocationPropertyName, IsRequired = false)]
        public string PickupDirectoryLocation
        {
            get
            {
                if (IsPropertyUndefined(PickupDirectoryLocationPropertyName))
                    return SmtpDefaults.PickupDirectoryLocation;

                return (string)base[PickupDirectoryLocationPropertyName];
            }
            set
            {
                base[PickupDirectoryLocationPropertyName] = value;
            }
        }
    }
    #endregion

    #region *** Element Collections ***

    /// <summary>
    /// Represents all the Domains defined in the config file
    /// </summary>
    [ConfigurationCollection(typeof (DomainElement), AddItemName = "Domain",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class DomainElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Gets the name used to identify this collection of elements in the configuration file when overridden in a derived class.
        /// </summary>
        /// <returns>
        /// The name of the collection; otherwise, an empty string. The default is an empty string.
        /// </returns>
        protected override string ElementName
        {
            get { return "Domain"; }
        }

        //Basic Stuff
        /// <summary>
        /// Gets the type of the <see cref="T:System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Configuration.ConfigurationElementCollectionType"/> of this collection.
        /// </returns>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new DomainElement();
        }

        /// <summary>
        /// Integer index
        /// </summary>
        /// <param name="index"></param>
        public DomainElement this[int index]
        {
            get { return (DomainElement) base.BaseGet(index); }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                base.BaseAdd(index, value);
            }
        }

        //Custom Key
        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement"/> to return the key for. </param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as DomainElement).Url;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public new DomainElement this[string url]
        {
            get { return (DomainElement) base.BaseGet(url); }
        }
    }

    /// <summary>
    /// Represents all the Environments defined in the config file
    /// </summary>
    [ConfigurationCollection(typeof (EnvironmentElement), AddItemName = "Environment",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class EnvironmentElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Gets the name used to identify this collection of elements in the configuration file when overridden in a derived class.
        /// </summary>
        /// <returns>
        /// The name of the collection; otherwise, an empty string. The default is an empty string.
        /// </returns>
        protected override string ElementName
        {
            get { return "Environment"; }
        }

        //Basic Stuff
        /// <summary>
        /// Gets the type of the <see cref="T:System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Configuration.ConfigurationElementCollectionType"/> of this collection.
        /// </returns>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new EnvironmentElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public EnvironmentElement this[int index]
        {
            get { return (EnvironmentElement) base.BaseGet(index); }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                base.BaseAdd(index, value);
            }
        }

        //Custom Key
        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement"/> to return the key for. </param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as EnvironmentElement).Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public new EnvironmentElement this[string name]
        {
            get { return (EnvironmentElement) base.BaseGet(name); }
        }
    }

    /// <summary>
    /// Represents all the Key/Value pair elements defined in the config file
    /// </summary>
    [ConfigurationCollection(typeof (KeyValueElement), AddItemName = "add",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class KeyValueElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Gets the name used to identify this collection of elements in the configuration file when overridden in a derived class.
        /// </summary>
        /// <returns>
        /// The name of the collection; otherwise, an empty string. The default is an empty string.
        /// </returns>
        protected override string ElementName
        {
            get { return "add"; }
        }

        //Basic Stuff
        /// <summary>
        /// Gets the type of the <see cref="T:System.Configuration.ConfigurationElementCollection"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Configuration.ConfigurationElementCollectionType"/> of this collection.
        /// </returns>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        /// <summary>
        /// When overridden in a derived class, creates a new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new KeyValueElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public KeyValueElement this[int index]
        {
            get { return (KeyValueElement) base.BaseGet(index); }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                base.BaseAdd(index, value);
            }
        }

        //Custom Key
        /// <summary>
        /// Gets the element key for a specified configuration element when overridden in a derived class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Object"/> that acts as the key for the specified <see cref="T:System.Configuration.ConfigurationElement"/>.
        /// </returns>
        /// <param name="element">The <see cref="T:System.Configuration.ConfigurationElement"/> to return the key for. </param>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as KeyValueElement).Key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public new KeyValueElement this[string key]
        {
            get { return (KeyValueElement) base.BaseGet(key); }
        }
    }

    #endregion

    #region *** Custom Exceptions ***

    [Serializable]
    internal class MissingConfigFileException : Exception
    {
        // Use the default ApplicationException constructors
        public MissingConfigFileException()
            : base()
        {
        }

        public MissingConfigFileException(string s)
            : base(s)
        {
        }

        public MissingConfigFileException(string s, Exception ex)
            : base(s, ex)
        {
        }
    }

    #endregion
}
