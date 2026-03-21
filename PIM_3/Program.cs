using Microsoft.EntityFrameworkCore;
using PIM_3.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Adiciona suporte às Páginas Razor
builder.Services.AddRazorPages();

// 2. CONFIGURAÇÃO DO SQLITE
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=PimBanco.db"));

var app = builder.Build();

// Configurações de ambiente
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// ESSENCIAL: Garante que o navegador ache a pasta wwwroot/js
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// Simplificando o mapeamento para evitar erros de assets
app.MapRazorPages();

app.Run();