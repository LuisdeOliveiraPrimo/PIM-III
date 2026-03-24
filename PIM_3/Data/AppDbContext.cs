using Microsoft.EntityFrameworkCore;
using PIM_3.Models;

namespace PIM_3.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<PromocaoHistorico> PromocoesHistorico { get; set; }
        public DbSet<PromocaoLock> PromocoesLocks { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; } // NOVA TABELA
        public DbSet<Funcionario> Funcionarios { get; set; }
    }
}