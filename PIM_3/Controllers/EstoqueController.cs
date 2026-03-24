using Microsoft.AspNetCore.Mvc;
using PIM_3.Services;

namespace PIM_3.Controllers;

public class EstoqueController : Controller
{
    private readonly IEstoqueService _estoqueService;

    public EstoqueController(IEstoqueService estoqueService)
    {
        _estoqueService = estoqueService;
    }

    public async Task<IActionResult> Index(int pagina = 1)
    {
        int tamanhoPagina = 15;
        var agora = DateTime.Now;
        var dataLimite = agora.AddDays(7);

        var estoque = await _estoqueService.GetPaginatedLotesAsync(pagina, tamanhoPagina);
        ViewBag.Promocoes = await _estoqueService.GetPromocoesAsync(agora, dataLimite);
        ViewBag.Vendas = await _estoqueService.GetVendasAsync();
        ViewBag.PromocoesAtivas = await _estoqueService.GetPromocoesAtivasAsync();
        ViewBag.EstoqueBaixo = await _estoqueService.GetEstoqueBaixoAsync();
        ViewBag.LotesVencendo = await _estoqueService.GetLotesVencendoAsync(agora);
        ViewBag.Locks = await _estoqueService.GetLocksAsync(agora);

        var periodo = 7;
        ViewBag.HistoricoPromocoes = await _estoqueService.GetHistoricoAsync(agora, periodo);


        ViewBag.PaginaAtual = pagina;
        return View(estoque);
    }

    [HttpPost]
    public async Task<IActionResult> AjustarEstoque(int id, int mudanca)
    {
        try
        {
            var novaQuantidade = await _estoqueService.AjustarEstoqueAsync(id, mudanca);
            return Ok(new { novaQuantidade });
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> AplicarPromocao(int produtoId, decimal novoPreco)
    {
        var usuario = "admin";
        var sucesso = await _estoqueService.AplicarPromocaoAsync(produtoId, novoPreco, usuario);

        if (!sucesso)
            return Conflict(new { mensagem = "Produto já está sendo editado em outra sessão ou não foi encontrado." });

        return Ok(new { mensagem = $"Preço alterado para R$ {novoPreco:N2}" });
    }

    [HttpPost]
    public async Task<IActionResult> RemoverPromocao(int produtoId)
    {
        var usuario = "admin";
        var sucesso = await _estoqueService.RemoverPromocaoAsync(produtoId, usuario);
        if (!sucesso) return NotFound();

        return Ok(new { mensagem = "Promoção removida com sucesso" });
    }
}