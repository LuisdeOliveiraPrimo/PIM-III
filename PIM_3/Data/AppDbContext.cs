using Microsoft.EntityFrameworkCore;
using PIM_3.Models;

namespace PIM_3.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Produto> Produtos { get; set; }
}