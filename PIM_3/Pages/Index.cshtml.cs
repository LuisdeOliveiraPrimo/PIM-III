using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PIM_3.Pages
{
    public class IndexModel : PageModel
    {
        public string MensagemBoasVindas { get; set; } = string.Empty;

        public void OnGet()
        {
            MensagemBoasVindas = "Bem-vindo ao Sistema PIM III!";
        }
    }
}