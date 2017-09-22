using Sparks.Models.WorkModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sparks.Controllers
{
    public class DatabaseController : Controller
    {
        public ActionResult Index()
        {
            return View(DbExtra.TableNames());
        }

        public ActionResult GetTableById(int ID)
        {
            throw new NotImplementedException();
        }

        public ActionResult SubmitDataByTableId(int ID)
        {
            throw new NotImplementedException();
        }
    }
}