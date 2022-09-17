using LanchesMac.Context;
using LanchesMac.Models;
using Microsoft.EntityFrameworkCore;

namespace LanchesMac.Areas.Admin.Services
{
    public class RelatorioVendasServices
    {
        private readonly AppDbContext _context;

        public RelatorioVendasServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pedido>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var resultado = from obj in _context.Pedidos select obj;

            if (minDate.HasValue)
            {
                resultado = resultado.Where(x => x.PedidoEnviado >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                resultado = resultado.Where(x => x.PedidoEnviado >= maxDate.Value);
            }

            return await resultado
                            .Include(l => l.PedidoItens) // Incluindo os itens de pedido
                            .ThenInclude(l => l.Lanche) // Incluindo os Lanches
                            .OrderByDescending(x => x.PedidoEnviado) // Ordenando por data
                            .ToListAsync();
        }
    }
}
