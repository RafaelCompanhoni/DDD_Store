using System;
using System.Net;
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

        public async Task<ActionResult> Index()
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

            try
            {
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
            }
            catch (Exception)
            {
                TempData["status"] = "alert-danger";
                TempData["message"] = string.Format("Não foi possível criar o usuário - erro no acesso ao banco de dados.");
                return View(createUserVM);
            }

            string redirectionTarget = Request.Form["save-and-back"] != null ? "Index" : "Create";
            return RedirectToAction(redirectionTarget);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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

            try
            {
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
            }
            catch (Exception)
            {
                TempData["status"] = "alert-danger";
                TempData["message"] = string.Format("Não foi possível criar o usuário - erro no acesso ao banco de dados.");
                return View(editUserVM);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                var result = await _userManager.DeleteAsync((int)id);

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
            }
            catch (Exception)
            {
                TempData["status"] = "alert-danger";
                TempData["message"] = string.Format("Não foi possível criar o usuário - erro no acesso ao banco de dados.");
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Verifica se o email informado já foi registrado 
        /// </summary>
        /// <param name="email">Email a consultar</param>
        /// <param name="userId">ID do usuário</param>
        /// <returns>True caso o email já tenha sido registrado</returns>
        public JsonResult IsEmailAlreadyRegistered(string email, string Id)
        {
            AppUser user = _userManager.FindByEmail(email);
            return (user != null && Id != user.Id.ToString()) ? Json(false, JsonRequestBehavior.AllowGet) : Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Verifica se o cpf informado já foi registrado 
        /// </summary>
        /// <param name="cpf">CPF a consultar</param>
        /// <param name="userId">ID do usuário</param>
        /// <returns></returns>
        public JsonResult IsCpfAlreadyRegistered(string cpf, string Id)
        {
            string parsedCpf = cpf.Replace(".", "").Replace("-", "");
            AppUser user = _userManager.FindByCpf(parsedCpf);
            return (user != null && Id != user.Id.ToString()) ? Json(false, JsonRequestBehavior.AllowGet) : Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}