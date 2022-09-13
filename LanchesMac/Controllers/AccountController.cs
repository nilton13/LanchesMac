using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;    
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Login com a requisição GET
        // Renderizar a página
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel()
            {
                ReturnUrl = returnUrl
            });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            // Localizando usuário pelo nome
            var user = await _userManager.FindByNameAsync(loginVM.Username);
            if(user != null)
            {
                var result = await _signInManager
                            .PasswordSignInAsync(user, loginVM.Password, false, false);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVM.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(loginVM.ReturnUrl);
                }
            }

            ModelState.AddModelError("", "Falha ao realizar o Login!!");
            return View(loginVM);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Para evitar um ataque de CSRF
        public async Task<IActionResult> Register(LoginViewModel registroVM)
        {
            if (ModelState.IsValid)
            {   // Criando uma nova instancia de IdentityUser com o usuario informado
                var user = new IdentityUser { UserName = registroVM.Username };
                // Criando usuário 
                var result = await _userManager.CreateAsync(user, registroVM.Password);

                // Verificando se foi possível realizar o cadastro.
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    this.ModelState.AddModelError("Registro","Falha ao registrar o usuário.");
                }
            }
            return View(registroVM);    
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear(); // Limpando dados armazenados da sessão
            HttpContext.User = null;
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
