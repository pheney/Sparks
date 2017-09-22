using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sparks.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Sparks");
            //return View();
        }
    }
}
