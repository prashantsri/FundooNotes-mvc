using Microsoft.Owin;
using Owin;
using FundooNotesData.Data;
using Microsoft.Owin.Security.OAuth;

[assembly: OwinStartup(typeof(FundooNotesData.Data.Startup))]

namespace FundooNotes
{
    public partial class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
        //public void Configuration(IAppBuilder app)
        //{
        //    ConfigureAuth(app);
        //}
    }
}
