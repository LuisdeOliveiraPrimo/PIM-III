using Microsoft.EntityFrameworkCore;
using PIM_3.Data;
using PIM_3.Models;

namespace PIM_3.Services;

public class EstoqueService : IEstoqueService
{
    private readonly AppDbContext _context;

    public EstoqueService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Lote>> GetPaginatedLotesAsync(int pagina, int tamanhoPagina)
    {
        return await _context.Lotes
            .Include(l => l.Produto)
            .OrderBy(l => l.DataValidade)
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .ToListAsync();
    }

    public async Task<List<Lote>> GetPromocoesAsync(DateTime agora, DateTime dataLimite) =>
        await _context.Lotes
            .Include(l => l.Produto)
            .Where(l => l.DataValidade <= dataLimite && l.DataValidade >= agora && l.QuantidadeAtual > 0
                        && (l.Produto.PrecoPromocional == null || l.Produto.PrecoPromocional <= 0))
            .OrderBy(l => l.DataValidade)
            .Take(15)
            .ToListAsync();

    public async Task<List<Produto>> GetPromocoesAtivasAsync() =>
        await _context.Produtos
            .Where(p => p.PrecoPromocional != null && p.PrecoPromocional > 0)
            .ToListAsync();

    public async Task<List<Lote>> GetVendasAsync() =>
        await _context.Lotes
            .Include(l => l.Produto)
            .Where(l => l.QuantidadeAtual < l.QuantidadeInicial)
            .OrderByDescending(l => l.Id)
            .Take(10)
            .ToListAsync();

    public async Task<List<Lote>> GetEstoqueBaixoAsync() =>
        await _context.Lotes
            .Include(l => l.Produto)
            .Where(l => l.QuantidadeAtual > 0 && l.QuantidadeAtual <= 10)
            .OrderBy(l => l.QuantidadeAtual)
            .ToListAsync();

    public async Task<List<Lote>> GetLotesVencendoAsync(DateTime agora) =>
        await _context.Lotes
            .Include(l => l.Produto)
            .Where(l => l.DataValidade <= agora.AddDays(3) && l.DataValidade >= agora && l.QuantidadeAtual > 0)
            .OrderBy(l => l.DataValidade)
            .ToListAsync();

    public async Task<List<PromocaoLock>> GetLocksAsync(DateTime agora) =>
        await _context.PromocoesLocks
            .Include(l => l.Produto)
            .Where(l => l.ExpiraEm >= agora)
            .ToListAsync();

    public async Task<List<PromocaoHistorico>> GetHistoricoAsync(DateTime agora, int periodo) =>
        await _context.PromocoesHistorico
            .Include(h => h.Produto)
            .Where(h => h.Data >= agora.AddDays(-periodo))
            .OrderByDescending(h => h.Data)
            .Take(100)
            .ToListAsync();

    public async Task<decimal> AjustarEstoqueAsync(int id, int mudanca)
    {
        var lote = await _context.Lotes.FindAsync(id);
        if (lote == null) throw new InvalidOperationException("Lote não encontrado.");

        lote.QuantidadeAtual += mudanca;
        if (lote.QuantidadeAtual < 0) lote.QuantidadeAtual = 0;

        await _context.SaveChangesAsync();
        return lote.QuantidadeAtual;
    }

    public async Task<bool> AplicarPromocaoAsync(int produtoId, decimal novoPreco, string usuario)
    {
        var produto = await _context.Produtos.FindAsync(produtoId);
        if (produto == null) return false;

        var bloqueio = await _context.PromocoesLocks
            .FirstOrDefaultAsync(l => l.ProdutoId == produtoId && l.ExpiraEm >= DateTime.Now);

        if (bloqueio != null && bloqueio.Usuario != usuario)
            return false;

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

        return true;
    }

    public async Task<bool> RemoverPromocaoAsync(int produtoId, string usuario)
    {
        var produto = await _context.Produtos.FindAsync(produtoId);
        if (produto == null) return false;

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
        return true;
    }

    public async Task<bool> UndoPromocaoAsync(int historicoId, string usuario)
    {
        var historico = await _context.PromocoesHistorico.FindAsync(historicoId);
        if (historico == null) return false;

        var produto = await _context.Produtos.FindAsync(historico.ProdutoId);
        if (produto == null) return false;

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
        return true;
    }

    public async Task<bool> RedoPromocaoAsync(int historicoId, string usuario)
    {
        var historico = await _context.PromocoesHistorico.FindAsync(historicoId);
        if (historico == null) return false;

        var produto = await _context.Produtos.FindAsync(historico.ProdutoId);
        if (produto == null) return false;

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
        return true;
    }
}
