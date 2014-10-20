﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using System.Threading.Tasks;
using LuaBijoux.Core.Identity;
using LuaBijoux.Core.DomainModels.Identity;
using LuaBijoux.Web.Areas.Admin.Models;

namespace LuaBijoux.Web.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private readonly IApplicationUserManager _userManager;

        public UsersController(IApplicationUserManager userManager) 
        {
            _userManager = userManager;
        }

        public async Task<ActionResult> Users()
        {
            return View(await _userManager.GetUsersAsync());
        }

        public ActionResult Create()
        {
            return View(new CreateUserVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserVM userModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }
             
            AppUser user = Mapper.Map<CreateUserVM, AppUser>(userModel);
            var creationResult = await _userManager.CreateAsync(user, userModel.Password);

            if (!creationResult.Succeeded)
            {
                TempData["status"] = "alert-danger";
                TempData["message"] = string.Format("Não foi possível criar o usuário - erro no acesso ao banco de dados.");
                return View(userModel);
            }

            TempData["status"] = "alert-success";
            TempData["message"] = string.Format("Usuário criado com sucesso. E-mail: <strong>{0}</strong>", user.Email);

            string redirectionTarget = Request.Form["save-and-back"] != null ? "Users" : "Create";
            return RedirectToAction(redirectionTarget);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Users");
            }

            AppUser user = await _userManager.FindByIdAsync(id ?? default(int));
            if (user == null)
            {
                return HttpNotFound();
            }

            CreateUserVM userViewModel = Mapper.Map<AppUser, CreateUserVM>(user);
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CreateUserVM userModel)
        {
            TempData["status"] = "alert-success";
            TempData["message"] = string.Format("Usuário alterado com sucesso. E-mail: <strong>{0}</strong>", userModel.Email);

            return View("Users");
        }

        /// <summary>
        /// Verifica se o email informado já foi registrado -- Migrar para WebAPI
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>True caso o email já tenha sido registrado</returns>
        public async Task<JsonResult> IsEmailAlreadyRegistered(string email)
        {


            AppUser user = await _userManager.FindByEmailAsync(email);
            return (user != null) ? Json("O email informado já foi cadastrado", JsonRequestBehavior.AllowGet) : Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}