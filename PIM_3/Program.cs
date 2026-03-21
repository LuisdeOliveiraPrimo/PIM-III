using Microsoft.EntityFrameworkCore;
using PIM_3.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. ADICIONE AS LINHAS ABAIXO (Configuração do Banco)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=PimBanco.db"));

// 2. Add services to the container.
builder.Services.AddRazorPages();

// CORREÇÃO: Certifique-se de que está escrito "builder.Build()" completo
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();