using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using FundooNotesData.Data.Models;
using FundooNotes.Services;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Http;
using System.Net.Http;
using System.Web;
using System.Net.Http.Headers;
using System.Collections.Generic;
using FundooNotesData.Data.Log;
using System.Text;
using System.Net;

namespace FundooNotes.Controllers
{


    public class AccountApicontroller : ApiController
    {

       // AccountsRepository accountRepository = new AccountsRepository();
        AccountService objAccountService = new AccountService();

        private SignInStatus result;
        [HttpPost]
        public async Task<IdentityResult> Register(RegisterViewModel model)
        {
            IdentityResult result = null;
            if (ModelState.IsValid)
            {
                result = await objAccountService.Register(model);
                return result;
            }
            return result;
        }

        [HttpPost]
        public async Task<SignInStatus> Signin(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                AccountController accountController = new AccountController();
                await accountController.Token(model);
                var result = await objAccountService.Signin(model, returnUrl);
                return result;


            }
            return result;
        }

        //GET: api/WebApi/ConfirmEmail
        public async Task<IdentityResult> ConfirmEmail(string userId, string code)
        {
            var result = await objAccountService.ConfirmEmail(userId, code);
            return result;
        }

       public async Task ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                await objAccountService.ForgotPassword(model);
            }
        }

        public async Task<string> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                await objAccountService.ResetPassword(model);
            }
            return "";
        }

        //GET: api/WebApi/VerifyPhoneNumber
        public async Task SendPhoneNumber(VerifyPhoneNumberBindingModel model)
        {
            if (ModelState.IsValid)
            {
                await objAccountService.SendPhoneNumber(model);

            }
        }
        //POST: api/WebApi/VerifyPhoneNumber
        public async Task Verifyotp(VerifyPhoneNumberBindingModel model)
        {
            if (ModelState.IsValid)
            {
                await objAccountService.Verifyotp(model);
            }
        }


        public async Task<string> GenerateTokenAsync(LoginViewModel context)
        {
            var jsonString = "";
            try
            {
                var handler = new WebRequestHandler() { ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true };

                using (var client = new HttpClient(handler))
                {
                    //client.BaseAddress = new Uri(Request.RequestUri.GetLeftPart(UriPartial.Authority).ToString());
                    client.BaseAddress = new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority));
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // HTTP POST       
                    //if (context.Password == null)
                    //{
                    //    context.Password = "Prashant@124";
                    //}
                    var body = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", context.Username),
                   

                        new KeyValuePair<string, string>("password", context.Password)
                };
                    //prashant
                    
                    //


                    var content = new FormUrlEncodedContent(body);
                   // var response1 = await client.PostAsJsonAsync("", body);
                    HttpResponseMessage response = await client.PostAsync("token", content);

                    if (response.IsSuccessStatusCode)
                    {
                        jsonString = await response.Content.ReadAsStringAsync();
                        jsonString = jsonString.Split(',')[0].ToString().Split(':')[1].ToString();

                        return jsonString.Replace("\"", "");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex.ToString()); ;
            }
            return jsonString;
        }

        public async Task<SignInStatus> ExternalLoginCallback(ExternalLoginInfo loginInfo, bool isPersistent)
        {
            var result = await objAccountService.ExternalLoginCallback(loginInfo, isPersistent);
            return result;
        }


        //public async Task<IdentityResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
            public async Task<IdentityResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            IdentityResult result = null;
            if (ModelState.IsValid)
            {
                result = await objAccountService.ExternalLoginConfirmation(model, returnUrl);
                //result = await objAccountService.Register(model);
                return result;
            }
            return result;
        }

        public void Logout()
        {
            objAccountService.Logout();
        }
    }
}