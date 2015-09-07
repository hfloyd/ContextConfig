using System;
using System.Configuration;
using System.Configuration.Internal;
using System.Reflection;
using HLF.ContextConfig;

namespace TestSite
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("Hello!<br/><hr/>");

            Response.Write("<h3>All Configuration Data</h3>");
            Response.Write("<p><b>(Using the 'ConfigSettings' class)</b></p>");
            WriteData();
            Response.Write("<hr/>");

            Response.Write("<h3>Testing Functions</h3>");
            Response.Write("<p><b>(Using the 'ContextConfig' class)</b></p>");
            LookupStuff();
            Response.Write("<hr/>");

            Response.Write("<h3>Testing Web.config Override</h3>");
            Response.Write("<p><b>(Using the 'ContextConfigOverride' class)</b></p>");
            TestConfigOverride();
            Response.Write("<hr/>");

        }

        private void WriteData()
        {
            string Version = ConfigSettings.Settings.Version;
            Response.Write(string.Format("Version: {0} <br/>", Version));
            Response.Write("<br/>");

            int DomainsCount = ConfigSettings.Settings.Domains.Count;
            Response.Write(string.Format("Total Domains: {0} <br/>", DomainsCount));

            int d = 0;
            foreach (DomainElement Domain in ConfigSettings.Settings.Domains)
            {
                Response.Write(string.Format("Domain {0} : {1} = {2} ({3}) <br/>", d, Domain.Url, Domain.Environment, Domain.SiteName));
                d++;
            }
            Response.Write("<br/>");

            int EnvironmentsCount = ConfigSettings.Settings.Environments.Count;
            Response.Write(string.Format("Total Environments: {0} <br/>", EnvironmentsCount));

            int e = 0;
            foreach (EnvironmentElement Env in ConfigSettings.Settings.Environments)
            {
                Response.Write(string.Format("Environment {0} : {1} <br/>", e, Env.Name));
                e++;

                int ConfigsCount = Env.Configs.Count;
                Response.Write(string.Format("ConfigsCount: {0} <ul>", ConfigsCount));

                int c = 0;
                foreach (KeyValueElement KeyValue in Env.Configs)
                {
                    Response.Write(string.Format("<li> {0} = {1} </li>", KeyValue.Key, KeyValue.Value));
                    c++;
                }
                Response.Write("</ul>");

                Response.Write("SMTP Settings<ul>");
                var smtpSettings = Env.SmtpSettings;
                Response.Write(string.Format("<li> From = {0} </li>", smtpSettings.From));
                Response.Write(string.Format("<li> Delivery Method = {0} </li>", smtpSettings.DeliveryMethod));
                Response.Write(string.Format("<li> Delivery Format = {0} </li>", smtpSettings.DeliveryFormat));
                Response.Write(string.Format("<li> Use Default Credentials = {0} </li>", smtpSettings.Network.UseDefaultCredentials));
                Response.Write(string.Format("<li> Enable SSL = {0} </li>", smtpSettings.Network.EnableSsl));
                Response.Write(string.Format("<li> Host = {0} </li>", smtpSettings.Network.Host));
                Response.Write(string.Format("<li> Port = {0} </li>", smtpSettings.Network.Port));
                Response.Write(string.Format("<li> Client Domain = {0} </li>", smtpSettings.Network.ClientDomain));
                Response.Write(string.Format("<li> User Name = {0} </li>", smtpSettings.Network.UserName));
                Response.Write(string.Format("<li> Password = {0} </li>", smtpSettings.Network.Password));
                Response.Write(string.Format("<li> Target Name = {0} </li>", smtpSettings.Network.TargetName));
                Response.Write(string.Format("<li> Pickup Directory Location = {0} </li>", smtpSettings.SpecifiedPickupDirectory.PickupDirectoryLocation));
                Response.Write("</ul>");

            }
            Response.Write("<br/>");
        }

        private void LookupStuff()
        {
            try
            {
                string B = ContextConfig.CurrentDomain;
                Response.Write(string.Format("ContextConfig.CurrentDomain = {0} <br/>", B));
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }

            try
            {
                string D = ContextConfig.DomainIsConfigured().ToString();
                Response.Write(string.Format("ContextConfig.DomainIsConfigured() = {0} <br/>", D));
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }

            try
            {

                string F = ContextConfig.DomainIsConfigured("xyz.com").ToString();
                Response.Write(string.Format("ContextConfig.DomainIsConfigured(\"xyz.com\") = {0} <br/>", F));
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }

            try
            {
                string G = ContextConfig.DomainIsConfigured("xyz.com", false).ToString();
                Response.Write(string.Format("ContextConfig.DomainIsConfigured(\"xyz.com\", false) = {0} <br/>", G));
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }

            try
            {
                string E = ContextConfig.DomainEnvironmentName();
                Response.Write(string.Format("ContextConfig.DomainEnvironmentName() = {0} <br/>", E));
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }

            try
            {
                string H = ContextConfig.DomainEnvironmentName("xyz.com");
                Response.Write(string.Format("ContextConfig.DomainEnvironmentName(\"xyz.com\") = {0} <br/>", H));
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }

            Response.Write("<br/>");

            try
            {
                string E = ContextConfig.DomainSiteName();
                Response.Write(string.Format("ContextConfig.DomainSiteName() = {0} <br/>", E));
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }

            try
            {
                string H = ContextConfig.DomainSiteName("xyz.com");
                Response.Write(string.Format("ContextConfig.DomainSiteName(\"xyz.com\") = {0} <br/>", H));
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }

            Response.Write("<br/>");

            try
            {
                string I = ContextConfig.EnvironmentIsConfigured().ToString();
                Response.Write(string.Format("ContextConfig.EnvironmentIsConfigured() = {0} <br/>", I));
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }

            try
            {
                string I = ContextConfig.DomainEnvironmentName().ToString();
                Response.Write(string.Format("ContextConfig.DomainEnvironmentName() = {0} <br/>", I));
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }

            try
            {
                string J = ContextConfig.EnvironmentIsConfigured("testing").ToString();
                Response.Write(string.Format("ContextConfig.EnvironmentIsConfigured(\"testing\") = {0} <br/>", J));
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }

            try
            {
                string K = ContextConfig.EnvironmentIsConfigured("testing", false).ToString();
                Response.Write(string.Format("ContextConfig.EnvironmentIsConfigured(\"testing\", false) = {0} <br/>", K));
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }

            Response.Write("<br/>");

            try
            {
                string C = ContextConfig.GetValue("MyAppKey");
                Response.Write(string.Format("ContextConfig.GetValue(\"MyAppKey\") = {0} <br/>", C));
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }

            try
            {
                string A = ContextConfig.GetValue("MyAppKey", "dev");
                Response.Write(string.Format("ContextConfig.GetValue(\"MyAppKey\", \"dev\") = {0} <br/>", A));
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }

            try
            {
                string L = ContextConfig.GetValue("LiveOnlyAppKey", "live");
                Response.Write(string.Format("ContextConfig.GetValue(\"LiveOnlyAppKey\", \"live\") = {0} <br/>", L));
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }

            try
            {
                string M = ContextConfig.GetValue("LiveOnlyAppKey");
                Response.Write(string.Format("ContextConfig.GetValue(\"LiveOnlyAppKey\") = {0} <br/>", M));
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }

            try
            {
                string N = ContextConfig.GetValue("KeyThatDoesntExist");
                Response.Write("Here we throw an error intentionally:");
                Response.Write(string.Format("ContextConfig.GetValue(\"KeyThatDoesntExist\") = {0} <br/>", N));
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }

            Response.Write("<br/>");

            try
            {
                SmtpSettingsElement smtpSettings = ContextConfig.GetSmtpSettings();

                Response.Write("SMTP Settings with default<ul>");
                Response.Write(string.Format("<li> From = {0} </li>", smtpSettings.From));
                Response.Write(string.Format("<li> Delivery Method = {0} </li>", smtpSettings.DeliveryMethod));
                Response.Write(string.Format("<li> Delivery Format = {0} </li>", smtpSettings.DeliveryFormat));
                Response.Write(string.Format("<li> Use Default Credentials = {0} </li>", smtpSettings.Network.UseDefaultCredentials));
                Response.Write(string.Format("<li> Enable SSL = {0} </li>", smtpSettings.Network.EnableSsl));
                Response.Write(string.Format("<li> Host = {0} </li>", smtpSettings.Network.Host));
                Response.Write(string.Format("<li> Port = {0} </li>", smtpSettings.Network.Port));
                Response.Write(string.Format("<li> Client Domain = {0} </li>", smtpSettings.Network.ClientDomain));
                Response.Write(string.Format("<li> User Name = {0} </li>", smtpSettings.Network.UserName));
                Response.Write(string.Format("<li> Password = {0} </li>", smtpSettings.Network.Password));
                Response.Write(string.Format("<li> Target Name = {0} </li>", smtpSettings.Network.TargetName));
                Response.Write(string.Format("<li> Pickup Directory Location = {0} </li>", smtpSettings.SpecifiedPickupDirectory.PickupDirectoryLocation));
                Response.Write("</ul>");

                smtpSettings = ContextConfig.GetSmtpSettings(false);

                Response.Write("SMTP Settings without default<ul>");
                Response.Write(string.Format("<li> From = {0} </li>", smtpSettings.From));
                Response.Write(string.Format("<li> Delivery Method = {0} </li>", smtpSettings.DeliveryMethod));
                Response.Write(string.Format("<li> Delivery Format = {0} </li>", smtpSettings.DeliveryFormat));
                Response.Write(string.Format("<li> Use Default Credentials = {0} </li>", smtpSettings.Network.UseDefaultCredentials));
                Response.Write(string.Format("<li> Enable SSL = {0} </li>", smtpSettings.Network.EnableSsl));
                Response.Write(string.Format("<li> Host = {0} </li>", smtpSettings.Network.Host));
                Response.Write(string.Format("<li> Port = {0} </li>", smtpSettings.Network.Port));
                Response.Write(string.Format("<li> Client Domain = {0} </li>", smtpSettings.Network.ClientDomain));
                Response.Write(string.Format("<li> User Name = {0} </li>", smtpSettings.Network.UserName));
                Response.Write(string.Format("<li> Password = {0} </li>", smtpSettings.Network.Password));
                Response.Write(string.Format("<li> Target Name = {0} </li>", smtpSettings.Network.TargetName));
                Response.Write(string.Format("<li> Pickup Directory Location = {0} </li>", smtpSettings.SpecifiedPickupDirectory.PickupDirectoryLocation));
                Response.Write("</ul>");
            }
            catch (Exception Ex)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", Ex.GetType().ToString(), Ex.Message));
            }
        }

        private void TestConfigOverride()
        {
            Response.Write(string.Format("'OverrideConfigurationManager' value in config = {0} <br/>", ConfigSettings.Settings.OverrideConfigurationManager));

            // test it
            string Test = ConfigurationManager.AppSettings["OverrideTest"];
            Response.Write(string.Format("OverrideTest = {0} <br/>", Test));

            // test missing value
            string TestNull = ConfigurationManager.AppSettings["NoKey"];
            Response.Write(string.Format("Missing Key = {0} <br/>", TestNull));

        }
    }
}