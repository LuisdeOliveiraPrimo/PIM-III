// Teste de emergência: Isso TEM que rodar primeiro
document.body.style.backgroundColor = "black";
document.body.style.color = "white";

console.log("JavaScript carregado com sucesso!");

window.onload = () => {
    const root = document.getElementById('app-root');
    if (root) {
        root.innerHTML = "<h1 style='padding:50px; text-align:center;'>🚀 CONEXÃO ESTABELECIDA!</h1>";
    } else {
        console.error("ERRO: Não encontrei a div 'app-root'");
    }
};