using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LuaBijoux.Web.Areas.Admin.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult Error()
        {
            return View();
        }
	}
}