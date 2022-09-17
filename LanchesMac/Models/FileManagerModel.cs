namespace LanchesMac.Models
{
    public class FileManagerModel
    {
        public FileInfo[] Files { get; set; } // Da acesso a métodos e propriedades 
        public IFormFile IFormFile { get; set; } //  Interface que permite enviar os arquivos
        public List<IFormFile> IFormFiles { get; set; } // Lista dos arquivos a serem enviados
        public string PathImagesProduto { get; set; } // Local onde vou armazenar no servidor
    }
}
