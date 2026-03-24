using PIM_3.Models;

namespace PIM_3.Services;

public interface IEstoqueService
{
    Task<List<Lote>> GetPaginatedLotesAsync(int pagina, int tamanhoPagina);
    Task<List<Lote>> GetPromocoesAsync(DateTime agora, DateTime dataLimite);
    Task<List<Produto>> GetPromocoesAtivasAsync();
    Task<List<Lote>> GetVendasAsync();
    Task<List<Lote>> GetEstoqueBaixoAsync();
    Task<List<Lote>> GetLotesVencendoAsync(DateTime agora);
    Task<List<PromocaoLock>> GetLocksAsync(DateTime agora);
    Task<List<PromocaoHistorico>> GetHistoricoAsync(DateTime agora, int periodo);

    Task<decimal> AjustarEstoqueAsync(int id, int mudanca);
    Task<bool> AplicarPromocaoAsync(int produtoId, decimal novoPreco, string usuario);
    Task<bool> RemoverPromocaoAsync(int produtoId, string usuario);
    Task<bool> UndoPromocaoAsync(int historicoId, string usuario);
    Task<bool> RedoPromocaoAsync(int historicoId, string usuario);
}
