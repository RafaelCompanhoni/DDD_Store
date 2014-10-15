using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LuaBijoux.Web.Areas.Admin.Controllers
{
    public class NavController : Controller
    {
        public PartialViewResult TopMenu()
        {
            return PartialView();
        }

        public PartialViewResult Menu(string category = "dashboard", string subcategory = null)
        {
            ViewBag.SelectedCategory = category;
            ViewBag.SelectedSubcategory = subcategory;

            return PartialView();
        }
	}
}