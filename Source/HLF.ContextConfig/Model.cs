using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLF.ContextConfig
{
   internal class ConfigData
    {
       public List<ContextDomain> Domains { get; set; }

          internal ConfigData()
          {
              //Create new config data object by reading config file via XML
          }

          ContextDomain GetDomain(string DomainUrl)
          {//Lookup domain
              return new ContextDomain();
          }

    }


   internal class ContextDomain
   {
       private string _Url;
       public string Url
       {
           get { return _Url; }
           set { _Url = value; }
       }

       private string _EnvName;
       public string EnvName
       {
           get { return _EnvName; }
           set { _EnvName = value; }
       }

       private ContextEnvironment _Environment;
       public ContextEnvironment Environment
       {
           get { return _Environment; }
           set { _Environment = value; }
       }
       
   }

   internal class ContextEnvironment
   {
       private string _Name;

       public string Name
       {
           get { return _Name; }
           set { _Name = value; }
       }
       
   }
}
