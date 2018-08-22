using FundooNotes.Services;
using FundooNotesData.Data.Models;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FundooNotes.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {

        NotesApicontroller notesApicontroller = new NotesApicontroller();
        AccountController accountController = new AccountController();


        static List<tblNotes> list = new List<tblNotes>();
        public static string token = string.Empty;
        public static string UserId = string.Empty;
        public static string Email = string.Empty;
        public static string External = string.Empty;

        string abc;
        [HttpGet]
        [Route("GetNotes")]
        public async Task<ActionResult> GetNotes()
        {
            //try
            //{
            //    var token = TempData["access_token"].ToString();
            //    TempData.Keep("access_token");

            //    list = await accountController.ConsumeApi("", token, UserId);


            //    ViewBag.reminderdata = list;
            //    return View(list);
            //}
            //catch (Exception ex)
            //{
            //    ex.ToString();
            //    // Logger.Write(ex.ToString());
            //    return RedirectToAction("Signin", "Account");
            //}
            try
            {
                Email = Session["EmailId"].ToString();
                token = TempData["access_token"].ToString();
                try
                {
                     abc= TempData["UserID"].ToString();
                    UserId = TempData["UserID"].ToString();
                }
                catch (Exception ex)
                {

                    throw;
                }
              
                External = TempData["ExternalLogin"].ToString();
                TempData.Keep();

                if (External != "Log")
                {
                    list = notesApicontroller.GetNotes(UserId);
                }
                else
                {
                    list = await accountController.ConsumeApi("", token, UserId);
                }

                ViewBag.data = list;
                return View(list);
            }
            catch (Exception ex)
            {
                //Logger.Write(ex.ToString());
                // Logger.Write(ex.ToString());
                return RedirectToAction("Signin", "Account");
            }


        }

        [HttpPost]
        [Route("GetNotes")]
        public async Task<ActionResult> GetNotes(tblNotes objtbl)
        {
            token = TempData["access_token"].ToString();
            TempData.Keep();
            objtbl.UserID = UserId;
            int result = 0;
            string re = string.Empty;
            //if (ModelState.IsValid)
            //{
            //    i = await notesApicontroller.AddNotes(objtbl);
            //}


            if (ModelState.IsValid)
            {

                if (External != "Log")
                {
                    result = await notesApicontroller.AddNotes(objtbl);
                }
                else
                {
                    result = await accountController.ConsumePostApi(objtbl, token);
                }


                return View();
            }
            return View();
        }

        [Route("Reminder")]
        public async Task<ActionResult> Reminder()
        {
            try
            {
                // var token = TempData["access_token"].ToString();
                Email = Session["EmailId"].ToString();
                token = TempData["access_token"].ToString();
                UserId = TempData["UserID"].ToString();
                External = TempData["ExternalLogin"].ToString();
                TempData.Keep();
                // TempData.Keep("access_token");

                if (External != "log" && External != "Log")
                {
                    list = notesApicontroller.GetNotes(UserId);
                }

                else
                {
                    list = await accountController.ConsumeApi("", token, UserId);
                }

                ViewBag.data = list;
                return View(list);
            }
            catch (Exception ex)
            {

                // Logger.Write(ex.ToString());
                return RedirectToAction("Login", "Account");
            }
        }

        [Route("Archive")]
        public async Task<ActionResult> Archive()
        {
            try
            {
                // var token = TempData["access_token"].ToString();
                Email = Session["EmailId"].ToString();
                token = TempData["access_token"].ToString();
                UserId = TempData["UserID"].ToString();
                External = TempData["ExternalLogin"].ToString();
                TempData.Keep();
                // TempData.Keep("access_token");

                if (External != "log" && External != "Log")
                {
                    list = notesApicontroller.GetNotes(UserId);
                }

                else
                {
                    list = await accountController.ConsumeApi("", token, UserId);
                }

                ViewBag.data = list;
                return View(list);
            }
            catch (Exception ex)
            {

                // Logger.Write(ex.ToString());
                return RedirectToAction("Login", "Account");
            }
        }
        //POST: Notes/UpdatePin



        [Route("Trash")]
        /// <summary>
        /// Trash
        /// </summary>
        /// <returns></returns>
        ///
        public async Task<ActionResult> Trash()
        {
            try
            {
                Email = Session["EmailId"].ToString();
                token = TempData["access_token"].ToString();
                UserId = TempData["UserID"].ToString();
                External = TempData["ExternalLogin"].ToString();
                TempData.Keep();
                // TempData.Keep("access_token");

                if (External != "log")
                {
                    list = notesApicontroller.GetNotes(UserId);
                }

                else
                {
                    list = await accountController.ConsumeApi("", token, UserId);
                }

                ViewBag.data = list;
                return View(list);
            }
            catch (Exception ex)
            {
                //Logger.Write(ex.ToString());

                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        [Route("UpdatePin")]
        public async Task<ActionResult> UpdatePin(tblNotes model)
        {
            int i = 0;
            if (ModelState.IsValid)
            {
                i = await notesApicontroller.UpdatePin(model);
            }
            return View("GetNotes");
        }
        //POST: Notes/UpdateColor
        [HttpPost]

        [Route("UpdateColor")]
        public async Task<ActionResult> UpdateColor(tblNotes model)
        {
            int i = 0;

            i = await notesApicontroller.UpdateColor(model);

            return View("GetNotes");

        }
        //[HttpPost]
        //public async Task<ActionResult> UploadImage (tblNotes model)
        //{
        //    return View("getnotes");
        //}

        [HttpGet]
        [Route("GetNoteExternal")]
        public async Task<ActionResult> GetNoteExternal(string Token)
        {

            AccountService accountService = new AccountService();
            //  string access_token = await objBaseApiController.GenerateTokenAsync(context);
            string email = string.Empty;
            try
            {
                token = Token;

                TempData["access_token"] = Token;
                email = Session["EmailId"].ToString();
                ApplicationUser UserData = await accountService.GetUserdata(email);
                // Session["EmailId"] = UserData.Email;
                TempData["UserID"] = UserData.Id;
                TempData["Name"] = UserData.FirstName;
                TempData.Keep();
                UserId = TempData["UserID"].ToString();
                NotesApicontroller noteApiController = new NotesApicontroller();

                list = noteApiController.GetNotesexternal(UserData.Id);
                ViewBag.data = list;
                return View("GetNotes", list);
            }
            catch (Exception ex)
            {
                // Logger.Write(ex.ToString());

                return RedirectToAction("Login", "Account");
            }


        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetNotesPopup()
        {
            return View();
        }

        //[Route("AddLabel")]
        //public ActionResult AddLabel()
        //{
        //    var list1 = new List<tblLabel>();
        //    list1 = notesApicontroller.GetLabel();
        //    return View(list1);

        //}
        //[HttpGet]
        //[Route("AddLabel")]
        //public string AddLabel()
        //{
        //    string JSONString = string.Empty;
        //    var list1 = new List<tblLabel>();
        //    list1 = notesApicontroller.GetLabel();

        //    JSONString = JsonConvert.SerializeObject(list1);
        //    return JSONString;
        //}
        //[HttpPost]
        //[Route("AddLabel")]
        //public async Task<ActionResult> AddLabel(tblLabel objtbllabel)
        //{
        //    UserId = TempData["UserID"].ToString();
        //    TempData.Keep();
        //    int result = 0;
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            objtbllabel.UserID = UserId;
        //            result = await notesApicontroller.AddLabel(objtbllabel);

        //            return View();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        //Logger.Write(ex.ToString());
        //    }
        //    return View(result);
        //}



        [Route("AddLabel")]
        public async Task<string> AddLabelAsync(tblLabel objtbllabel)
        {
            try
            {
                objtbllabel.UserID = TempData["UserID"].ToString();
            }
            catch (Exception EX)
            {

                throw;
            }

            string JSONString = string.Empty;
            if (objtbllabel.Label == null)
            {
                string userid = TempData["UserID"].ToString();
                TempData.Keep();
                var list1 = new List<tblLabel>();
                list1 = notesApicontroller.GetLabel(userid);

                JSONString = JsonConvert.SerializeObject(list1);
                return JSONString;



            }
            else
            {
                int result = 0;
                //UserId = TempData["UserID"].ToString();
                TempData.Keep();
                try
                {
                    if (ModelState.IsValid)
                    {
                       
                        // value = Addlablemethod(objtbllabel);
                        result = await notesApicontroller.AddLabel(objtbllabel);
                        var list1 = new List<tblLabel>();
                        list1 = notesApicontroller.GetLabel(TempData["UserID"].ToString());

                        JSONString = JsonConvert.SerializeObject(list1);
                        return JSONString;
                    }
                }
                catch (Exception ex)
                {

                    //Logger.Write(ex.ToString());
                }
            }

            return JSONString;
        }

        public async Task<int> Addlablemethod(tblLabel objtbllabel)
        {
            int result = 0;
            result = await notesApicontroller.AddLabel(objtbllabel);
            return result;
        }
        //For adding lable in Notes
        [HttpGet]
        public ActionResult SelectLabel()
        {
           string userid= TempData["UserID"].ToString();
            var list1 = new List<tblLabel>();
            list1 = notesApicontroller.GetLabel(userid);
            return View(list1);
        }

        [HttpPost]
        [Route("addnotelabel")]

        public async Task<ActionResult> addnotelabel(tblNotes objtbl)
        {
            token = TempData["access_token"].ToString();
            TempData.Keep();
            objtbl.UserID = UserId;
            int result = 0;
            string re = string.Empty;

            if (External != "Log")
            {
                result = await notesApicontroller.addnotelabel(objtbl);
            }
            else
            {
                result = await accountController.ConsumePostLabelApi(objtbl, token);
            }


            //string userid = TempData["UserID"].ToString();
            //int i = 0;
            //if (ModelState.IsValid)
            //{
            //    i = await notesApicontroller.addnotelabel(objtbl);
            //}
            //  labelshow();

            TempData["UserID"] = UserId;
            TempData.Keep();
            return View("GetNotes");

        }



        [HttpPost]
        [Route("labelshow")]

        //public string labelshow()
        //{
        //    tblLabelNotes objtbl = new tblLabelNotes();
        //    string userid=TempData["UserID"].ToString();
        //    Tuple<List<tblLabel>, List<tblLabelNotes>> result=null;
        //    //int result = 0;
        //    if (ModelState.IsValid)
        //    {
        //        result = notesApicontroller.labelshow(objtbl, userid);
        //    }
        //    string JSONString;
        //    JSONString = JsonConvert.SerializeObject(result);

        //    return JSONString;
        //}
        public async Task<string> labelshow()
        {
            token = TempData["access_token"].ToString();
            TempData.Keep();
            tblNotes obj = new tblNotes();
            obj.UserID = UserId;
            int result1 = 0;
            string re = string.Empty;
            // Tuple<List<tblLabel>, List<tblLabelNotes>> result = null;
            string ABC = "";


            tblLabelNotes objtbl = new tblLabelNotes();
            string userid = TempData["UserID"].ToString();
            objtbl.UserID = userid;
            string myString;

            if (External != "Log")
            {
                ABC = await notesApicontroller.labelshow(objtbl);
                myString = ABC;
            }
            else
            {
                ABC = await accountController.ConsumePostchipApi(objtbl, token);
                ABC = ABC.Replace("\\", "");
                 myString = ABC;

                myString = myString.Substring(0, myString.Length - 1);
                int index1 = myString.IndexOf('"');
                if (index1 != -1)
                {
                    myString = myString.Remove(index1, 1);
                }
            }
           
            // Tuple<List<tblLabel>, List<tblLabelNotes>> result=null;
            //int result = 0;
            //if (ModelState.IsValid)
            //{
            //    result = notesApicontroller.labelshow(objtbl, userid);
            //}
            //string JSONString;
            // JSONString = JsonConvert.SerializeObject(ABC);

            return myString;
        }

        [HttpGet]

        public ActionResult Displaylabel(string Displaylabel)

        {
            var list1 = new List<tblNotes>();
            list1 = notesApicontroller.Displaylabel(Displaylabel, UserId);
            ViewBag.data = list;
            return View(list1);
        }
        [HttpGet]
        // Get: Notes/Collaborator
        public ActionResult Collaborators()
        {

            return View();
        }
        [Route("AddCollaborator")]
        public async Task<ActionResult> AddCollaborator(tblCollaborator objtbllabel)
        {
            if(objtbllabel.Mode==1)
            {
                objtbllabel.OwnerID = Session["EmailId"].ToString();
            }
          
            int result = 0;
            result = await notesApicontroller.AddCollaboratorAsync(objtbllabel);
            return View("GetNotes");
        }
        [HttpPost]
        [Route("OwnerLabel")]
        public async Task<string> OwnerLabel(tblNotes obj)
        {

            token = TempData["access_token"].ToString();
            TempData.Keep();
      
            int result1 = 0;
            string re = string.Empty;
            // Tuple<List<tblLabel>, List<tblLabelNotes>> result = null;
            string ABC = "";


            tblCollaborator objtbl = new tblCollaborator();
            string userid = TempData["UserID"].ToString();
            objtbl.UserID = userid;
            objtbl.NotesID = obj.ID;
            string myString="";

            if (External != "Log")
            {
                ABC = await notesApicontroller.Coll(objtbl);
                myString = ABC;
            }
            else
            {
                ABC = await accountController.ConsumePostownerApi(objtbl, token);
                ABC = ABC.Replace("\\", "");
                myString = ABC;

                myString = myString.Substring(0, myString.Length - 1);
                int index1 = myString.IndexOf('"');
                if (index1 != -1)
                {
                    myString = myString.Remove(index1, 1);
                }
            }


            return myString;
        }

      
    }
}