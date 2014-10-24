using System;
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
        public async Task<ActionResult> Create(CreateUserVM createUserVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createUserVM);
            }

            AppUser user = Mapper.Map<CreateUserVM, AppUser>(createUserVM);
            var result = await _userManager.CreateAsync(user, createUserVM.Password);

            if (!result.Succeeded)
            {
                TempData["status"] = "alert-danger";
                TempData["message"] = string.Format("Não foi possível criar o usuário - erro no acesso ao banco de dados.");
                return View(createUserVM);
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
                return RedirectToAction("Users");
            }

            AppUser user = await _userManager.FindByIdAsync(id ?? default(int));

            if (user == null)
            {
                return HttpNotFound();
            }

            EditUserVM userViewModel = Mapper.Map<AppUser, EditUserVM>(user);
            return View(userViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserVM editUserVM)
        {
            if (!ModelState.IsValid)
            {
                return View(editUserVM);
            }

            AppUser user = await _userManager.FindByIdAsync(Int32.Parse(editUserVM.Id));
            Mapper.Map(editUserVM, user);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                TempData["status"] = "alert-danger";
                TempData["message"] = string.Format("Não foi possível modificar o usuário - erro no acesso ao banco de dados.");
                return View(editUserVM);
            }

            TempData["status"] = "alert-success";
            TempData["message"] = string.Format("Usuário alterado com sucesso. E-mail: <strong>{0}</strong>", user.Email);

            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Users");
            }

            var result = await _userManager.DeleteAsync((int) id);

            if (!result.Succeeded)
            {
                TempData["status"] = "alert-danger";
                TempData["message"] = string.Format("Não foi possível excluir o usuário - erro no acesso ao banco de dados.");
            }
            else
            {
                TempData["status"] = "alert-success";
                TempData["message"] = string.Format("Usuário excluído com sucesso.");
            }

            return RedirectToAction("Users");
        }

        /// <summary>
        /// Verifica se o email informado já foi registrado 
        /// </summary>
        /// <param name="email">Email a consultar</param>
        /// <param name="userId">ID do usuário</param>
        /// <returns>True caso o email já tenha sido registrado</returns>
        public JsonResult IsEmailAlreadyRegistered(string email, string userId)
        {
            AppUser user = _userManager.FindByEmail(email);
            return (user != null && userId != user.Id.ToString()) ? Json(false, JsonRequestBehavior.AllowGet) : Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Verifica se o cpf informado já foi registrado 
        /// </summary>
        /// <param name="cpf">CPF a consultar</param>
        /// <param name="userId">ID do usuário</param>
        /// <returns></returns>
        public JsonResult IsCpfAlreadyRegistered(string cpf, string userId)
        {
            string parsedCpf = cpf.Replace(".", "").Replace("-", "");
            AppUser user = _userManager.FindByCpf(parsedCpf);
            return (user != null && userId != user.Id.ToString()) ? Json(false, JsonRequestBehavior.AllowGet) : Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}