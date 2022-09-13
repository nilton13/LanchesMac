using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly CarrinhoCompra _carrinhoCompra;

        public PedidoController(IPedidoRepository pedidoRepository, CarrinhoCompra carrinhoCompra)
        {
            _pedidoRepository = pedidoRepository;
            _carrinhoCompra = carrinhoCompra;
        }

        [Authorize] // Obriga o usuário estar autenticado para acessar a rota
        public IActionResult Checkout()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(Pedido pedido)
        {

            int totalItensPedido = 0;
            decimal precoTotalPedido = 0.0m;

            // Obter os itens do carrinho de compras do Cliente
            List<CarrinhoCompraItem> items = _carrinhoCompra.GetCarrinhoCompraItems();
            _carrinhoCompra.CarrinhoCompraItens = items;

            // Verificar se existem itens de pedido
            if(_carrinhoCompra.CarrinhoCompraItens.Count == 0)
            {
                ModelState.AddModelError("", "Seu carrinho está vazio, inclua um lanche para prosseguir");
            }

            // Calculando o total de itens e o total do Pedido.
            foreach(var item in items)
            {
                totalItensPedido += item.Quantidade;
                precoTotalPedido += (item.Lanche.preco * item.Quantidade);
            }

            //Atribuir os valores obtidos ao pedido
            pedido.TotalItensPedido = totalItensPedido;
            pedido.PedidoTotal = precoTotalPedido;

            // Validar os dados do pedido
            if (ModelState.IsValid)
            {
                // Criar o Pedido e os Detalhes do Pedido
                _pedidoRepository.CriarPedido(pedido);

                //Definindo mensagens ao cliente
                ViewBag.CheckoutCompletoMensagem = "Agradecemos pela preferência!";
                ViewBag.TotalPedido = _carrinhoCompra.GetCarrinhoCompraTotal();

                // Limpar o carrinho de compras
                _carrinhoCompra.LimparCarrinho();

                // Exibe a view com os dados do cliente e do pedido
                return View("~/Views/Pedido/CheckoutCompleto.cshtml", pedido);
            }

            return View(pedido);
        }
    }
}
