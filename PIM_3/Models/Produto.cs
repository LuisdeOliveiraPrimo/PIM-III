namespace PIM_3.Models
{
    public class Produto
    {
        public int Id { get; set; } // O SQLite cria esse número sozinho
        public string Nome { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int QuantidadeEstoque { get; set; }
    }
}