using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class ContatoController : Controller
    {
        public IActionResult Index()
        {
            //Adicionando Autorização
            /*
             if(User.Identity.IsAuthenticated)
            {
                return View();
            }
             */
            return View();
        }
    }
}
