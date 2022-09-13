using LanchesMac.Models;

namespace LanchesMac.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        //Irá retornar uma lista de Categorias
        IEnumerable<Categoria> Categorias { get; }  
    }
}
