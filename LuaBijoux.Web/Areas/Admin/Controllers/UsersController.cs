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
            return (user != null && Id != user.Id.ToString()) ? Json("O e-mail informado já foi registrado.", JsonRequestBehavior.AllowGet) : Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Verifica se o cpf informado já foi registrado 
        /// </summary>
        /// <param name="cpf">CPF a consultar</param>
        /// <param name="Id">ID do usuário</param>
        /// <returns></returns>
        public JsonResult ValidateCpf(string cpf, string Id)
        {
            if (!IsCpfWellFormed(cpf))
            {
                return Json("O CPF informado é inválido", JsonRequestBehavior.AllowGet);
            }

            string parsedCpf = cpf.Replace(".", "").Replace("-", "");
            AppUser user = _userManager.FindByCpf(parsedCpf);
            return (user != null && Id != user.Id.ToString()) ? Json("O CPF informado já foi registrado.", JsonRequestBehavior.AllowGet) : Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Verifica se o CPF informado é válido
        /// </summary>
        /// <param name="cpf">CPF a ser validado</param>
        /// <returns></returns>
        private bool IsCpfWellFormed(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito); 
        }
    }
}