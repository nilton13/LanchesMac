using System.ComponentModel.DataAnnotations;

namespace LanchesMac.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o nome")]
        [Display(Name = "Usuário")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Informe a senha")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; } // Enviar automaticamente o usuario a página que ele queria acessar
    }
}
