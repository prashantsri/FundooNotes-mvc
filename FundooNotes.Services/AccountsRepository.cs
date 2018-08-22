using FundooNotesData.Data.Infrastructure;
using FundooNotesData.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;



namespace FundooNotes.Services
{
    public class AccountsRepository
    {
        //private ApplicationUserManager _userManager;
        //private ApplicationSignInManager _signInManager;

        //public ApplicationSignInManager SignInManager
        //{
        //    get
        //    {
        //        return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
        //    }
        //    private set
        //    {
        //        _signInManager = value;
        //    }
        //}
        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}

        //public async Task<IdentityResult> Register(RegisterViewModel model)
        //{
        //    var user = new ApplicationUser
        //    {
                
        //        UserName = model.Username,
        //        Email = model.Username,
        //        FirstName = model.Firstname,
        //        LastName = model.Lastname,
        //        Gender = model.Gender,
        //        Dateofbirth =  model.Dateofbirth,
        //        Adhar = model.Adhar,
        //        PhoneNumber = model.selectedCountry+model.PhoneNumber,
        //        Address = model.Address,
        //        Line2 = model.line2,
        //        City = model.city,
        //        Postalcode = model.postalcode,
        //        State = model.State,
        //        Countrycode = model.selectedCountry,
        //        Password = model.password1
        //    };
        //    try
        //    {
        //        var result = await UserManager.CreateAsync(user, model.password1);
                
        //        if (result.Succeeded)
        //        {
        //            try
        //            {
        //                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        //                var callbackUrl = urlHelper.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: "http");

        //                await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
        //                return IdentityResult.Success;
        //            }
        //            catch (System.Exception ex)
        //            {

        //                ex.ToString();
        //            }
        //            //For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
        //            //Send an email with this link
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return IdentityResult.Failed("Email already exists");

        //}

        //public async Task<string> Verifyotp(VerifyPhoneNumberBindingModel model)
        //{
        //    var userid = await UserManager.FindByEmailAsync(model.Username);

        //    if (userid == null)
        //    {
        //        return "The email is not a registered user !";
        //    }

        //    // phoneNumber = await UserManager.GetPhoneNumberAsync(userid.Id.ToString());
        //    var phoneNumber = userid.PhoneNumber;
        //    if (phoneNumber == null)
        //    {
        //        return "The phone number is not valid !";
        //    }
         
        //    var result = await UserManager.ChangePhoneNumberAsync(userid.Id.ToString(), phoneNumber, model.Code);

        //    if (result.Succeeded)
        //    {
        //        return "Success";
        //    }
        //    return "Error";
        //}

        //public async Task<string> SendPhoneNumber(VerifyPhoneNumberBindingModel model)
        //{
        //    var userid = await UserManager.FindByEmailAsync(model.Username);

        //    if (userid == null)
        //    {
        //        return "The email is not a registered user !";
        //    }

        //    try
        //    {
        //        //var phoneNumber = await UserManager.GetPhoneNumberAsync(userid.Id.ToString());

        //       var  phoneNumber = (userid.PhoneNumber.ToString());

        //        if (phoneNumber == null)
        //        {
        //            return "The phone number is not valid !";
        //        }

        //        var code = await UserManager.GenerateChangePhoneNumberTokenAsync(userid.Id.ToString(), phoneNumber.ToString());
          
        //        await UserManager.SendSmsAsync(userid.Id.ToString(), "Your FundooApp mobile number verification, the current code is " + code);
        //        model.Code = code;
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //    return "Success";
        //}

        //public async Task<SignInStatus> Signin(LoginViewModel model, string returnUrl)
        //{

        //    SignInStatus result;
        //    try
        //    {
        //        result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: false);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //}

        //public async Task<IdentityResult> ConfirmEmail(string userId, string code)
        //{
        //    IdentityResult result = null;
        //    try
        //    {
        //        result = await UserManager.ConfirmEmailAsync(userId, code);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {

        //        ex.ToString();
        //    }
        //    return result;
        //}

        //public async Task<string> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    var user = await UserManager.FindByNameAsync(model.Username);
        //    if (user == null )
        //    {
        //        //|| !(await UserManager.IsEmailConfirmedAsync(user.Id))
        //        // Don't reveal that the user does not exist or is not confirmed
        //        return "User does not exists";
        //    }
        //    string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
        //    //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //    var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        //    var callbackUrl = urlHelper.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: "http");
        //    await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
        //    return "Check Your Email";
        //}

        //public async Task<string> ResetPassword(ResetPasswordViewModel model)
        //{
        //    var user = await UserManager.FindByNameAsync(model.Username);
        //    if (user == null)
        //    {
        //        // Don't reveal that the user does not exist
        //        return "user does not exist";
        //    }
        //    var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        //    if (result.Succeeded)
        //    {
        //        return "Password Changed";
        //    }
        //    return "Error";
        //}

    }
}
