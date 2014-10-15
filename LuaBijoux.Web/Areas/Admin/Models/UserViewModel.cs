using System;
using System.ComponentModel.DataAnnotations;

namespace LuaBijoux.Web.Areas.Admin.Models
{
    public class UserViewModel
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Por favor, informe o nome.")]
        [StringLength(2, ErrorMessage = "O tamanho máximo para o nome é de 50 caracteres")]
        public string FirstName { get; set; }

        [Display(Name = "Sobrenome")]
        [Required(ErrorMessage = "Por favor, informe o sobrenome.")]
        public string LastName { get; set; }

        [Display(Name = "E-Mail")]
        [Required(ErrorMessage = "Por favor, informe o e-mail.")]
        [EmailAddress(ErrorMessage = "Endereço de e-mail inválido")]
        public string Email { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "Por favor, informe o CPF.")]
        public string CPF { get; set; }

        [Display(Name = "Data de nascimento")]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        /*
        [Display(Name = "Gênero")]
        public Gender Gender { get; set; }
        */

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Por favor, informe a senha.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirme a senha")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirmação da senha incorreta.")]
        public string ConfirmPassword { get; set; }
    }
}