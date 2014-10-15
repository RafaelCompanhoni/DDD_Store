using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using System.Threading.Tasks;
using LuaBijoux.Core.Identity;
using LuaBijoux.Web.Areas.Admin.Models;


namespace LuaBijoux.Web.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private IApplicationUserManager _userManager;

        public UsersController(IApplicationUserManager userManager) 
        {
            _userManager = userManager;
        }

        public async Task<ActionResult> ManageUsers()
        {
            return View(await _userManager.GetUsersAsync());
        }

        public ActionResult CreateUser()
        {
            return View("CreateUser");
        }

        [HttpPost]
        public ActionResult CreateUser(UserViewModel userModel)
        {
            string redirectTo = Request.Form["save-and-back"] != null ? "ManageUsers" : null;
            return View(redirectTo);

            /*
            if (ModelState.IsValid) 
            {
                User user = Mapper.Map<UserViewModel, User>(userModel);

                _userRepository.CreateUser(user, userModel.Password);
                TempData["message"] = string.Format("Usuário {0} foi salvo com sucesso", user.UserName);

                return View(redirectTo);
            } 
            else 
            {
                return View(userModel);
            }
            */
        }
	}
}