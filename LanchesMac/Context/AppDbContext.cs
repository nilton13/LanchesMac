using LanchesMac.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LanchesMac.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        //Carrega as informações necessária para configuração do DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {                
        }

        // Define as Tabelas a serem criadas
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Lanche> Lanches { get; set; }
        public DbSet<CarrinhoCompraItem> CarrinhoCompraItems { get; set; }  

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalhe> PedidoDetalhes { get; set; }
    }

}
