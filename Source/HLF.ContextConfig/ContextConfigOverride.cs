/* Based on ConfigOverrideTest
 * FROM: https://gist.github.com/myaumyau/4975059/
 * http://stackoverflow.com/questions/158783/is-there-a-way-to-override-configurationmanager-appsettings
 */

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Internal;
using System.Reflection;

namespace HLF.ContextConfig
{
    public static class ContextConfigOverride
    {
        sealed class ConfigProxy:IInternalConfigSystem
        {
            readonly IInternalConfigSystem _Baseconf;

            public ConfigProxy(IInternalConfigSystem baseconf)
            {
                this._Baseconf = baseconf;
            }

            object _Appsettings;

            public object GetSection(string ConfigKey)
            {
                if(ConfigKey == "appSettings" && this._Appsettings != null) return this._Appsettings;
                object o = _Baseconf.GetSection(ConfigKey);
                if(ConfigKey == "appSettings" && o is NameValueCollection)
                {
                    // create a new collection because the underlying collection is read-only
                    var cfg = new NameValueCollection((NameValueCollection)o);

                    // add or replace your settings
                    //example: cfg["test"] = "Hello world";
                    foreach (var KeyVal in ContextConfig.AllEnvironmentConfigs())
                    {
                        cfg[KeyVal.Key] = KeyVal.Value;
                    }

                    o = this._Appsettings = cfg;
                }
                return o;
            }

            public void RefreshConfig(string sectionName)
            {
                if (sectionName == "appSettings") _Appsettings = null;
                _Baseconf.RefreshConfig(sectionName);
            }

            public bool SupportsUserConfig
            {
                get { return _Baseconf.SupportsUserConfig; }
            }
        }

        /// <summary>
        /// A call to this should be placed somewhere before config values are called, for instance, in App Start, etc.
        /// </summary>
        public static void ActivateOverride()
        {
            ClearConfigCache();

            // initialize the ConfigurationManager
            object o = ConfigurationManager.AppSettings;

            // hack your proxy IInternalConfigSystem into the ConfigurationManager
            FieldInfo s_configSystem = typeof(ConfigurationManager).GetField("s_configSystem", BindingFlags.Static | BindingFlags.NonPublic);
            s_configSystem.SetValue(null, new ConfigProxy((IInternalConfigSystem)s_configSystem.GetValue(null)));
        }


        private static void ClearConfigCache()
        {
            //Based on Code from http://www.codeproject.com/Articles/69364/Override-Configuration-Manager

            FieldInfo[] fiStateValues = null;
            Type tInitState = typeof
                (System.Configuration.ConfigurationManager).GetNestedType
                ("InitState", BindingFlags.NonPublic);

            if (null != tInitState)
            {
                fiStateValues = tInitState.GetFields();
            }

            FieldInfo fiInit = typeof
                (System.Configuration.ConfigurationManager).GetField
                ("s_initState", BindingFlags.NonPublic | BindingFlags.Static);
            FieldInfo fiSystem = typeof
                (System.Configuration.ConfigurationManager).GetField
                ("s_configSystem", BindingFlags.NonPublic | BindingFlags.Static);

            if (fiInit != null && fiSystem != null && null != fiStateValues)
            {
                fiInit.SetValue(null, fiStateValues[1].GetValue(null));
                fiSystem.SetValue(null, null);
            }
        }
    }
}
