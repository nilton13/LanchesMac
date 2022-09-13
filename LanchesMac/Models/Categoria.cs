using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanchesMac.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        // Quando criamos um atributo com o nome Id o EntityFramework entende que 
        // o atributo é  uma Primary Key.
        [Key]
        public int CategoriaId { get; set; }
        [StringLength(100, ErrorMessage ="O tamanho máximo é 100 caracteres.")]
        [Required(ErrorMessage = "Informe o nome da categoria")]
        [Display(Name = "Nome")]
        public string CategoriaNome { get; set; }
        [StringLength(200, ErrorMessage = "O tamanho máximo é 100 caracteres.")]
        [Required(ErrorMessage = "Informe a descrição da categoria")]
        [Display(Name = "Nome")]
        public string Descricao { get; set; }
        // 1 categoria terá Vários Lanches
        public List<Lanche> Lanches { get; set; }
    }
}
