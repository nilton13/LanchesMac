using LanchesMac.Context;
using Microsoft.EntityFrameworkCore;

namespace LanchesMac.Models
{
    public class CarrinhoCompra
    {
        private readonly AppDbContext _context;

        public CarrinhoCompra(AppDbContext context)
        {
            _context = context;
        }

        public string CarrinhoCompraId { get; set; }
        public List<CarrinhoCompraItem> CarrinhoCompraItens { get; set; }

        public static CarrinhoCompra GetCarrinho(IServiceProvider services)
        {
            // Definindo uma sessão
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            // obtem um serviço do tipo do nosso contexto
            var context = services.GetService<AppDbContext>();

            // obtem ou gera o Id do Carrinho
            string carrinhoId = session.GetString("CarrinhoId") ?? Guid.NewGuid().ToString();

            //atribui o id do carrinho na Sessão
            session.SetString("CarrinhoId", carrinhoId);

            // retornando o carrinho com o contexto e o Id atribuido ou obtido
            return new CarrinhoCompra(context)
            {
                CarrinhoCompraId = carrinhoId
            };
        }

        public void AdicionarAoCarrinho(Lanche lanche)
        {
            // Verificar se existe um lanche com o Id do lanche que eu to querendo incluir
            var carrinhoCompraItem = _context.CarrinhoCompraItems.SingleOrDefault(
                s => s.Lanche.LancheId == lanche.LancheId &&
                s.CarrinhoCompraId == CarrinhoCompraId);

            // Verificando se o Lance já existe no carrinho
            if(carrinhoCompraItem == null)
            {
                carrinhoCompraItem = new CarrinhoCompraItem
                {
                    CarrinhoCompraId = CarrinhoCompraId,
                    Lanche = lanche,
                    Quantidade = 1
                };
                _context.CarrinhoCompraItems.Add(carrinhoCompraItem);
            }
            else // Se o ite já existir no carrinho
            {
                carrinhoCompraItem.Quantidade++;
            }

            //Persistindo no banco de dados
            _context.SaveChanges();
        }

        public int RemoverDoCarrinho(Lanche lanche)
        {
            // Verificar se existe um lanche com o Id do lanche que eu to querendo incluir
            var carrinhoCompraItem = _context.CarrinhoCompraItems.SingleOrDefault(
                s => s.Lanche.LancheId == lanche.LancheId &&
                s.CarrinhoCompraId == CarrinhoCompraId);

            var quantidadeLocal = 0;

            if(carrinhoCompraItem != null)
            {
                if (carrinhoCompraItem.Quantidade > 1) // se a quantidade > 1 decrementar    
                {
                    carrinhoCompraItem.Quantidade--;
                    quantidadeLocal = carrinhoCompraItem.Quantidade;
                }
                else
                {
                    _context.CarrinhoCompraItems.Remove(carrinhoCompraItem);
                }
            }
            _context.SaveChanges();
            return quantidadeLocal;
            
        }

        public List<CarrinhoCompraItem> GetCarrinhoCompraItems()
        {
            // Retornará uma instancia do carrinho de Compras se Não for igual a Null
            // Se não ele irá no banco e retornará todos os carrinhos com seus Itens
            return CarrinhoCompraItens ??
                (CarrinhoCompraItens =
                    _context.CarrinhoCompraItems
                    .Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
                    .Include(s => s.Lanche)
                    .ToList());
        }
    
        public void LimparCarrinho()
        {
            // Obtendo o carrinho
            var carrinhoItens = _context.CarrinhoCompraItems
                                    .Where(carrinho => carrinho.CarrinhoCompraId == CarrinhoCompraId);

            // Remover todas as entidades recuperadas do carrinho.
            _context.CarrinhoCompraItems.RemoveRange(carrinhoItens);
            _context.SaveChanges();
        }
  
        public decimal GetCarrinhoCompraTotal()
        {
            var total = _context.CarrinhoCompraItems
                .Where(c => c.CarrinhoCompraId == CarrinhoCompraId)
                .Select(c => c.Lanche.preco * c.Quantidade).Sum();

            return total;
        }
    }
}
