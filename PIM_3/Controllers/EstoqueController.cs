using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIM_3.Data;

namespace PIM_3.Controllers;

public class EstoqueController : Controller
{
    private readonly AppDbContext _context;

    public EstoqueController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int pagina = 1)
    {
        int tamanhoPagina = 15;
        var dataLimite = DateTime.Now.AddDays(7);

        // 1. Busca os lotes normais (Paginados de 15 em 15)
        var estoque = await _context.Lotes
            .Include(l => l.Produto)
            .OrderBy(l => l.DataValidade)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        // 2. Busca lotes que vencem em até 7 dias (Para a tabela de Promoções)
        ViewBag.Promocoes = await _context.Lotes
            .Include(l => l.Produto)
            .Where(l => l.DataValidade <= dataLimite && l.DataValidade >= DateTime.Now && l.QuantidadeAtual > 0)
            .OrderBy(l => l.DataValidade)
            .Take(15)
            .ToListAsync();

        // 3. Busca Vendas Recentes (Lotes onde a Qtd Atual é menor que a Inicial)
        ViewBag.Vendas = await _context.Lotes
            .Include(l => l.Produto)
            .Where(l => l.QuantidadeAtual < l.QuantidadeInicial)
            .OrderByDescending(l => l.Id)
            .Take(10)
            .ToListAsync();

        ViewBag.PaginaAtual = pagina;
        return View(estoque);
    }

    [HttpPost]
    public async Task<IActionResult> AjustarEstoque(int id, int mudanca)
    {
        var lote = await _context.Lotes.FindAsync(id);
        if (lote == null) return NotFound();

        lote.QuantidadeAtual += mudanca;
        if (lote.QuantidadeAtual < 0) lote.QuantidadeAtual = 0;

        await _context.SaveChangesAsync();
        return Ok(new { novaQuantidade = lote.QuantidadeAtual });
    }

    [HttpPost]
    public async Task<IActionResult> AplicarPromocao(int produtoId, decimal novoPreco)
    {
        var produto = await _context.Produtos.FindAsync(produtoId);
        if (produto == null) return NotFound();

        produto.PrecoPromocional = novoPreco;
        await _context.SaveChangesAsync();

        return Ok(new { mensagem = $"Preço de {produto.Nome} atualizado para R$ {novoPreco:N2}" });
    }
}