/* Based on ConfigOverrideTest
 * FROM: https://gist.github.com/myaumyau/4975059/
 * http://stackoverflow.com/questions/158783/is-there-a-way-to-override-configurationmanager-appsettings
 */

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Internal;
using System.Reflection;
using System.Web;

namespace HLF.ContextConfig
{
    /// <summary>
    /// Handles overriding standard 'ConfigurationManager.AppSettings["key"]' operations
    /// </summary>
    public static class ContextConfigOverride
    {
        sealed internal class ConfigProxy:IInternalConfigSystem
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
        /// Re-initializes the ConfigurationManager to utilize ContextConfig values when calling 'ConfigurationManager.AppSettings["key"]'
        /// A call to this should be placed somewhere before config values are called, for instance, in App Start.
        /// Or use the 'ContextConfigOverrideModule' HttpModule for default handling
        /// **Will activate the override based on the value in ContextConfig.config**
        /// </summary>
        public static void ActivateOverride()
        {
            //Get value from config
            bool DoOverride = ConfigSettings.Settings.OverrideConfigurationManager;

            //call using config value
            ActivateOverride(DoOverride);
        }

        /// <summary>
        /// Re-initializes the ConfigurationManager to utilize ContextConfig values when calling 'ConfigurationManager.AppSettings["key"]'
        /// A call to this should be placed somewhere before config values are called, for instance, in App Start.
        /// Or use the 'ContextConfigOverrideModule' HttpModule for default handling
        /// </summary>
        /// <param name="DoOverride">Should the override be activated? (False=no code will run)</param>
        public static void ActivateOverride(bool DoOverride)
        {
            ClearConfigCache();

            if (DoOverride)
            {
                // initialize the ConfigurationManager
                object o = ConfigurationManager.AppSettings;

                // hack your proxy IInternalConfigSystem into the ConfigurationManager
                FieldInfo s_configSystem = typeof (ConfigurationManager).GetField("s_configSystem",
                                                                                  BindingFlags.Static |
                                                                                  BindingFlags.NonPublic);
                s_configSystem.SetValue(null, new ConfigProxy((IInternalConfigSystem) s_configSystem.GetValue(null)));
            }
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

    /// <summary>
    /// HttpModule to run 'ContextConfigOverride.ActivateOverride()'
    /// Register in Web.config file:
    /// &lt;add name="ContextConfigOverrideModule" type="HLF.ContextConfig.ContextConfigOverrideModule, HLF.ContextConfig" /&gt;
    /// IIS6 or Classic Mode - place in  &lt;system.web&gt;&lt;httpModules&gt; section
    /// IIS7+ or Integrated Mode - place in &lt;system.webServer&gt;&lt;modules&gt; section
    /// </summary>
    public class ContextConfigOverrideModule : IHttpModule
    {
        /// <summary>
        /// Runs 'ActivateOverride()' on App Start
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            ContextConfigOverride.ActivateOverride();
        }

        public void Dispose()
        {

        }

    }
}
