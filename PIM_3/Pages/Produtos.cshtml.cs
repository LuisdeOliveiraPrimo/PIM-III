using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PIM_3.Data;
using PIM_3.Models;

namespace PIM_3.Pages
{
    public class ProdutosModel : PageModel
    {
        private readonly AppDbContext _context;

        public ProdutosModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Produto> ListaProdutos { get; set; } = default!;

        // Executa ao carregar a página: busca tudo no banco
        public async Task OnGetAsync()
        {
            ListaProdutos = await _context.Produtos.ToListAsync();
        }

        // Executa ao clicar no botão: salva um produto de teste
        public async Task<IActionResult> OnPostAddTesteAsync()
        {
            var novo = new Produto
            {
                Nome = "Produto Teste " + Guid.NewGuid().ToString().Substring(0, 4),
                Categoria = "Geral",
                Preco = 10.50m,
                QuantidadeEstoque = 10
            };

            _context.Produtos.Add(novo);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}