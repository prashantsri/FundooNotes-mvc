using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using FundooNotesData.Data.Models;
using FundooNotesData.Data.Infrastructure;
using FundooNotesData.Data.Log;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using FundooNotes.Services;
using System.Net;
using FundooNotes.Result;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Security;
using Microsoft.Owin.Security.Cookies;

namespace FundooNotes.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        AccountService accountService = new AccountService();
        AccountApicontroller AccountApicontroller = new AccountApicontroller();
        public static string token = "";
        public static string currenturl = "";
        public string url = string.Empty;
        public string email = string.Empty;

        [HttpGet]
        public ActionResult Authorize()
        {
            var claims = new ClaimsPrincipal(User).Claims.ToArray();
            var identity = new ClaimsIdentity(claims, "Bearer");
            AuthenticationManager.SignIn(identity);
            var url = Request.Url.AbsoluteUri;
            return new EmptyResult();

        }

        public AccountController()
        {
        }

        [AllowAnonymous]
        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                result = await AccountApicontroller.Register(model);
                if (result.Succeeded)
                {
                    TempData["RegisterSuccess"] = "Registered Successfully";

                    return View("Signin");
                }
                else
                {
                    TempData["EmailExists"] = "Email already Exists";
                    return View();
                }

            }

            TempData["Fields"] = "Enter all Fields Correctly";
            return View(model);
        }

        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]

        public ActionResult Signin(string returnUrl)


        {
            ViewBag.ReturnUrl = returnUrl;
            currenturl = Request.Url.Scheme + "://" + Request.Url.Authority;
            return View();
        }

        /// <summary>
        /// Post consume api.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> ConsumePostApi(tblNotes model, string token)
        {
            try
            {
                var client = new HttpClient();
                string returnurl = currenturl + "/api/NotesApi/AddNotes";

                client.BaseAddress = new Uri(returnurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var body = new List<KeyValuePair<string, string>>
                {
                        new KeyValuePair<string, string>("ID", Convert.ToString(model.ID)),
                        new KeyValuePair<string, string>("Content", model.Content),
                        new KeyValuePair<string, string>("UserID", Convert.ToString(model.UserID)),
                        new KeyValuePair<string, string>("Title", model.Title),
                        new KeyValuePair<string, string>("Mode",Convert.ToString(model.Mode)),
                        new KeyValuePair<string,string>("IsPin",Convert.ToString(model.IsPin)),
                        new KeyValuePair<string,string>("ColorCode",(model.ColorCode)),
                        new KeyValuePair<string,string>("Reminder",(model.Reminder)),
                        new KeyValuePair<string,string>("IsArchive",Convert.ToString(model.IsArchive)),
                        new KeyValuePair<string,string>("IsActive",Convert.ToString(model.IsActive)),
                        new KeyValuePair<string,string>("IsDelete",Convert.ToString(model.IsDelete)),
                        new KeyValuePair<string,string>("IsTrash",Convert.ToString(model.IsTrash)),
                        new KeyValuePair<string,string>("ImageUrl",model.ImageUrl),
                        new KeyValuePair<string, string>("Label",model.Label),
                        new KeyValuePair<string, string>("owner",model.owner),
                        new KeyValuePair<string, string>("Share",model.share)

                };
                var content = new FormUrlEncodedContent(body);
                HttpResponseMessage response = await client.PostAsync(returnurl, content);
                return 1;
            }
            catch (Exception ex)
            {

                Logger.Write(ex.ToString()); ;
            }
            return 0;

        }


        public async Task<int> ConsumePostLabelApi(tblNotes model, string token)
        {
            try
            {
                var client = new HttpClient();
                string returnurl = currenturl + "/api/NotesApi/addnotelabel";

                client.BaseAddress = new Uri(returnurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var body = new List<KeyValuePair<string, string>>
                {
                        new KeyValuePair<string, string>("ID", Convert.ToString(model.ID)),
                        new KeyValuePair<string, string>("Content", model.Content),
                        new KeyValuePair<string, string>("UserID", Convert.ToString(model.UserID)),
                        new KeyValuePair<string, string>("Title", model.Title),
                        new KeyValuePair<string, string>("Mode",Convert.ToString(model.Mode)),
                        new KeyValuePair<string,string>("IsPin",Convert.ToString(model.IsPin)),
                        new KeyValuePair<string,string>("ColorCode",(model.ColorCode)),
                        new KeyValuePair<string,string>("Reminder",(model.Reminder)),
                        new KeyValuePair<string,string>("IsArchive",Convert.ToString(model.IsArchive)),
                        new KeyValuePair<string,string>("IsActive",Convert.ToString(model.IsActive)),
                        new KeyValuePair<string,string>("IsDelete",Convert.ToString(model.IsDelete)),
                        new KeyValuePair<string,string>("IsTrash",Convert.ToString(model.IsTrash)),
                        new KeyValuePair<string,string>("ImageUrl",model.ImageUrl),
                        new KeyValuePair<string, string>("Label",model.Label),
                        new KeyValuePair<string, string>("owner",model.owner),
                        new KeyValuePair<string, string>("Share",model.share)

                };
                var content = new FormUrlEncodedContent(body);
          //      HttpResponseMessage response = await client.PostAsync(returnurl, content);

                //

                var response = await client.PostAsync(returnurl, new FormUrlEncodedContent(body));

                // var response = await httpClient.PostAsync(returnurl, new FormUrlEncodedContent(body));

                var contents = await response.Content.ReadAsStringAsync();
         //       return contents;

                return 1;
            }
            catch (Exception ex)
            {

                Logger.Write(ex.ToString()); ;
            }
            return 0;

        }



        public async Task<string> ConsumePostchipApi(tblLabelNotes model, string token)
        {
            try
            {
                var client = new HttpClient();
                string returnurl = currenturl + "/api/NotesApi/labelshow";

                client.BaseAddress = new Uri(returnurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



                //var response = await client.GetAsync(returnurl);
                //var contents = await response.Content.ReadAsStringAsync();
                //var data = JsonConvert.DeserializeObject<List<tblLabelNotes>>(contents);

                var body = new List<KeyValuePair<string, string>>
                {
                        new KeyValuePair<string, string>("ID", Convert.ToString(model.ID)),
                        new KeyValuePair<string, string>("LabelID", model.LabelID),
                        new KeyValuePair<string, string>("UserID", Convert.ToString(model.UserID)),
                         new KeyValuePair<string, string>("NotesID", Convert.ToString(model.NotesID))


                };
                //var content = new FormUrlEncodedContent(body);
                var response = await client.PostAsync(returnurl, new FormUrlEncodedContent(body));

                // var response = await httpClient.PostAsync(returnurl, new FormUrlEncodedContent(body));

                var contents = await response.Content.ReadAsStringAsync();
                return contents;

            }
            catch (Exception ex)
            {

                Logger.Write(ex.ToString()); ;
            }
            return "";



        }

        public async Task<string> ConsumePostownerApi(tblCollaborator model, string token)
        {
            try
            {
                var client = new HttpClient();
                string returnurl = currenturl + "/api/NotesApi/coll";

                client.BaseAddress = new Uri(returnurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



                //var response = await client.GetAsync(returnurl);
                //var contents = await response.Content.ReadAsStringAsync();
                //var data = JsonConvert.DeserializeObject<List<tblLabelNotes>>(contents);

                var body = new List<KeyValuePair<string, string>>
                {
                        new KeyValuePair<string, string>("ID", Convert.ToString(model.ID)),
                        new KeyValuePair<string, string>("UserID", Convert.ToString(model.UserID)),
                         new KeyValuePair<string, string>("NotesID", Convert.ToString(model.NotesID)),
                            new KeyValuePair<string, string>("OwnerID", Convert.ToString(model.OwnerID)),
                               new KeyValuePair<string, string>("SharID", Convert.ToString(model.SharID)),



                };
                //var content = new FormUrlEncodedContent(body);
                var response = await client.PostAsync(returnurl, new FormUrlEncodedContent(body));

                // var response = await httpClient.PostAsync(returnurl, new FormUrlEncodedContent(body));

                var contents = await response.Content.ReadAsStringAsync();
                return contents;

            }
            catch (Exception ex)
            {

                Logger.Write(ex.ToString()); ;
            }
            return "";



        }


        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Signin(LoginViewModel model, string returnUrl)
        {

            if (!ModelState.IsValid)
            {
                TempData["InEmailPassword"] = "Invalid Email or Password";
                return View(model);
            }
            else
            {
                var result = await AccountApicontroller.Signin(model, returnUrl);
                //switch (result)
                //{
                //    case SignInStatus.Success:
                //        TempData["Login"] = "Logged In Successfully";

                //        TempData["access_token"] = token;
                //        return RedirectToAction("GetNotes", "Notes");
                //    case SignInStatus.LockedOut:
                //        return View("Lockout");
                //    case SignInStatus.RequiresVerification:
                //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                //    case SignInStatus.Failure:
                //    default:
                //        TempData["LoginFaliure"] = "Invalid Login Attempt";
                //        //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ClientScript", "alert('hello')", true);
                //        return View(model);
                //}
                switch (result)
                {
                    case SignInStatus.Success:
                        TempData["Login"] = "Logged In Successfully";
                        TempData["access_token"] = token;

                        ApplicationUser Useradta = await accountService.GetUser(model.Username, 1);
                        email = Useradta.Email;
                        //  TempData["Email"] = email;
                        TempData["UserID"] = Useradta.Id;
                        Session["EmailId"] = Useradta.Email;
                        TempData["Name"] = Useradta.FirstName;
                        TempData["ExternalLogin"] = "Log";
                        TempData.Keep();
                        return RedirectToAction("GetNotes", "Notes");

                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        TempData["LoginFaliure"] = "Invalid Login attempt";
                        return View(model);
                }
            }
        }

        [AllowAnonymous]
        [Route("Token")]
        public async Task<string> Token(LoginViewModel context)
        {
            try
            {

                string access_token = await AccountApicontroller.GenerateTokenAsync(context);

                token = access_token;
                TempData["access_token"] = access_token;

                return access_token;
            }
            catch (Exception ex)
            {
                Logger.Write(ex.ToString());
            }

            return "Error";
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await AccountApicontroller.ConfirmEmail(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Verify()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]

        public void CheckCodeAsync(string name)
        {

            VerifyPhoneNumberBindingModel model = new VerifyPhoneNumberBindingModel
            {
                Username = name
            };

            testmethodAsync(model);
            //  await AccountApicontroller.SendPhoneNumber(model);
        }

        public async Task testmethodAsync(VerifyPhoneNumberBindingModel model)
        {
            await AccountApicontroller.SendPhoneNumber(model);
        }



        //[AllowAnonymous]
        //[HttpGet]
        //public async Task<ActionResult> Verifycode(VerifyPhoneNumberBindingModel model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        //await AccountApicontroller.SendPhoneNumber(model);
        //        //ViewBag.otp = model.Code;
        //    }
        //    return PartialView("Verify");
        //}

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Verifyotp(VerifyPhoneNumberBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            await AccountApicontroller.Verifyotp(model);
            return View("Signin");
        }


        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                await AccountApicontroller.ForgotPassword(model);
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await AccountApicontroller.ResetPassword(model);
            if (result != "Error")
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            return View();
        }


        public async Task<List<tblNotes>> ConsumeApi(string returnUrl, string token, string UserId)
        {
            var list = new List<tblNotes>();
            try
            {

                var client = new HttpClient();

                string returnurl = currenturl + "/api/NotesApi/GetNotes?UserId=" + UserId;

                client.BaseAddress = new Uri(returnurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



                var response = await client.GetAsync(returnurl);
                var contents = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<tblNotes>>(contents);

                foreach (tblNotes item in data)
                {
                    list.Add(item);
                }

            }
            catch (Exception ex)
            {

                Logger.Write(ex.ToString()); ;
            }
            return list;
        }



        public async Task<int> ConsumePostApi(tblNotes model)
        {
            try
            {
                var client = new HttpClient();
                string returnurl = currenturl + "/api/NotesApi/AddNote";

                client.BaseAddress = new Uri(returnurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var body = new List<KeyValuePair<string, string>>
                {
                        new KeyValuePair<string, string>("ID", Convert.ToString(model.ID)),
                        new KeyValuePair<string, string>("Content", model.Content),
                        new KeyValuePair<string, string>("UserID", Convert.ToString(model.UserID)),
                        new KeyValuePair<string, string>("Title", model.Title),
                        new KeyValuePair<string, string>("Mode",Convert.ToString(model.Mode)),
                        new KeyValuePair<string,string>("IsPin",Convert.ToString(model.IsPin)),
                        new KeyValuePair<string,string>("ColorCode",(model.ColorCode)),
                        new KeyValuePair<string,string>("Reminder",(model.Reminder)),
                        new KeyValuePair<string,string>("IsArchive",Convert.ToString(model.IsArchive)),
                        new KeyValuePair<string,string>("IsActive",Convert.ToString(model.IsActive)),
                        new KeyValuePair<string,string>("IsDelete",Convert.ToString(model.IsDelete)),
                        new KeyValuePair<string,string>("IsTrash",Convert.ToString(model.IsTrash)),
                        new KeyValuePair<string,string>("ImageUrl",model.ImageUrl)

                };
                var content = new FormUrlEncodedContent(body);
                HttpResponseMessage response = await client.PostAsync(returnurl, content);
                return 1;
            }
            catch (Exception ex)
            {

                Logger.Write(ex.ToString()); ;
            }
            return 0;

        }
        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        //protected override void Dispose(bool disposing)


        //{
        //    if (disposing)
        //    {
        //        if (_userManager != null)
        //        {
        //            _userManager.Dispose();
        //            _userManager = null;
        //        }

        //        if (_signInManager != null)
        //        {
        //            _signInManager.Dispose();
        //            _signInManager = null;
        //        }
        //    }

        //    base.Dispose(disposing);
        //}

        [HttpPost]
        [AllowAnonymous]


        public ActionResult ExternalLogin(string provider, string returnUrl)
        {

            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();



            if (loginInfo == null)
            {
                return RedirectToAction("Signin");
            }

            Session["EmailId"] = loginInfo.Email;

            TempData["ExternalLogin"] = loginInfo.Login.LoginProvider.ToString();

            if (loginInfo.Login.LoginProvider == "Facebook")
            {



                string SaveLocation = HttpContext.Server.MapPath("~/Images");
                var identifier = loginInfo.ExternalIdentity.FindFirstValue(ClaimTypes.NameIdentifier);

                var email = loginInfo.ExternalIdentity.FindFirstValue(ClaimTypes.Email);
                var name = loginInfo.ExternalIdentity.FindFirstValue(ClaimTypes.Name);
                var dob = loginInfo.ExternalIdentity.FindFirstValue(ClaimTypes.DateOfBirth);



                var picture = $"https://graph.facebook.com/{identifier}/picture?type=large";


                using (WebClient client = new WebClient())
                {
                    client.DownloadFile(new Uri(picture), SaveLocation + "/" + loginInfo.DefaultUserName + ".jpg");
                    url = currenturl + "/Images/" + loginInfo.DefaultUserName + ".jpg";

                }

            }



            // Sign in the user with this external login provider if the user already has a login
            var result = await AccountApicontroller.ExternalLoginCallback(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    Session["EmailId"] = loginInfo.Email.ToString();
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                //return View("ExternalLoginFailure");
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    ViewBag.LoginProvider = loginInfo.Login.ProviderKey;

                    var email = loginInfo.Email;
                    var name = loginInfo.DefaultUserName;

                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email, Firstname = name, ProfileUrl = url });
            }
        }


        [HttpPost]
        [System.Web.Http.HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [AllowAnonymous]
        public async Task<string> RegisterExternal(ExternalLoginBindingModel external)
        {
            //var info = await AuthenticationManager.GetExternalLoginInfoAsync();

            AccountService accountsRepository = new AccountService();
            var result = await accountsRepository.RegisterExternal(external);
            TempData["UserID"] = result;
            return result;
        }


        //private IAuthenticationManager Authentication
        //{
        //    get { return Request.GetOwinContext().Authentication; }
        //}

        //// GET api/Account/ExternalLogin
        //[OverrideAuthentication]
        ////[HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]

        //[AllowAnonymous]
        //[Route("ExternalLogin", Name = "ExternalLogin")]
        //public async Task<System.Web.Http.IHttpActionResult> GetExternalLogin(string provider, string error = null)
        //{
        //    string redirectUri = string.Empty;

        //    if (error != null)
        //    {
        //        //return BadRequest(Uri.EscapeDataString(error));
        //    }

        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return new ChallengeResult1(provider,this.Request);
        //    }

        //    var redirectUriValidationResult = ValidateClientAndRedirectUri(this.Request, ref redirectUri);

        //    if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
        //    {
        //        //return BadRequest(redirectUriValidationResult);
        //    }

        //    ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

        //    if (externalLogin == null)
        //    {
        //        //return InternalServerError();
        //    }

        //    if (externalLogin.LoginProvider != provider)
        //    {
        //        Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //        return new ChallengeResult(provider, this);
        //    }

        //    IdentityUser user = await accountService.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

        //    bool hasRegistered = user != null;

        //    redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}",
        //                                    redirectUri,
        //                                    externalLogin.ExternalAccessToken,
        //                                    externalLogin.LoginProvider,
        //                                    hasRegistered.ToString(),
        //                                    externalLogin.UserName);

        //    return Redirect(redirectUri);

        //}
        //private string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        //{

        //    Uri redirectUri;

        //    var redirectUriString = GetQueryString(request, "redirect_uri");

        //    if (string.IsNullOrWhiteSpace(redirectUriString))
        //    {
        //        return "redirect_uri is required";
        //    }

        //    bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

        //    if (!validUri)
        //    {
        //        return "redirect_uri is invalid";
        //    }

        //    var clientId = GetQueryString(request, "client_id");

        //    if (string.IsNullOrWhiteSpace(clientId))
        //    {
        //        return "client_Id is required";
        //    }

        //    var client = _repo.FindClient(clientId);

        //    if (client == null)
        //    {
        //        return string.Format("Client_id '{0}' is not registered in the system.", clientId);
        //    }

        //    if (!string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority), StringComparison.OrdinalIgnoreCase))
        //    {
        //        return string.Format("The given URL is not allowed by Client_id '{0}' configuration.", clientId);
        //    }

        //    redirectUriOutput = redirectUri.AbsoluteUri;

        //    return string.Empty;

        //}

        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null) return null;

            var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

            if (string.IsNullOrEmpty(match.Value)) return null;

            return match.Value;
        }
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return View("ExternalLoginFailure");
        //        }
        //        var user = new ApplicationUser()
        //        {
        //            UserName = model.Email,
        //            Email = model.Email,
        //            //BirthDate = model.BirthDate,
        //            //HomeTown = model.HomeTown

        //        };
        //        IdentityResult result = await UserManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await SignInAsync(user, isPersistent: false);

        //                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
        //                // Send an email with this link
        //                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //                // SendEmail(user.Email, callbackUrl, "Confirm your account", "Please confirm your account by clicking this link");

        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}


        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Redirect", "Home");
            //return RedirectToAction("GetNotes", "Notes");
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {



            if (ModelState.IsValid)
            {
                var result = await AccountApicontroller.ExternalLoginConfirmation(model, returnUrl);
                if (result.Succeeded)
                {
                    return RedirectToAction("Redirect", "Home");
                    //return RedirectToAction("GetNotes", "Notes");

                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }


        #endregion
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Logout()
        {
            TempData.Remove("access_token");
            TempData.Remove("UserId");
            FormsAuthentication.SignOut();
            Session.Abandon(); // 
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalBearer);
            AuthenticationManager.SignOut(CookieAuthenticationDefaults.AuthenticationType);

            AccountApicontroller.Logout();
            return View("Signin");
        }

    }
}




