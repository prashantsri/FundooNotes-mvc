using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace FundooNotesData.Data.Domain
{
    class Config
    {
        public static string AccountSid => WebConfigurationManager.AppSettings["AccountSid"] ??
                                      "ACe9916dfa3eb4ad2d0e866738de2b43d0";

        public static string AuthToken => WebConfigurationManager.AppSettings["AuthToken"] ??
                                          "c73b45659cd8510cdc2a6358e34bbc48";

        public static string TwilioNumber => WebConfigurationManager.AppSettings["TwilioNumber"] ??
                                             "+16105954938";
    }
}
