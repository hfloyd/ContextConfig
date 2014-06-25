using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;

//using Umbraco.Core.IO;
//using Umbraco.Core.Logging;
//using umbraco.BusinessLogic; 

namespace HLF.ContextConfig
{
    /// <summary>
    /// Config Settings - 
    /// 
    /// reads the config file
    /// 
    /// <uSync>
    ///     <Settings>
    ///         <add 
    ///             read="true"                     - read the uSync directory on startup
    ///             write="false"                   - write the uSync directory on statup
    ///             attach="true"                   - attach the events to save on the fly
    ///             folder="~/uSync/"               - place to put files
    ///             archive="~/uSync.Archive/"      - place to archive files
    ///             versions="true"                 - store versions at every save
    ///             />
    ///     </settings>
    /// </uSync>
    /// 
    /// </summary>
    public class ContextConfigSection : ConfigurationSection
    {
        //[ConfigurationProperty("read", DefaultValue = "true", IsRequired = false)]
        //public Boolean Read
        //{
        //    get
        //    {
        //        return (Boolean)this["read"];
        //    }
        //    set
        //    {
        //        this["read"] = value;
        //    }
        //}

        [ConfigurationProperty("Domains")]
        public ContextConfigDomains Domains
        {
            get { return (ContextConfigDomains)this["Domains"]; }
        }

        [ConfigurationProperty("Environments")]
        public ContextConfigEnvironments Environments
        {
            get { return (ContextConfigEnvironments)this["Environments"]; }
        }

    }

    public class ContextDomain : ConfigurationElement 
    {
        [ConfigurationProperty("url", IsRequired=true)]
        public string Url 
        {
            get { return (string)base["url"]; }
        }

        [ConfigurationProperty("env", IsRequired=false)]
        public string Environment 
        {
            get { return (string)base["env"]; }
        }

        internal string url {
            get { return Url ; }
        }
    }

    public class ContextEnvironment : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
        }

        internal string name
        {
            get { return Name; }
        }
    }

    public class KeyValue : ConfigurationElement 
    {
        [ConfigurationProperty("key", IsRequired=true)]
        public string Key 
        {
            get { return (string)base["key"]; }
        }

        [ConfigurationProperty("value", IsRequired=false)]
        public string Value 
        {
            get { return (string)base["value"]; }
        }

        internal string key {
            get { return Key ; }
        }
    }

    [ConfigurationCollection(typeof(ContextDomain), AddItemName = "Domain", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ContextConfigDomains : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ContextDomain(); 
        }
        
        protected override object GetElementUrl(ConfigurationElement element)
        {
            return ((ContextDomain)element).url;
        }

        public int IndexOf(ContextDomain element)
        {
            return BaseIndexOf(element) ;
        }

        public ContextDomain this[int index]
        {
            get { return (ContextDomain)BaseGet(index); }
        }

        public string[] GetAll()
        {
            return BaseGetAllKeys().Cast<string>().ToArray() ; 
        }

    }

    [ConfigurationCollection(typeof(ContextEnvironment), AddItemName = "Env", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ContextConfigEnvironments : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ContextEnvironment();
        }

        protected override object GetElementName(ConfigurationElement element)
        {
            return ((ContextEnvironment)element).name;
        }

        public int IndexOf(ContextEnvironment element)
        {
            return BaseIndexOf(element);
        }

        public ContextEnvironment this[int index]
        {
            get { return (ContextEnvironment)BaseGet(index); }
        }

        public string[] GetAll()
        {
            return BaseGetAllKeys().Cast<string>().ToArray();
        }

    }

    [ConfigurationCollection(typeof(KeyValue), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ContextConfigEnvKeys : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new KeyValue();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((KeyValue)element).key;
        }

        public int IndexOf(KeyValue element)
        {
            return BaseIndexOf(element);
        }

        public ContextDomain this[int index]
        {
            get { return (ContextDomain)BaseGet(index); }
        }

        public string[] GetAll()
        {
            return BaseGetAllKeys().Cast<string>().ToArray();
        }

    }

    //public class uSyncElements : ConfigurationElement
    //{
    //    [ConfigurationProperty("docTypes", DefaultValue = "true", IsRequired = true)]
    //    public Boolean DocumentTypes
    //    {
    //        get { return (Boolean)this["docTypes"]; }
    //    }

    //    [ConfigurationProperty("mediaTypes", DefaultValue = "true", IsRequired = true)]
    //    public Boolean MediaTypes
    //    {
    //        get { return (Boolean)this["mediaTypes"]; }
    //    }

    //    [ConfigurationProperty("dataTypes", DefaultValue = "true", IsRequired = true)]
    //    public Boolean DataTypes
    //    {
    //        get { return (Boolean)this["dataTypes"]; }
    //    }

    //    [ConfigurationProperty("templates", DefaultValue = "true", IsRequired = true)]
    //    public Boolean Templates
    //    {
    //        get { return (Boolean)this["templates"]; }
    //    }

    //    [ConfigurationProperty("stylesheets", DefaultValue = "true", IsRequired = true)]
    //    public Boolean Stylesheets
    //    {
    //        get { return (Boolean)this["stylesheets"]; }
    //    }

    //    [ConfigurationProperty("macros", DefaultValue = "true", IsRequired = true)]
    //    public Boolean Macros
    //    {
    //        get { return (Boolean)this["macros"]; }
    //    }

    //    [ConfigurationProperty("dictionary", DefaultValue = "false", IsRequired = false)]
    //    public Boolean Dictionary
    //    {
    //        get { return (Boolean)this["dictionary"]; }
    //    }

    //}

    //public class uSyncDocTypeSettings : ConfigurationElement
    //{
    //    [ConfigurationProperty("DeletePropertyValues", DefaultValue = "false", IsRequired = true)]
    //    public Boolean DeletePropertyValues 
    //    {
    //        get { return (Boolean)this["DeletePropertyValues"]; }
    //    }
    //}

/*
        public static List<string> PreservedPreValues
        {
            get
            {
                List<string> datalisttypes = new List<string>();
                datalisttypes.Add("f8d60f68-ec59-4974-b43b-c46eb5677985"); // ApprovedColour
                datalisttypes.Add("b4471851-82b6-4c75-afa4-39fa9c6a75e9"); // Checkbox List
                datalisttypes.Add("a74ea9c9-8e18-4d2a-8cf6-73c6206c5da6"); // dropdown 
                datalisttypes.Add("928639ed-9c73-4028-920c-1e55dbb68783"); // dropdown-multiple
                datalisttypes.Add("a52c7c1c-c330-476e-8605-d63d3b84b6a6"); // radiobox

                return datalisttypes; 
            }
        }

    }
  */   

    public class ContextConfigSettings {

        private static string _configfile = "ContextConfig.config";
        private static ContextConfigSection _settings;
        private static Configuration config;

        static ContextConfigSettings()
        {
            try
            {
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = HttpContext.Current.Server.MapPath(string.Format("~/config/{0}", _configfile));

                if (System.IO.File.Exists(fileMap.ExeConfigFilename))
                {
                    // load the settings file
                    config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                    if (config != null)
                    {
                        _settings = (ContextConfigSection)config.GetSection("ContextConfig");
                    }
                }
            }
            catch (Exception ex)
            {
                //LogHelper.Info<uSyncSettings>("Error loading settings file {0}", () => ex.ToString());
            }
            finally
            {

                if (_settings == null)
                {
                    //LogHelper.Info<uSyncSettings>("WARNING: Working with no config file");
                    //_settings = new uSyncSettingsSection(); // default config - won't be savable mind?
                }
            }
        }

        public static void Save()
        {
            if ( _settings != null ) 
                _settings.CurrentConfiguration.Save(ConfigurationSaveMode.Full);
        }

        //public static bool Read
        //{
        //    get { return _settings.Read ; }
        //    set { _settings.Read = value;  }
        //}

        public static string[] Domains
        {
            get { return _settings.Domains.GetAll(); }
        }

        public static string[] Environments
        {
            get { return _settings.Environments.GetAll(); }
        }
        
    }
  
}
