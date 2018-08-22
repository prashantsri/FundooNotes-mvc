using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FundooNotes.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            var url = Request.Url;
            return View();
        }


        [AllowAnonymous]
        public ActionResult Redirect()
        {
            return View();
        }
    }
}