using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanchesMac.Models
{
    [Table("Lanches")]
    public class Lanche
    {
        [Key]
        public int LancheId { get; set; }
        [StringLength(100, ErrorMessage = "O tamanho máximo é 100 caracteres.")]
        [Required(ErrorMessage = "Informe o nome do Lanche")]
        [Display(Name = "Nome do Lanche")]
        public string Name { get; set; }
        public string DescricaoCurta { get; set; }
        public string DescricaoDetalhada { get; set; }
        [Required(ErrorMessage = "Informe o preço do Lanche")]
        [Display(Name= "Preço")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(1,999.99, ErrorMessage ="O preço deve estar entre 1 e 999,99")]
        public decimal preco { get; set; }
        public string ImagemUrl { get; set; }
        public string ImagemThumbnailUrl { get; set; }
        public bool IsLanchePreferido { get; set; }
        public bool EmEstoque { get; set; }
        // Criando relacionamento
        public int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }

    }
}
