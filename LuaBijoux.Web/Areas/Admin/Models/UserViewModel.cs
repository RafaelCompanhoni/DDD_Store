using System;
using System.ComponentModel.DataAnnotations;

namespace LuaBijoux.Web.Areas.Admin.Models
{
    public class UserViewModel
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Por favor, informe o nome.")]
        [StringLength(2, ErrorMessage = "O tamanho máximo para o nome é de 20 caracteres.")]
        public string FirstName { get; set; }

        [Display(Name = "Sobrenome")]
        [Required(ErrorMessage = "Por favor, informe o sobrenome.")]
        [StringLength(2, ErrorMessage = "O tamanho máximo para o sobrenome é de 30 caracteres.")]
        public string LastName { get; set; }

        [Display(Name = "E-Mail")]
        [Required(ErrorMessage = "Por favor, informe o e-mail.")]
        [EmailAddress(ErrorMessage = "Endereço de e-mail inválido.")]
        [StringLength(2, ErrorMessage = "O tamanho máximo para o email é de 60 caracteres.")]
        public string Email { get; set; }

        [Display(Name = "Confirmação do email")]
        [Compare("Email", ErrorMessage = "Os email informados não são idênticos.")]
        public string EmailConfirmation { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "Por favor, informe o CPF.")]
        [StringLength(2, ErrorMessage = "O tamanho máximo para o CPF é de 11 dígitos.")]
        public string Cpf { get; set; }

        [Display(Name = "Data de nascimento")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Por favor, informe a data de nascimento.")]
        [DataType(DataType.Date)]
        public DateTime? Birthdate { get; set; }

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