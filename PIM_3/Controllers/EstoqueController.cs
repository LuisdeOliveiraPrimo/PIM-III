using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIM_3.Data;
using PIM_3.Models;

namespace PIM_3.Controllers;

public class EstoqueController : Controller
{
    private readonly AppDbContext _context;

    public EstoqueController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int pagina = 1, int historicoDias = 7)
    {
        int tamanhoPagina = 15;
        var agora = DateTime.Now;
        var dataLimite = agora.AddDays(7);

        var estoque = await _context.Lotes
            .Include(l => l.Produto)
            .OrderBy(l => l.DataValidade)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();

        ViewBag.Promocoes = await _context.Lotes
            .Include(l => l.Produto)
            .Where(l => l.DataValidade <= dataLimite && l.DataValidade >= agora && l.QuantidadeAtual > 0
                        && (l.Produto.PrecoPromocional == null || l.Produto.PrecoPromocional <= 0))
            .OrderBy(l => l.DataValidade)
            .Take(15)
            .ToListAsync();

        ViewBag.PromocoesAtivas = await _context.Produtos
            .Where(p => p.PrecoPromocional != null && p.PrecoPromocional > 0)
            .ToListAsync();

        ViewBag.Vendas = await _context.Lotes
            .Include(l => l.Produto)
            .Where(l => l.QuantidadeAtual < l.QuantidadeInicial)
            .OrderByDescending(l => l.Id)
            .Take(10)
            .ToListAsync();

        ViewBag.EstoqueBaixo = await _context.Lotes
            .Include(l => l.Produto)
            .Where(l => l.QuantidadeAtual > 0 && l.QuantidadeAtual <= 10)
            .OrderBy(l => l.QuantidadeAtual)
            .ToListAsync();

        ViewBag.LotesVencendo = await _context.Lotes
            .Include(l => l.Produto)
            .Where(l => l.DataValidade <= agora.AddDays(3) && l.DataValidade >= agora && l.QuantidadeAtual > 0)
            .OrderBy(l => l.DataValidade)
            .ToListAsync();

        ViewBag.Locks = await _context.PromocoesLocks
            .Include(l => l.Produto)
            .Where(l => l.ExpiraEm >= agora)
            .ToListAsync();

        var periodo = historicoDias <= 0 ? 30 : (historicoDias > 30 ? 30 : historicoDias);
        ViewBag.HistoricoPromocoes = await _context.PromocoesHistorico
            .Include(h => h.Produto)
            .Where(h => h.Data >= agora.AddDays(-periodo))
            .OrderByDescending(h => h.Data)
            .Take(100)
            .ToListAsync();

        ViewBag.HistoricoDias = periodo;
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
        var usuario = "admin";
        var produto = await _context.Produtos.FindAsync(produtoId);
        if (produto == null) return NotFound();

        var bloqueio = await _context.PromocoesLocks
            .FirstOrDefaultAsync(l => l.ProdutoId == produtoId && l.ExpiraEm >= DateTime.Now);

        if (bloqueio != null && bloqueio.Usuario != usuario)
        {
            return Conflict(new { mensagem = "Produto já está sendo editado em outra sessão." });
        }

        var precoAntigo = produto.PrecoPromocional ?? produto.PrecoVenda;
        produto.PrecoPromocional = novoPreco;

        _context.PromocoesHistorico.Add(new PromocaoHistorico
        {
            ProdutoId = produtoId,
            Usuario = usuario,
            Data = DateTime.Now,
            PrecoAntigo = precoAntigo,
            PrecoAplicado = novoPreco,
            Acao = "Aplicado"
        });

        _context.PromocoesLocks.Add(new PromocaoLock
        {
            ProdutoId = produtoId,
            Usuario = usuario,
            IniciadoEm = DateTime.Now,
            ExpiraEm = DateTime.Now.AddSeconds(30)
        });

        await _context.SaveChangesAsync();

        var lockAtual = await _context.PromocoesLocks
            .FirstOrDefaultAsync(l => l.ProdutoId == produtoId && l.Usuario == usuario);
        if (lockAtual != null)
        {
            _context.PromocoesLocks.Remove(lockAtual);
            await _context.SaveChangesAsync();
        }

        return Ok(new { mensagem = $"Preço de {produto.Nome} atualizado para R$ {novoPreco:N2}" });
    }

    [HttpPost]
    public async Task<IActionResult> RemoverPromocao(int produtoId)
    {
        var usuario = "admin";
        var produto = await _context.Produtos.FindAsync(produtoId);
        if (produto == null) return NotFound();

        var precoAntigo = produto.PrecoPromocional ?? produto.PrecoVenda;
        produto.PrecoPromocional = null;

        _context.PromocoesHistorico.Add(new PromocaoHistorico
        {
            ProdutoId = produtoId,
            Usuario = usuario,
            Data = DateTime.Now,
            PrecoAntigo = precoAntigo,
            PrecoAplicado = produto.PrecoVenda,
            Acao = "Removido"
        });

        await _context.SaveChangesAsync();
        return Ok(new { mensagem = $"Promoção removida de {produto.Nome}" });
    }

    [HttpPost]
    public async Task<IActionResult> UndoPromocao(int historicoId)
    {
        var usuario = "admin";
        var historico = await _context.PromocoesHistorico.FindAsync(historicoId);
        if (historico == null) return NotFound();

        var produto = await _context.Produtos.FindAsync(historico.ProdutoId);
        if (produto == null) return NotFound();

        produto.PrecoPromocional = historico.PrecoAntigo;

        _context.PromocoesHistorico.Add(new PromocaoHistorico
        {
            ProdutoId = historico.ProdutoId,
            Usuario = usuario,
            Data = DateTime.Now,
            PrecoAntigo = historico.PrecoAplicado,
            PrecoAplicado = historico.PrecoAntigo,
            Acao = "Undo"
        });

        await _context.SaveChangesAsync();
        return Ok(new { mensagem = "Undo aplicado" });
    }

    [HttpPost]
    public async Task<IActionResult> RedoPromocao(int historicoId)
    {
        var usuario = "admin";
        var historico = await _context.PromocoesHistorico.FindAsync(historicoId);
        if (historico == null) return NotFound();

        var produto = await _context.Produtos.FindAsync(historico.ProdutoId);
        if (produto == null) return NotFound();

        produto.PrecoPromocional = historico.PrecoAplicado;

        _context.PromocoesHistorico.Add(new PromocaoHistorico
        {
            ProdutoId = historico.ProdutoId,
            Usuario = usuario,
            Data = DateTime.Now,
            PrecoAntigo = historico.PrecoAntigo,
            PrecoAplicado = historico.PrecoAplicado,
            Acao = "Redo"
        });

        await _context.SaveChangesAsync();
        return Ok(new { mensagem = "Redo aplicado" });
    }
}
