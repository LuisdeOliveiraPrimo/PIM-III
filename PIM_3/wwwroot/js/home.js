// home.js - Stoque.me

window.addEventListener('DOMContentLoaded', () => {

    const root = document.getElementById('app-root');
    if (!root) return;

    root.innerHTML = '';

    // ESTILIZAÇÃO DO BODY
    document.body.style.margin = "0";
    document.body.style.fontFamily = "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif";
    document.body.style.backgroundColor = "#ffffff";
    document.body.style.color = "#333333";

    // ==========================================
    // SEÇÃO 1: CABEÇALHO (Com Navegação para Razor Pages)
    // ==========================================
    const header = document.createElement('header');
    header.style.cssText = "display:flex; justify-content:space-between; align-items:center; padding:15px 80px; border-bottom:1px solid #e0e0e0; background:white; position: sticky; top:0; z-index:100;";

    // Logo com Clique para voltar à Home
    const logo = document.createElement('h1');
    logo.innerText = "Stoque.me";
    logo.style.cssText = "font-style:italic; font-weight:800; margin:0; font-size:1.8rem; background:linear-gradient(90deg, #0ABFAD, #045951); -webkit-background-clip:text; background-clip:text; -webkit-text-fill-color:transparent; color:transparent; cursor:pointer;";
    logo.onclick = () => window.location.href = '/';

    const nav = document.createElement('nav');
    const ul = document.createElement('ul');
    ul.style.cssText = "display:flex; list-style:none; margin:0; padding:0; gap:30px; align-items:center;";

    const criarBotaoMenu = (texto, pagina) => {
        const li = document.createElement('li');
        const btn = document.createElement('span'); // Usamos span para estilizar como link

        btn.innerText = texto;
        btn.style.cssText = "cursor:pointer; color:#555555; font-weight:500; font-size:1.1rem; transition: 0.3s;";

        // Efeito Hover
        btn.onmouseover = () => btn.style.color = "#0ABFAD";
        btn.onmouseout = () => btn.style.color = "#555555";

        // REDIRECIONAMENTO FORÇADO
        btn.onclick = () => {
            window.location.href = window.location.origin + '/' + pagina;
        };

        li.appendChild(btn);
        return li;
    };

    ul.appendChild(criarBotaoMenu("Dashboard", "Dashboard"));
    ul.appendChild(criarBotaoMenu("Produtos", "Produtos"));
    ul.appendChild(criarBotaoMenu("Relatórios", "Relatorios"));

    // Botão Sair
    const liSair = document.createElement('li');
    const btnSair = document.createElement('button');
    btnSair.innerText = "Sair";
    btnSair.style.cssText = "background:#dc3545; color:white; border:none; padding:8px 20px; border-radius:20px; cursor:pointer; font-weight:bold; font-size:1rem;";
    btnSair.onclick = () => alert("Saindo do sistema...");

    liSair.appendChild(btnSair);
    ul.appendChild(liSair);

    nav.appendChild(ul);
    header.append(logo, nav);

    // ==========================================
    // SEÇÃO 2: HERO
    // ==========================================
    const hero = document.createElement('section');
    hero.style.cssText = "padding:80px; text-align:center; background:#f8f9fa;";

    const tituloHero = document.createElement('h2');
    tituloHero.innerHTML = "Gestão de Estoque <br/> Intuitiva e Confiável";
    tituloHero.style.cssText = "font-size:3rem; color:#222222; margin:0 0 20px 0; font-weight:800;";

    const subTituloHero = document.createElement('p');
    subTituloHero.innerText = "Acompanhe seus produtos em tempo real e otimize seus processos de forma simples.";
    subTituloHero.style.cssText = "font-size:1.2rem; color:#666666; margin:0 0 40px 0;";

    const btnAcao = document.createElement('button');
    btnAcao.innerText = "Começar Agora";
    btnAcao.style.cssText = "background:#007bff; color:white; padding:15px 40px; border-radius:30px; border:none; cursor:pointer; font-weight:bold; font-size:1.1rem; box-shadow: 0 4px 15px rgba(0,123,255,0.3);";
    btnAcao.onclick = () => window.location.href = '/Produtos';

    hero.append(tituloHero, subTituloHero, btnAcao);

    // ==========================================
    // SEÇÃO 3: CARDS DE RESUMO (KPIs)
    // ==========================================
    const resumo = document.createElement('section');
    resumo.style.cssText = "padding:60px 80px; display:grid; grid-template-columns:repeat(3, 1fr); gap:30px;";

    const criarCardResumo = (titulo, valor, corDestaque, descricao) => {
        const card = document.createElement('div');
        card.style.cssText = "background:white; padding:30px; border-radius:10px; border:1px solid #e0e0e0; box-shadow:0 4px 6px rgba(0,0,0,0.05);";

        const titCard = document.createElement('small');
        titCard.innerText = titulo;
        titCard.style.cssText = "color:#888888; font-weight:bold; text-transform: uppercase; letter-spacing: 1px;";

        const valCard = document.createElement('h3');
        valCard.innerText = valor;
        valCard.style.cssText = `margin:10px 0 5px 0; font-size:2.2rem; color:${corDestaque}; font-weight:800;`;

        const dCard = document.createElement('p');
        dCard.innerText = descricao;
        dCard.style.cssText = "font-size:0.9rem; color:#AAAAAA; margin:0;";

        card.append(titCard, valCard, dCard);
        return card;
    };

    resumo.appendChild(criarCardResumo("Produtos em Estoque", "60.000 mil", "#004085", "Total de itens ativos."));
    resumo.appendChild(criarCardResumo("Valor em Gôndola", "R$ 1.000.000 milões", "#28a745", "Capital investido estimado."));
    resumo.appendChild(criarCardResumo("Alertas Hoje", "15.000 mil", "#ffc107", "Abaixo do estoque mínimo."));

    // ==========================================
    // SEÇÃO 4: SOLUÇÕES (O PRODUTO)
    // ==========================================
    const solucoes = document.createElement('section');
    solucoes.style.padding = "80px";

    const titSolucoes = document.createElement('h3');
    titSolucoes.innerText = "O que o Stoque.me oferece";
    titSolucoes.style.cssText = "text-align:center; font-size:2.2rem; margin:0 0 50px 0; font-weight:800;";
    solucoes.appendChild(titSolucoes);

    const gridSolucoes = document.createElement('div');
    gridSolucoes.style.cssText = "display:grid; grid-template-columns:repeat(3, 1fr); gap:30px;";

    const criarItemSolucao = (titulo, descricao, contemplas) => {
        const item = document.createElement('div');
        item.style.cssText = "padding:30px; border:1px solid #f0f0f0; border-radius:15px; background:#fdfdfd;";

        const tit = document.createElement('h4');
        tit.innerText = titulo;
        tit.style.cssText = "font-size:1.4rem; margin:0 0 10px 0; font-weight:800; background:linear-gradient(90deg, #0ABFAD, #045951); -webkit-background-clip:text; background-clip:text; -webkit-text-fill-color:transparent; color:transparent;";

        const desc = document.createElement('p');
        desc.innerText = descricao;
        desc.style.cssText = "color:#666666; font-size:1rem; margin-bottom:15px; line-height: 1.5;";

        const lista = document.createElement('ul');
        lista.style.cssText = "padding-left:20px; font-size:0.9rem; color:#444444;";
        contemplas.forEach(txt => {
            const li = document.createElement('li');
            li.innerText = txt;
            li.style.marginBottom = "5px";
            lista.appendChild(li);
        });

        item.append(tit, desc, lista);
        return item;
    };

    gridSolucoes.appendChild(criarItemSolucao(
        "Gestão de Estoque",
        "Controle operacional completo com inteligência de validade.",
        ["Controle PVPS", "Reposição de Carga", "Gatilhos de Estoque Baixo", "Promoções Automáticas"]
    ));

    gridSolucoes.appendChild(criarItemSolucao(
        "Visão Analítica",
        "Dados transformados em lucro para sua gestão.",
        ["Cálculo de ROI", "Ranking de Giro", "Faturamento Real", "Relatório de Perdas"]
    ));

    gridSolucoes.appendChild(criarItemSolucao(
        "Gestão de Operação",
        "Segurança total sobre quem movimenta os itens.",
        ["Cadastro de Equipe", "Logs de Movimentação", "Baixa Personalizada", "Controle de Acessos"]
    ));

    solucoes.appendChild(gridSolucoes);

    // ==========================================
    // SEÇÃO 5: ÚLTIMAS MOVIMENTAÇÕES (Feed)
    // ==========================================
    const movimentacoes = document.createElement('section');
    movimentacoes.style.cssText = "padding:40px 80px; backgroundColor:#fdfdfd;";

    const titMov = document.createElement('h4');
    titMov.innerText = "⚡ Atividade Recente";
    titMov.style.marginBottom = "20px";

    const listaMov = document.createElement('div');
    const logs = [
        "✅ João Silva cadastrou 50un de 'Arroz Tio João'",
        "🛒 Venda: 2x 'Coca-Cola 2L' - Total R$ 24,00",
        "⚠️ Alerta: Estoque de 'Leite' atingiu nível mínimo"
    ];

    logs.forEach(log => {
        const p = document.createElement('p');
        p.innerText = log;
        p.style.cssText = "padding:10px; border-left:4px solid #0ABFAD; background:white; margin-bottom:10px; box-shadow:0 2px 4px rgba(0,0,0,0.02); font-size:0.9rem;";
        listaMov.appendChild(p);
    });
    movimentacoes.append(titMov, listaMov);

    // ==========================================
    // SEÇÃO 6: EXTRAS & FAQ
    // ==========================================
    const extras = document.createElement('section');
    extras.style.cssText = "padding:60px 80px; display:grid; grid-template-columns:1fr 1fr; gap:50px;";

    const mobileBox = document.createElement('div');
    mobileBox.style.cssText = "background:linear-gradient(135deg, #0ABFAD, #045951); color:white; padding:40px; border-radius:20px;";
    mobileBox.innerHTML = `<h3>📱 Stoque.me Mobile</h3><p>Controle seu mercado pelo celular.</p>`;

    const faqBox = document.createElement('div');
    faqBox.innerHTML = `<h4>FAQ</h4><details><summary>Como o ROI é calculado?</summary><p>Custo vs Venda Real.</p></details>`;

    extras.append(mobileBox, faqBox);

    // ==========================================
    // SEÇÃO 7: RODAPÉ
    // ==========================================
    const footer = document.createElement('footer');
    footer.style.cssText = "padding:40px 80px; background:#222; color:#888; textAlign:center; fontSize:0.8rem;";
    footer.innerHTML = `<p><b>Stoque.me</b> - PIM III | Unip 2026</p>`;

    // MONTAGEM FINAL
    root.append(header, hero, resumo, solucoes, movimentacoes, extras, footer);
});