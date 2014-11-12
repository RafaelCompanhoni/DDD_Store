using System;
using System.Linq.Expressions;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using System.Threading.Tasks;
using LuaBijoux.Core.Data;
using LuaBijoux.Core.Identity;
using LuaBijoux.Core.DomainModels.Identity;
using LuaBijoux.Core.Logging;
using LuaBijoux.Core.Services;
using LuaBijoux.Web.Areas.Admin.Models;
using LuaBijoux.Web.Areas.Admin.Models.Users;
using LuaBijoux.Web.Infrastructure.Utils;

namespace LuaBijoux.Web.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private readonly IApplicationUserManager _userManager;
        private readonly ILogger _logger;

        public UsersController(IApplicationUserManager userManager, ILogger logger) 
        {
            _userManager = userManager;
            _logger = logger;
        }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Users(string sortOrder)
        {
            // TODO - TempData está com um funcionamento errático
            // Remover a implementação customizado de cookies

            Expression<Func<AppUser, string>> keySelector = x => x.FirstName;
            OrderBy orderBy = OrderBy.Ascending;

            switch (sortOrder)
            {
                case "name_asc":
                    keySelector = x => x.FirstName;
                    orderBy = OrderBy.Ascending;
                    TempData["NameSortOrder"] = "name_desc";
                    break;
                case "name_desc":
                    keySelector = x => x.FirstName;
                    orderBy = OrderBy.Descending;
                    TempData["NameSortOrder"] = "name_asc";
                    break;
                default:
                    keySelector = x => x.FirstName;
                    orderBy = OrderBy.Ascending;
                    TempData["NameSortOrder"] = "name_desc";
                    break;
            }

            return PartialView(_userManager.GetUsers(keySelector, orderBy));
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

            _logger.Log(String.Format("USERS - Novo usuário criado - Email: {0}", user.Email)); 

            TempData["status"] = "alert-success";
            TempData["message"] = string.Format("Usuário criado com sucesso. E-mail: <strong>{0}</strong>", user.Email);

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

            AppUser user = await _userManager.FindByIdAsync(Int32.Parse(editUserVM.Id));
            Mapper.Map(editUserVM, user);
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                TempData["status"] = "alert-danger";
                TempData["message"] = string.Format("Não foi possível modificar o usuário - erro no acesso ao banco de dados.");
                return View(editUserVM);
            }

            _logger.Log(String.Format("USERS - Usuário editado - Email: {0}", user.Email)); 

            TempData["status"] = "alert-success";
            TempData["message"] = string.Format("Usuário alterado com sucesso. E-mail: <strong>{0}</strong>", user.Email);

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
          
            var result = await _userManager.DeleteAsync((int)id);

            if (!result.Succeeded)
            {
                TempData["status"] = "alert-danger";
                TempData["message"] = string.Format("Não foi possível excluir o usuário - erro no acesso ao banco de dados.");
            }

            _logger.Log(String.Format("USERS - Usuário deletado - ID: {0}", id)); 

                TempData["status"] = "alert-success";
                TempData["message"] = string.Format("Usuário excluído com sucesso.");
            
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Verifica se o e-mail informado já foi registrado 
        /// </summary>
        /// <param name="email">Email a consultar</param>
        /// <param name="id">ID do usuário</param>
        /// <returns>True caso o email já tenha sido registrado</returns>
        public JsonResult IsEmailAlreadyRegistered(string email, string id)
        {
            AppUser user = _userManager.FindByEmail(email);
            return (user != null && id != user.Id.ToString()) ? Json("O e-mail informado já foi registrado.", JsonRequestBehavior.AllowGet) : Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Verifica se o CPF informado já foi registrado 
        /// </summary>
        /// <param name="cpf">CPF a consultar</param>
        /// <param name="id">ID do usuário</param>
        /// <returns></returns>
        public JsonResult ValidateCpf(string cpf, string id)
        {
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "").Replace("_", "");

            if (!IsCpfWellFormed(cpf))
            {
                if (cpf != "")
                {
                    _logger.Log(String.Format("USERS - Tentativa de cadastro com CPF inválido - CPF: {0}", cpf));     
                }
                
                return Json("O CPF informado é inválido", JsonRequestBehavior.AllowGet);
            }

            AppUser user = _userManager.FindByCpf(cpf);
            return (user != null && id != user.Id.ToString()) ? Json("O CPF informado já foi registrado.", JsonRequestBehavior.AllowGet) : Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Verifica se o CPF informado é válido
        /// </summary>
        /// <param name="cpf">CPF a ser validado</param>
        /// <returns></returns>
        private static bool IsCpfWellFormed(string cpf)
        {
            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            if (cpf.Length != 11)
                return false;

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();

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