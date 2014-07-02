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
            get { return (string) base["version"]; }
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
