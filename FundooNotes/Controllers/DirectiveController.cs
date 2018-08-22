using FundooNotes.Services;
using FundooNotesData.Data.Infrastructure;
using FundooNotesData.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FundooNotes.Controllers
{
    public class DirectiveController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        static List<tblLabel> list = new List<tblLabel>();
        string email = string.Empty;
        NotesApicontroller notesApiController = new NotesApicontroller();
        string str;
        AccountService accountService = new AccountService();


        // GET: Directive
        public ActionResult Header()
        {
            return View();
        }
        public async Task<ActionResult> SideNav()

        {
            if (TempData["ExternalLogin"] != null)
            {

                email = Session["EmailId"].ToString();
                ApplicationUser UserData = await accountService.GetUserdata(email);
                TempData["UserID"] = UserData.Id;
                TempData["profile"] = UserData.Profile;
                TempData["Name"] = UserData.FirstName;
                TempData.Keep();
            }
            str = TempData["UserID"].ToString();

            var list = new List<tblLabel>();
            var data = from a in _context.tblLabels
                       where a.UserID == str
                       select a;
            foreach (var item in data)
            {
                list.Add(item);
            }
            return View(list);
        }
    }
}