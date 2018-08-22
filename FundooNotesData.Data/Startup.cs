using FundooNotesData.Data.Infrastructure;
using FundooNotesData.Data.Models;
using FundooNotesData.Data.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using Microsoft.Owin.Security.Facebook;


namespace FundooNotesData.Data
{
    public class Startup
    {

        // Enable the application to use OAuthAuthorization. You can then secure your Web APIs
        static Startup()
        {
            PublicClientId = "web";

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                AuthorizeEndpointPath = new PathString("/Account/Authorize"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };
        }


        public void Configuration(IAppBuilder app)
        {


            //HttpConfiguration httpConfig = new HttpConfiguration();

            ConfigureOAuthTokenGeneration(app);

            //ConfigureWebApi(httpConfig);

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            //app.UseWebApi(httpConfig);

        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);


            // Plugin the OAuth bearer JSON Web Token tokens generation and Consumption will be here
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                       validateInterval: TimeSpan.FromMinutes(20),
                       regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");


            //Configure Facebook External Login
            //app.UseFacebookAuthentication(
            //    appId: "195328631106894",
            //    appSecret: "b64595806378c621c59d92bea70289e4");


            app.UseFacebookAuthentication(new FacebookAuthenticationOptions()
            {
                AppId = "195328631106894",
                AppSecret = "b64595806378c621c59d92bea70289e4",
                Scope = {
            "user_birthday", //Access the date and month of a person's birthday.  
            "public_profile" //Provides access to a subset of items that are part of a person's public profile.  
            //A person's public profile refers to the following properties on the user object by default:  
        },
                Fields = {
            "birthday", //User's DOB  
            "picture", //User Profile Image  
            "name", //User Full Name  
            "email", //User Email  
            "gender", //user's Gender  
            
        },
            });


            //app.UseFacebookAuthentication(facebookAuthOptions);

            ////Configure Google External Login
            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "490149521595-3lfbn4a4ka8ojs192mvj5vc3onncm10h.apps.googleusercontent.com",
            //    ClientSecret = "krwkGxwsx-j8hXtzZG4jtEZ3"
            //});

        }


        //public void ConfigureAuth(IAppBuilder app)
        //{
        //    // Configure the db context, user manager and signin manager to use a single instance per request
        //    app.CreatePerOwinContext(ApplicationDbContext.Create);
        //    app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
        //    app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

        //    // Enable the application to use a cookie to store information for the signed in user
        //    // and to use a cookie to temporarily store information about a user logging in with a third party login provider
        //    // Configure the sign in cookie
        //    app.UseCookieAuthentication(new CookieAuthenticationOptions
        //    {
        //        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
        //        LoginPath = new PathString("/Account/Login"),
        //        Provider = new CookieAuthenticationProvider
        //        {
        //            // Enables the application to validate the security stamp when the user logs in.
        //            // This is a security feature which is used when you change a password or add an external login to your account.  
        //            OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
        //                validateInterval: TimeSpan.FromMinutes(30),
        //                regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
        //        }
        //    });
        //    app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

        //    // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
        //    app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

        //    // Enables the application to remember the second login verification factor such as phone or email.  <add key="owin:AppStartup" value="FundooApp.Data.Startup" />
        //    // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
        //    // This is similar to the RememberMe option when you log in.
        //    app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

        //    // Uncomment the following lines to enable logging in with third party login providers
        //    //app.UseMicrosoftAccountAuthentication(
        //    //    clientId: "",
        //    //    clientSecret: "");

        //    //app.UseTwitterAuthentication(
        //    //   consumerKey: "",
        //    //   consumerSecret: "");

        //    //app.UseFacebookAuthentication(
        //    //   appId: "",
        //    //   appSecret: "");

        //    //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
        //    //{
        //    //    ClientId = "",
        //    //    ClientSecret = ""
        //    //});
        //}
    }
}
