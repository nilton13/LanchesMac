using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LanchesMac.Repositories
{
    public class LancheRepository : ILancheRepository
    {
        private readonly AppDbContext _context;
        public LancheRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Lanche> Lanches => _context.Lanches
                .Include(c => c.Categoria); // Adcionando Categoria

        public IEnumerable<Lanche> LanchesPreferidos => _context
                .Lanches.Where(l => l.IsLanchePreferido) // Obtendo todos os Lanches onde Preferido = True
                .Include(c => c.Categoria); // Adcionando Categoria

        public Lanche GetLancheById(int LancheId)
        {
            return _context.Lanches
                .FirstOrDefault(l => l.LancheId == LancheId); //  Recuperando Lanche pelo Id
        }
    }
}
