using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PIM_3.Pages
{
    public class CadastroModel : PageModel
    {
        // Esta variável guarda o que o HTML vai exibir
        public string MensagemDeRetorno { get; set; } = string.Empty;

        // Quando você abre a página pela primeira vez
        public void OnGet()
        {
            MensagemDeRetorno = "Aguardando preenchimento...";
        }

        // Quando você clica no botão "Enviar"
        public void OnPost(string NomeUsuario)
        {
            if (string.IsNullOrEmpty(NomeUsuario))
            {
                MensagemDeRetorno = "Por favor, digite um nome!";
            }
            else
            {
                MensagemDeRetorno = $"Sucesso! O usuário {NomeUsuario} foi enviado ao servidor.";
            }
        }
    }
}