using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class CarrinhoCompraController : Controller
    {

        private readonly ILancheRepository _lancheRepository;
        private readonly CarrinhoCompra _carrinhoCompra;

        // Injentando as instâncias de Carrinho de compras e LancheRepository
        public CarrinhoCompraController(ILancheRepository lancheRepository, CarrinhoCompra carrinhoCompra)
        {
            _lancheRepository = lancheRepository;
            _carrinhoCompra = carrinhoCompra;
        }

        public IActionResult Index()
        {
            //Recuperando os itens do carrinho
            var itens = _carrinhoCompra.GetCarrinhoCompraItems();
            _carrinhoCompra.CarrinhoCompraItens = itens;

            // Instanciando uma novo ViewModel
            var carrinhoCompraVM = new CarrinhoCompraViewModel
            {
                CarrinhoCompra = _carrinhoCompra,
                CarrinhoCompraTotal = _carrinhoCompra.GetCarrinhoCompraTotal()
            };
           
            return View(carrinhoCompraVM);
        }

        [Authorize]
        public RedirectToActionResult AdicionarItemNoCarrinhoCompra(int lancheId)
        {
            // Recuperando o lanche de acordo com o Id Passado.
            var lancheSelecionado = _lancheRepository.Lanches
                    .FirstOrDefault(p => p.LancheId == lancheId);
            
            if(lancheSelecionado != null){
                _carrinhoCompra.AdicionarAoCarrinho(lancheSelecionado);
            }

            return RedirectToAction("Index");  
        }

        [Authorize]
        public RedirectToActionResult RemoverItemNoCarrinhoCompra(int lancheId)
        {
            // Recuperando o lanche de acordo com o Id Passado.
            var lancheSelecionado = _lancheRepository.Lanches
                    .FirstOrDefault(p => p.LancheId == lancheId);

            if (lancheSelecionado != null)
            {
                _carrinhoCompra.RemoverDoCarrinho(lancheSelecionado);
            }

            return RedirectToAction("Index");
        }
    }
}
