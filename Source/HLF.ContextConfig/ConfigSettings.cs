﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;

namespace HLF.ContextConfig
{
    public class ConfigSettings : ConfigurationSection
    {
        //Config File Object
        private static string _configfile = "ContextConfig.config";
        private static ConfigSettings _Config;
        private static Configuration config;
        public static ConfigSettings Settings = GetSettings();

        public static ConfigSettings GetSettings()
        {
            //try
            //{
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename =
                        HttpContext.Current.Server.MapPath(string.Format("~/config/{0}", _configfile));

                if (System.IO.File.Exists(fileMap.ExeConfigFilename))
                {
                    // load the settings file
                    config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

                    if (config != null)
                    {
                        _Config = (ConfigSettings)config.GetSection("ContextConfig");
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //   // _Errors.Add(string.Format("Error loading settings file {0}", ex.ToString()));
            //        //LogHelper.Info<uSyncSettings>("Error loading settings file {0}", () => ex.ToString());
            //}
            //finally
            //{

                if (_Config == null)
                {
                    //_Errors.Add(string.Format("WARNING: Working with no config file"));
                    //LogHelper.Info<uSyncSettings>("WARNING: Working with no config file");
                    _Config = new ConfigSettings(); // default config - won't be savable mind?
                }
           // }
            return _Config;
        }


        //*** ConfigurationProperties ***

        //<ContextConfig> 'version' attribute
        [ConfigurationProperty("version")]
        public string Version
        {
            get { return (string)base["version"]; }
        }

        //<ContextConfig> <Domains> collection
        [ConfigurationProperty("Domains")]
        public DomainElementCollection Domains
        {
            get { return (DomainElementCollection)base["Domains"]; }
        }

        //<ContextConfig> <Environments> collection
        [ConfigurationProperty("Environments")]
        public EnvironmentElementCollection Environments
        {
            get { return (EnvironmentElementCollection)base["Environments"]; }
        }

    }

    //*** Element Objects ***

    public class DomainElement : ConfigurationElement
    {
        //<Domain> 'url' attribute
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get { return (string)base["url"]; }
        }

        //<Domain> 'environment' attribute
        [ConfigurationProperty("environment", IsRequired = true)]
        public string Environment
        {
            get { return (string)base["environment"]; }
        }
    }

    public class EnvironmentElement : ConfigurationElement
    {
        //<Environment> 'name' attribute
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
        }

        //<Environment> <Configs> (key/values) collection
        [ConfigurationProperty("Configs")]
        public KeyValueElementCollection Configs
        {
            get { return (KeyValueElementCollection)base["Configs"]; }
        }

    }

    public class KeyValueElement : ConfigurationElement
    {
        //<Environment> <add> 'key' attribute
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return (string)base["key"]; }
        }

        //<Environment> <add> 'value' attribute
        [ConfigurationProperty("value", IsRequired = false)]
        public string Value
        {
            get { return (string)base["value"]; }
        }
    }

    //*** Element Collections ***

    [ConfigurationCollection(typeof(DomainElement), AddItemName = "Domain", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class DomainElementCollection : ConfigurationElementCollection
    {
        protected override string ElementName
        {
            get { return "Domain"; }
        }

        //Basic Stuff
        public ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DomainElement();
        }

        public DomainElement this[int index]
        {
            get { return (DomainElement)base.BaseGet(index); }
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
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as DomainElement).Url;
        }

        public DomainElement this[string url]
        {
            get { return (DomainElement)base.BaseGet(url); }
        }
    }

    [ConfigurationCollection(typeof(EnvironmentElement), AddItemName = "Environment", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class EnvironmentElementCollection : ConfigurationElementCollection
    {
        protected override string ElementName
        {
            get { return "Environment"; }
        }

        //Basic Stuff
        public ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new EnvironmentElement();
        }

        public EnvironmentElement this[int index]
        {
            get { return (EnvironmentElement)base.BaseGet(index); }
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
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as EnvironmentElement).Name;
        }

        public EnvironmentElement this[string name]
        {
            get { return (EnvironmentElement)base.BaseGet(name); }
        }
    }

    [ConfigurationCollection(typeof(KeyValueElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class KeyValueElementCollection : ConfigurationElementCollection
    {
        protected override string ElementName
        {
            get { return "add"; }
        }

        //Basic Stuff
        public ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new KeyValueElement();
        }

        public KeyValueElement this[int index]
        {
            get { return (KeyValueElement)base.BaseGet(index); }
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
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as KeyValueElement).Key;
        }

        public KeyValueElement this[string key]
        {
            get { return (KeyValueElement)base.BaseGet(key); }
        }
    }

}