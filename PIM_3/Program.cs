using Microsoft.EntityFrameworkCore;
using PIM_3.Data;
using PIM_3.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurações de Serviços
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=PimBanco.db"));

var app = builder.Build();

// 2. Configurações de Middleware (Ordem importa!)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 3. BLOCO DE SEED DATA - DADOS REAIS E PERSONALIZADOS
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    // GERAR 15.000 PRODUTOS REAIS
    if (!context.Produtos.Any())
    {
        Console.WriteLine("🌱 Gerando 15.000 produtos reais... aguarde.");

        var catalogo = new Dictionary<string, string[]>
        {
            { "Bebidas", new[] { "Coca-Cola 2L", "Pepsi 1.5L", "Suco Del Valle Uva", "Água Mineral 500ml", "Cerveja Heineken 350ml", "Red Bull 250ml" } },
            { "Laticínios", new[] { "Leite Integral Ninho", "Iogurte Grego Danone", "Queijo Muçarela fatiado", "Manteiga Aviação 200g", "Requeijão Nestlé" } },
            { "Mercearia", new[] { "Arroz Tio João 5kg", "Feijão Camil 1kg", "Macarrão Barilla Penne", "Óleo de Soja Liza", "Açúcar União 1kg" } },
            { "Limpeza", new[] { "Detergente Ypê", "Amaciante Downy", "Sabão em Pó Omo 1kg", "Desinfetante Veja", "Água Sanitária Qboa" } },
            { "Hortifruti", new[] { "Banana Nanica kg", "Maçã Gala kg", "Tomate Italiano kg", "Alface Americana un", "Batata Inglesa kg" } }
        };

        var random = new Random();
        var novosProdutos = new List<Produto>();
        var categoriasKeys = catalogo.Keys.ToArray();

        for (int i = 1; i <= 15000; i++)
        {
            var categoria = categoriasKeys[random.Next(categoriasKeys.Length)];
            var nomeBase = catalogo[categoria][random.Next(catalogo[categoria].Length)];

            novosProdutos.Add(new Produto
            {
                Nome = $"{nomeBase} (SKU-{i:D5})",
                Classificacao = categoria,
                PrecoVenda = (decimal)(random.NextDouble() * (48) + 2)
            });
        }
        context.Produtos.AddRange(novosProdutos);
        context.SaveChanges();
    }

    // GERAR 3.000 LOTES REAIS
    if (!context.Lotes.Any())
    {
        Console.WriteLine("📦 Gerando 3.000 lotes com validades variadas...");
        var random = new Random();
        var novosLotes = new List<Lote>();
        var idsProdutos = context.Produtos.Select(p => p.Id).ToList();

        for (int i = 1; i <= 3000; i++)
        {
            int produtoId = idsProdutos[random.Next(idsProdutos.Count)];
            // 15% nascem vencendo/vencidos para testar sua tabela de promoções
            int diasValidade = random.Next(100) < 15 ? random.Next(-5, 6) : random.Next(10, 200);

            novosLotes.Add(new Lote
            {
                ProdutoId = produtoId,
                QuantidadeInicial = 100,
                QuantidadeAtual = random.Next(5, 80),
                DataRecebimento = DateTime.Now.AddDays(-random.Next(1, 10)),
                DataValidade = DateTime.Now.AddDays(diasValidade),
                CodigoLote = $"LT-{random.Next(1000, 9999)}-{i}"
            });
        }
        context.Lotes.AddRange(novosLotes);
        context.SaveChanges();
        Console.WriteLine("✅ Banco de dados populado com Coca-Cola e muito mais!");
    }

    // GERAR 100 FUNCIONÁRIOS
    if (!context.Funcionarios.Any())
    {
        var funcs = new List<Funcionario>();
        for (int i = 1; i <= 100; i++)
        {
            funcs.Add(new Funcionario { Nome = $"Funcionario {i}", Email = $"func{i}@stoque.me", SenhaHash = "123", Ativo = true });
        }
        context.Funcionarios.AddRange(funcs);
        context.SaveChanges();
    }
}

app.Run();