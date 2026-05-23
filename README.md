# PIM III — Stoque.me

Projeto Integrado Multidisciplinar III: sistema web para gestão de inventário e vendas voltado a pequenos comércios, supermercados de bairro e lojas de conveniência.

O sistema, denominado **Stoque.me**, foi desenvolvido em C# com ASP.NET Core (Razor Pages) e usa SQLite via Entity Framework Core para persistência. O objetivo é reduzir perdas por vencimento, automatizar o controle de estoque e fornecer dashboards e relatórios para apoiar decisões operacionais.

---

## 📌 Visão geral

- Problema alvo: controle manual de estoque, desperdício por vencimento, baixa visibilidade de indicadores e dificuldades operacionais.
- Solução: aplicação web com módulos de inventário, dashboard analítico, relatórios (PDF/XLSX/CSV), controle operacional e promoções automáticas PVPS (Primeiro que Vence, Primeiro que Sai).

## 📁 Principais pastas

- `Data/` — contexto do banco e configurações de persistência.
- `Models/` — classes de domínio (Produto, Lote, Venda, Fornecedor, etc.).
- `Services/` — regras de negócio e operações sobre os dados.
- `Pages/` — páginas Razor (UI). Cada página tem `.cshtml` e `.cshtml.cs`.
- `wwwroot/` — recursos estáticos (css, js, lib, imagens).
- `Program.cs` & `appsettings.*` — boot e configuração da aplicação.

---

## 🚀 Requisitos e execução

Pré-requisitos:
- `.NET 8.0` SDK (ou versão compatível do SDK usada no projeto).
- (Opcional) Editor: Visual Studio, VS Code.

Executando localmente:

1. Abra um terminal na pasta raiz do projeto (`PIM_3`).
2. Restaure e compile:

```bash
dotnet restore
dotnet build
```

3. Inicie em modo de desenvolvimento (hot-reload):

```bash
dotnet watch run
```

4. Abra no navegador o URL indicado no console (ex.: `https://localhost:5001` ou o que aparecer em `launchSettings.json`).

Notas úteis:
- Se houver problema com certificado de desenvolvimento (HTTPS), execute:

```bash
dotnet dev-certs https --trust
```

- Se alguma porta estiver ocupada por execução anterior, mate o processo ou use um OutputPath alternativo para compilar sem sobrescrever o binário em execução.

---

## 🛠️ Funcionalidades (resumo)

- Cadastro e gerenciamento de produtos, lotes e fornecedores.
- Controle de estoque com rastreabilidade por lote.
- Dashboard com KPIs e gráficos de vendas/estoque.
- Geração e download de relatórios (PDF, XLSX, CSV).
- Promoções automáticas PVPS para reduzir perdas por vencimento.
- Logs e histórico de operações por usuário.
- Interface responsiva e acessível (desktop, tablet, mobile).

---

## 💡 Observações técnicas

- Persistência: SQLite com Entity Framework Core (migrations podem ser usadas para evoluir o schema).
- Padrões: arquitetura em camadas, princípios de OOP e injeção de dependência.
- Front-end: Razor Pages com CSS/JS customizados; os recursos estáticos ficam em `wwwroot/`.

---

## 🔎 Troubleshooting (erros comuns)

- Erro de WebSocket do browser-refresh (ex.: `WebSocket connection to 'wss://localhost:59309/' failed`): verifique se o servidor está rodando, se a porta corresponde ao `launchSettings.json` e se o certificado HTTPS está confiável. Use `dotnet dev-certs https --trust` e reinicie com `dotnet watch run`.
- Erro de formatação no editor (`The changes must not overlap.`): conflito entre formatadores. Há uma configuração recomendada em `/.vscode/settings.json` (o projeto já contém ajustes para evitar formatações concorrentes).

---

## 🧪 Testes rápidos

- Teste a geração de relatórios clicando em *Relatórios* → *Download PDF/XLSX/CSV*.
- No DevTools do navegador, ative o modo responsivo para validar o menu hamburger e o drawer em telas pequenas.

---

## 📚 Referências e conceitos abordados

- Programação orientada a objetos (C#)
- Desenvolvimento web com ASP.NET Core Razor Pages
- ORM: Entity Framework Core com SQLite
- UX/UI, responsividade e acessibilidade
- Engenharia de software ágil e análise de dados

---

## Resumo do projeto

O presente Projeto Integrado Multidisciplinar III tem como objetivo o desenvolvimento de um sistema web para gestão de inventário e vendas voltado para pequenos comércios, supermercados de bairro e lojas de conveniência. O projeto foi desenvolvido utilizando a linguagem C# com o framework ASP.NET Core Razor Pages, integrado ao banco de dados SQLite por meio do Entity Framework Core.

O sistema denominado “Stoque.me” busca solucionar problemas recorrentes relacionados ao controle manual de estoque, desperdício de produtos, perdas financeiras por vencimento e dificuldades no acompanhamento de vendas e movimentações internas.

A aplicação possui módulos de inventário, dashboard analítico, relatórios internos, controle operacional, monitoramento de estoque e sistema de promoções automáticas utilizando o conceito PVPS (Primeiro que Vence, Primeiro que sai). Além disso, o sistema foi desenvolvido considerando conceitos de usabilidade, acessibilidade, responsividade e organização em camadas.

O projeto também aborda conceitos relacionados à programação orientada a objetos, banco de dados relacional, UX/UI Design, engenharia de software ágil e análise de dados.

Palavras-chave: Gestão de Estoque. ASP.NET Core. SQLite. C#. Dashboard. Inventário.

---

## ✍️ Contribuição

Contribuições são bem-vindas: abra issues para bugs ou features e envie pull requests com descrições claras das mudanças.

---

## 📝 Licença

Este projeto pertence a o grupo de ads da UNIP.
