using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;

namespace LanchesMac.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context; // Instancia do Banco de Dados

        //Injeção de Dependências do Banco de Dados
        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        //Retornando todas as categorias da tabela Categorias
        public IEnumerable<Categoria> Categorias => _context.Categorias;
    }
}
