using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using LanchesMac.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class LancheController : Controller
    {
        private readonly ILancheRepository _lancheRepository; // Criando uma Instância de LancheRepository.

        //Recuperando o LancheRepositorio utilizando Injeção de dependência.
        public LancheController(ILancheRepository lancheRepository)
        {
            _lancheRepository = lancheRepository;
        }

        public IActionResult List(string categoria)
        {
            IEnumerable<Lanche> lanches;
            string categoriaAtual = string.Empty;

            if (string.IsNullOrEmpty(categoria))
            {
                lanches = _lancheRepository.Lanches.OrderBy(l => l.LancheId); // Ordernando por ID.
                categoriaAtual = "Todos os Lanches";
            }
            else
            {

                // Verificando se a categoria informada é igual a Normal
                //if(string.Equals("Normal", categoria, StringComparison.OrdinalIgnoreCase)) // Compara os Textos em Caixa Alta ou caixa Baixa
                //{
                //    lanches = _lancheRepository.Lanches
                //        .Where(l => l.Categoria.CategoriaNome.Equals("Normal"))
                //        .OrderBy(l => l.Name);
                //}
                //else
                //{
                //    lanches = _lancheRepository.Lanches
                //        .Where(l => l.Categoria.CategoriaNome.Equals("Natural"))
                //        .OrderBy(l => l.Name);
                //}

                lanches = _lancheRepository.Lanches
                        .Where(l => l.Categoria.CategoriaNome.Equals(categoria))
                        .OrderBy(c => c.Name);
                categoriaAtual = categoria;
            }

            // Instanciando um novo LancheListViewModel com o que foi recuperado.
            var lancheListViewModel = new LancheListViewModel
            {
                Lanches = lanches,
                CategoriaAtual = categoriaAtual
            };

            return View(lancheListViewModel);
         }

        public IActionResult Details(int LancheId)
        {
            var lanche = _lancheRepository.Lanches.FirstOrDefault(l => l.LancheId == LancheId);
            return View(lanche);
        }

        public ViewResult Search(string searchString)
        {
            IEnumerable<Lanche> lanches;
            string categoriaAtual = string.Empty;

            if (string.IsNullOrEmpty(searchString)) // Verificando se a string recebida esta vazia.
            {
                lanches = _lancheRepository.Lanches.OrderBy(p => p.LancheId);
                categoriaAtual = "Todos os Lanches";
            }
            else
            {   // Filtrando os Lanches pelo nome digitado
                lanches = _lancheRepository.Lanches
                            .Where(p => p.Name.ToLower().Contains(searchString.ToLower()));

                if (lanches.Any())
                {
                    categoriaAtual = "Lanches";
                }
                else
                {
                    categoriaAtual = "Nenhum lanche foi encontrado";
                }
            }

            //Retornando o ListViewModel atualizado
            return View("~/Views/Lanche/List.cshtml", new LancheListViewModel
            {
                Lanches = lanches,
                CategoriaAtual = categoriaAtual
            });
        }
    }
}
