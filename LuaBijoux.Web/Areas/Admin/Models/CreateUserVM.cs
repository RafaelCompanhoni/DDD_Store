using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using LuaBijoux.Web.Infrastructure.Attributes;
using LuaBijoux.Web.Infrastructure.Validators;

namespace LuaBijoux.Web.Areas.Admin.Models
{
    public class CreateUserVM
    {
        public string Id { get; set; } 

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Por favor, informe o nome.")]
        [StringLength(20, ErrorMessage = "O tamanho máximo para o nome é de 20 caracteres.")]
        public string FirstName { get; set; }

        [Display(Name = "Sobrenome")]
        [Required(ErrorMessage = "Por favor, informe o sobrenome.")]
        [StringLength(30, ErrorMessage = "O tamanho máximo para o sobrenome é de 30 caracteres.")]
        public string LastName { get; set; }

        [Display(Name = "E-Mail")]
        [Required(ErrorMessage = "Por favor, informe o e-mail.")]
        [EmailAddress(ErrorMessage = "Endereço de e-mail inválido.")]
        [StringLength(60, ErrorMessage = "O tamanho máximo para o email é de 60 caracteres.")]
        [RemoteClientServer("IsEmailAlreadyRegistered", "Users", AdditionalFields = "Id", ErrorMessage = "O email informado já foi registrado.")]
        public string Email { get; set; }

        [Display(Name = "Confirmação do e-mail")]
        [System.ComponentModel.DataAnnotations.Compare("Email", ErrorMessage = "Os e-mails informados não são idênticos.")]
        public string EmailConfirmation { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "Por favor, informe o CPF.")]
        [Cpf(ErrorMessage = "O CPF informado não é válido.")]
        [RemoteClientServer("IsCpfAlreadyRegistered", "Users", AdditionalFields = "Id", ErrorMessage = "O CPF informado já foi registrado.")]
        public string Cpf { get; set; }

        [Display(Name = "Data de nascimento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? Birthdate { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Por favor, informe a senha.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^.{6,}$", ErrorMessage = "A senha deve conter no mínimo 6 caracteres.")]
        public string Password { get; set; }

        [Display(Name = "Confirme a senha")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "As senhas informadas devem ser iguais.")]
        public string ConfirmPassword { get; set; }
    }
}