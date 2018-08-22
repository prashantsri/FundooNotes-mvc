using FundooNotesData.Data.Infrastructure;
using FundooNotesData.Data.Log;
using FundooNotesData.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FundooNotes.Services
{
    
    public class AccountService
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public async Task<IdentityResult> Register(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {

                UserName = model.Username,
                Email = model.Username,
                FirstName = model.Firstname,
                LastName = model.Lastname,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Adhar = model.Adhar,
                PhoneNumber = model.selectedCountry + model.PhoneNumber,
                Address = model.Address,
                Line2 = model.line2,
                City = model.city,
                Postalcode = model.postalcode,
                State = model.State,
                Countrycode = model.selectedCountry,
                Password = model.password1
            };
            try
            {
                var result = await UserManager.CreateAsync(user, model.password1);

                if (result.Succeeded)
                {
                    try
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                        var callbackUrl = urlHelper.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: "http");

                        await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                        return IdentityResult.Success;
                    }
                    catch (System.Exception ex)
                    {

                        ex.ToString();
                    }
                    //For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    //Send an email with this link
                }
            }
            catch (Exception ex)
            {

            }
            return IdentityResult.Failed("Email already exists");

        }

       

        public async Task<string> SendPhoneNumber(VerifyPhoneNumberBindingModel model)
        {

            //var userid = await UserManager.FindByEmailAsync(model.Username);
            var userid =  UserManager.FindByEmail(model.Username);

            if (userid == null)
            {
                return "The email is not a registered user !";
            }

            try
            {
                //taking phone number
                string strNewPhoneNumber = userid.PhoneNumber;
                var taskPhoneNumberSet = UserManager.SetPhoneNumberAsync(userid.Id, strNewPhoneNumber);
                taskPhoneNumberSet.Wait();
                var phoneNumber = UserManager.GetPhoneNumberAsync(userid.Id);

                if (phoneNumber == null)
                {
                    return "The phone number is not valid !";
                }

                var code = await UserManager.GenerateChangePhoneNumberTokenAsync(userid.Id.ToString(), phoneNumber.Result.ToString());

                await UserManager.SendSmsAsync(userid.Id.ToString(), "Your FundooApp mobile number verification, the current code is " + code);


                model.Code = code;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return "Success";
        }
        public async Task<string> Verifyotp(VerifyPhoneNumberBindingModel model)
        {
            var userid = await UserManager.FindByEmailAsync(model.Username);

            // check if email is valid or not.
            if (userid == null)
            {
                return "The email is not a registered user !";
            }

            var phoneNumber = await UserManager.GetPhoneNumberAsync(userid.Id.ToString());

            // check if phone number valid 
            if (phoneNumber == null)
            {
                return "The phone number is not valid !";
            }

            var result = await UserManager.ChangePhoneNumberAsync(userid.Id.ToString(), phoneNumber, model.Code);

            if (result.Succeeded)
            {
                return "Success";
            }
            return "Error";

        }

        public async Task<SignInStatus> ExternalLoginCallback(ExternalLoginInfo loginInfo, bool isPersistent)
        {
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            var info = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return result;
            }
            var userdata = await UserManager.FindByEmailAsync(info.Email);
            if (userdata != null)
            {
                var ID = userdata.Id;



            }
            return result;

        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalBearer);
            AuthenticationManager.SignOut(CookieAuthenticationDefaults.AuthenticationType);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }
        //public async Task<IdentityResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
                public async Task<IdentityResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            IdentityResult result = null;
            try
            {
                model.Password = "Prashant@124";
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return result;
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email,PasswordHash=model.Password, FirstName = model.Firstname, Password=model.Password, Profile = model.ProfileUrl };

                try
                {
                    result = await UserManager.CreateAsync(user);
                }
                catch (Exception ex)
                {

                    ex.Message.ToString();
                }
               
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return result;
                    }
                }
                return result;

            }
            catch (Exception ex)
            {
                Logger.Write(ex.ToString());

            }
            return result;
        }

        public async Task<SignInStatus> Signin(LoginViewModel model, string returnUrl)
        {

            SignInStatus result;
            try
            {
                result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: false);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<IdentityResult> ConfirmEmail(string userId, string code)
        {
            IdentityResult result = null;
            try
            {
                result = await UserManager.ConfirmEmailAsync(userId, code);
                return result;
            }
            catch (Exception ex)
            {

                ex.ToString();
            }
            return result;
        }

        public async Task<string> ForgotPassword(ForgotPasswordViewModel model)
        {
            var user = await UserManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                //|| !(await UserManager.IsEmailConfirmedAsync(user.Id))
                // Don't reveal that the user does not exist or is not confirmed
                return "User does not exists";
            }
            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var callbackUrl = urlHelper.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: "http");
            await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
            return "Check Your Email";
        }

        public async Task<string> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await UserManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return "user does not exist";
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return "Password Changed";
            }
            return "Error";
        }

        public async Task<string> RegisterExternal(ExternalLoginBindingModel external)
        {
            var infos = await AuthenticationManager.GetExternalLoginInfoAsync();
            string info = Convert.ToString(AuthenticationManager.User.Identity.Name);
            if (info == null)
            {
                return "Error";
            }
            var userdata = await UserManager.FindByEmailAsync(info);

            if (userdata != null)
            {
                var ID = userdata.Id;
                //TempData["UserID"] = ID;
                return ID;
            }
            else
            {

            }
            return "Error";
        }


        public async Task<ApplicationUser> GetUserdata(string email)
        {

            ApplicationUser UserData = await UserManager.FindByEmailAsync(email);

            return UserData;
        }
        public async Task<ApplicationUser> GetUser(string email, int findBy)
        {
            ApplicationUser data = new ApplicationUser();
            // Find user by emial.
            if (findBy == 1)
            {
                data = await UserManager.FindByEmailAsync(email);

                return data;
            }
            else
            {
                data = await UserManager.FindByIdAsync(email);

                return data;
            }


        }
        public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            IdentityUser user = await _userManager.FindAsync(loginInfo);

            return user;
        }

        //public async Task<IdentityResult> CreateAsync(IdentityUser user)
        //{
        //    var result = await _userManager.CreateAsync(iden` user);

        //    return result;
        //}

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);

            return result;
        }
    }
}
