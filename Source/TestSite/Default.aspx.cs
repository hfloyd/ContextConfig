using System;
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
        }

        private void LookupStuff()
        {
            try
            {
                string B = ContextConfig.CurrentDomain;
                Response.Write(string.Format("ContextConfig.CurrentDomain = {0} <br/>", B));
            }
            catch (Exception e)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", e.GetType().ToString(), e.Message));
            }

            try
            {
                string D = ContextConfig.DomainIsConfigured().ToString();
                Response.Write(string.Format("ContextConfig.DomainIsConfigured() = {0} <br/>", D));
            }
            catch (Exception e)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", e.GetType().ToString(), e.Message));
            }

            try
            {

                string F = ContextConfig.DomainIsConfigured("xyz.com").ToString();
                Response.Write(string.Format("ContextConfig.DomainIsConfigured(\"xyz.com\") = {0} <br/>", F));
            }
            catch (Exception e)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", e.GetType().ToString(), e.Message));
            }

            try
            {
                string G = ContextConfig.DomainIsConfigured("xyz.com", false).ToString();
                Response.Write(string.Format("ContextConfig.DomainIsConfigured(\"xyz.com\", false) = {0} <br/>", G));
            }
            catch (Exception e)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", e.GetType().ToString(), e.Message));
            }

            try
            {
                string E = ContextConfig.DomainEnvironmentName();
                Response.Write(string.Format("ContextConfig.DomainEnvironmentName() = {0} <br/>", E));
            }
            catch (Exception e)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", e.GetType().ToString(), e.Message));
            }

            try
            {
                string H = ContextConfig.DomainEnvironmentName("xyz.com");
                Response.Write(string.Format("ContextConfig.DomainEnvironmentName(\"xyz.com\") = {0} <br/>", H));
            }
            catch (Exception e)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", e.GetType().ToString(), e.Message));
            }

            Response.Write("<br/>");

            try
            {
                string I = ContextConfig.EnvironmentIsConfigured().ToString();
                Response.Write(string.Format("ContextConfig.EnvironmentIsConfigured() = {0} <br/>", I));
            }
            catch (Exception e)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", e.GetType().ToString(), e.Message));
            }

            try
            {
                string J = ContextConfig.EnvironmentIsConfigured("testing").ToString();
                Response.Write(string.Format("ContextConfig.EnvironmentIsConfigured(\"testing\") = {0} <br/>", J));
            }
            catch (Exception e)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", e.GetType().ToString(), e.Message));
            }

            try
            {
                string K = ContextConfig.EnvironmentIsConfigured("testing", false).ToString();
                Response.Write(string.Format("ContextConfig.EnvironmentIsConfigured(\"testing\", false) = {0} <br/>", K));
            }
            catch (Exception e)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", e.GetType().ToString(), e.Message));
            }

            Response.Write("<br/>");

            try
            {
                string C = ContextConfig.GetValue("MyAppKey");
                Response.Write(string.Format("ContextConfig.GetValue(\"MyAppKey\") = {0} <br/>", C));
            }
            catch (Exception e)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", e.GetType().ToString(), e.Message));
            }

            try
            {
                string A = ContextConfig.GetValue("MyAppKey", "dev");
                Response.Write(string.Format("ContextConfig.GetValue(\"MyAppKey\", \"dev\") = {0} <br/>", A));
            }
            catch (Exception e)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", e.GetType().ToString(), e.Message));
            }

            try
            {
                string L = ContextConfig.GetValue("LiveOnlyAppKey", "live");
                Response.Write(string.Format("ContextConfig.GetValue(\"LiveOnlyAppKey\", \"live\") = {0} <br/>", L));
            }
            catch (Exception e)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", e.GetType().ToString(), e.Message));
            }

            try
            {
                string M = ContextConfig.GetValue("LiveOnlyAppKey");
                Response.Write(string.Format("ContextConfig.GetValue(\"LiveOnlyAppKey\") = {0} <br/>", M));
            }
            catch (Exception e)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", e.GetType().ToString(), e.Message));
            }

            try
            {
                string N = ContextConfig.GetValue("KeyThatDoesntExist");
                Response.Write(string.Format("ContextConfig.GetValue(\"KeyThatDoesntExist\") = {0} <br/>", N));
            }
            catch (Exception e)
            {
                Response.Write(string.Format("<span style=\"color:#FF0000;\">ERROR: <b>{0}</b> : {1} </span><br/>", e.GetType().ToString(), e.Message));
            }

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
                Response.Write(string.Format("Domain {0} : {1} = {2} <br/>", d, Domain.Url, Domain.Environment));
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
            }
            Response.Write("<br/>");

        }
    }
}