using Microsoft.EntityFrameworkCore;
using PIM_3.Data; // Se o nome do seu projeto for PIM_3

var builder = WebApplication.CreateBuilder(args);

// 1. Adiciona suporte às Páginas Razor (O que já estava aí)
builder.Services.AddRazorPages();

// 2. CONFIGURAÇÃO DO SQLITE (Adicione estas linhas)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=PimBanco.db"));

var app = builder.Build();

// Configurações de ambiente (O que já estava aí)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Garante que o seu home.js seja lido!

app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();