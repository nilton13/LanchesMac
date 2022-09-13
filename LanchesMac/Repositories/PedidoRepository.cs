using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;

namespace LanchesMac.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly CarrinhoCompra _carrinhoCompra;

        public PedidoRepository(AppDbContext appDbContext, CarrinhoCompra carrinhoCompra)
        {
            _appDbContext = appDbContext;
            _carrinhoCompra = carrinhoCompra;
        }

        public void CriarPedido(Pedido pedido)
        {
            pedido.PedidoEnviado = DateTime.Now;
            _appDbContext.Pedidos.Add(pedido); //  Incluindo o pedido no contexto
            _appDbContext.SaveChanges(); // Salvando/Persistindo os dados.


            // Recuperando os itens do carrinho de compras
            var carrinhoCompraItens = _carrinhoCompra.CarrinhoCompraItens;

            // Percorrendo todos os itens do carrinho de compras
            foreach (var carrinhoItem in carrinhoCompraItens)
            {
                // Montando os detalhes do pedido
                var pedidoDetail = new PedidoDetalhe()
                {
                    Quantidade = carrinhoItem.Quantidade,
                    LancheId = carrinhoItem.Lanche.LancheId,
                    PedidoId = pedido.PedidoId,
                    Preco = carrinhoItem.Lanche.preco
                };

                _appDbContext.PedidoDetalhes.Add(pedidoDetail);
            }
            _appDbContext.SaveChanges(); // Persistindo no banco de dados.
        }
    }
}
