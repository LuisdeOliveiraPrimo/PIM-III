namespace PIM_3.Models;

public class PromocaoHistorico
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public string Usuario { get; set; } = "sistema";
    public DateTime Data { get; set; } = DateTime.Now;
    public decimal PrecoAntigo { get; set; }
    public decimal PrecoAplicado { get; set; }
    public string Acao { get; set; } = string.Empty; // "Aplicado", "Removido", "Undo", "Redo"

    public Produto? Produto { get; set; }
}