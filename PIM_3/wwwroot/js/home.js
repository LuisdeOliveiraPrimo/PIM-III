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
    // SEÇÃO 1: CABEÇALHO
    // ==========================================
    const header = document.createElement('header');
    header.style.display = "flex";
    header.style.justifyContent = "space-between";
    header.style.alignItems = "center";
    header.style.padding = "15px 80px";
    header.style.borderBottom = "1px solid #e0e0e0";
    header.style.backgroundColor = "white";

    const logo = document.createElement('h1');
    logo.innerText = "Stoque.me";
    logo.style.fontStyle = "italic";
    logo.style.fontWeight = "800";
    logo.style.margin = "0";
    logo.style.fontSize = "1.8rem";
    logo.style.background = "linear-gradient(90deg, #0ABFAD, #045951)";
    logo.style.webkitBackgroundClip = "text";
    logo.style.backgroundClip = "text";
    logo.style.webkitTextFillColor = "transparent";
    logo.style.color = "transparent";

    const nav = document.createElement('nav');
    const ul = document.createElement('ul');
    ul.style.display = "flex";
    ul.style.listStyle = "none";
    ul.style.margin = "0";
    ul.style.padding = "0";
    ul.style.gap = "30px";
    ul.style.alignItems = "center";

    const criarLink = (texto) => {
        const li = document.createElement('li');
        const a = document.createElement('a');
        a.innerText = texto;
        a.href = "#";
        a.style.textDecoration = "none";
        a.style.color = "#555555";
        a.style.fontWeight = "500";
        a.style.fontSize = "1.1rem";
        li.appendChild(a);
        return li;
    };

    ul.appendChild(criarLink("Dashboard"));
    ul.appendChild(criarLink("Produtos"));
    ul.appendChild(criarLink("Relatórios"));

    const liSair = document.createElement('li');
    const btnSair = document.createElement('button');
    btnSair.innerText = "Sair";
    btnSair.style.background = "#dc3545";
    btnSair.style.color = "white";
    btnSair.style.border = "none";
    btnSair.style.padding = "8px 20px";
    btnSair.style.borderRadius = "20px";
    btnSair.style.cursor = "pointer";
    btnSair.style.fontSize = "1.0rem";
    btnSair.style.fontWeight = "bold";
    liSair.appendChild(btnSair);
    ul.appendChild(liSair);

    nav.appendChild(ul);
    header.appendChild(logo);
    header.appendChild(nav);

    // ==========================================
    // SEÇÃO 2: HERO
    // ==========================================
    const hero = document.createElement('section');
    hero.style.padding = "80px";
    hero.style.textAlign = "center";
    hero.style.backgroundColor = "#f8f9fa";

    const tituloHero = document.createElement('h2');
    tituloHero.innerHTML = "Gestão de Estoque <br/> Intuitiva e Confiável";
    tituloHero.style.fontSize = "3rem";
    tituloHero.style.color = "#222222";
    tituloHero.style.margin = "0 0 20px 0";
    tituloHero.style.fontWeight = "800";

    const subTituloHero = document.createElement('p');
    subTituloHero.innerText = "Acompanhe seus produtos em tempo real e otimize seus processos de forma simples.";
    subTituloHero.style.fontSize = "1.2rem";
    subTituloHero.style.color = "#666666";
    subTituloHero.style.margin = "0 0 40px 0";

    const btnAcao = document.createElement('button');
    btnAcao.innerText = "Começar Agora";
    btnAcao.style.background = "#007bff";
    btnAcao.style.color = "white";
    btnAcao.style.padding = "15px 40px";
    btnAcao.style.borderRadius = "30px";
    btnAcao.style.border = "none";
    btnAcao.style.cursor = "pointer";
    btnAcao.style.fontWeight = "bold";

    hero.appendChild(tituloHero);
    hero.appendChild(subTituloHero);
    hero.appendChild(btnAcao);

    // ==========================================
    // SEÇÃO 3: CARDS DE RESUMO (KPIs)
    // ==========================================
    const resumo = document.createElement('section');
    resumo.style.padding = "60px 80px";
    resumo.style.display = "grid";
    resumo.style.gridTemplateColumns = "repeat(3, 1fr)";
    resumo.style.gap = "30px";

    const criarCardResumo = (titulo, valor, corDestaque, descricao) => {
        const card = document.createElement('div');
        card.style.background = "white";
        card.style.padding = "30px";
        card.style.borderRadius = "10px";
        card.style.border = "1px solid #e0e0e0";
        card.style.boxShadow = "0 4px 6px rgba(0,0,0,0.05)";

        const titCard = document.createElement('small');
        titCard.innerText = titulo;
        titCard.style.color = "#888888";
        titCard.style.fontWeight = "bold";

        const valCard = document.createElement('h3');
        valCard.innerText = valor;
        valCard.style.margin = "10px 0 5px 0";
        valCard.style.fontSize = "2.2rem";
        valCard.style.color = corDestaque;

        const dCard = document.createElement('p');
        dCard.innerText = descricao;
        dCard.style.fontSize = "0.9rem";
        dCard.style.color = "#AAAAAA";

        card.appendChild(titCard);
        card.appendChild(valCard);
        card.appendChild(dCard);
        return card;
    };

    resumo.appendChild(criarCardResumo("Produtos Cadastrados no Estoque", "60.000 mil", "#004085", "Total de itens ativos."));
    resumo.appendChild(criarCardResumo("Valor em Gôndola", "R$ 1.000.000 milão", "#28a745", "Capital investido."));
    resumo.appendChild(criarCardResumo("Alertas Hoje", "15.000 mil", "#ffc107", "Abaixo do estoque mínimo."));

    // ==========================================
    // SEÇÃO 4: NOSSAS SOLUÇÕES (O PRODUTO)
    // ==========================================
    const solucoes = document.createElement('section');
    solucoes.style.padding = "80px";

    const titSolucoes = document.createElement('h3');
    titSolucoes.innerText = "O que o Stoque.me oferece";
    titSolucoes.style.textAlign = "center";
    titSolucoes.style.fontSize = "2.2rem";
    titSolucoes.style.margin = "0 0 50px 0";
    solucoes.appendChild(titSolucoes);

    const gridSolucoes = document.createElement('div');
    gridSolucoes.style.display = "grid";
    gridSolucoes.style.gridTemplateColumns = "repeat(3, 1fr)";
    gridSolucoes.style.gap = "30px";

    const criarItemSolucao = (titulo, descricao, contemplas) => {
        const item = document.createElement('div');
        item.style.padding = "30px";
        item.style.border = "1px solid #f0f0f0";
        item.style.borderRadius = "15px";
        item.style.backgroundColor = "#fdfdfd";

        const tit = document.createElement('h4');
        tit.innerText = titulo;
        tit.style.fontSize = "1.4rem";
        tit.style.margin = "0 0 10px 0";
        tit.style.background = "linear-gradient(90deg, #0ABFAD, #045951)";
        tit.style.webkitBackgroundClip = "text";
        tit.style.backgroundClip = "text";
        tit.style.webkitTextFillColor = "transparent";

        const desc = document.createElement('p');
        desc.innerText = descricao;
        desc.style.color = "#666666";
        desc.style.fontSize = "1rem";
        desc.style.marginBottom = "15px";

        const lista = document.createElement('ul');
        lista.style.paddingLeft = "20px";
        lista.style.fontSize = "0.9rem";
        contemplas.forEach(txt => {
            const li = document.createElement('li');
            li.innerText = txt;
            li.style.marginBottom = "5px";
            lista.appendChild(li);
        });

        item.appendChild(tit);
        item.appendChild(desc);
        item.appendChild(lista);
        return item;
    };

    gridSolucoes.appendChild(criarItemSolucao(
        "Gestão de Estoque",
        "Controle operacional completo da sua gôndola até o estoque reserva.",
        ["Controle de Validade (PVPS)", "Controle de Carga (Reposição)", "Estoque em Tempo Real", "Gatilhos de Estoque Baixo"]
    ));

    gridSolucoes.appendChild(criarItemSolucao(
        "Relatório de Desempenho",
        "Transforme dados em decisões para aumentar a lucratividade do seu mercado.",
        ["Cálculo de ROI", "Ranking de Giro", "Faturamento por Produto", "Relatório de Perdas Mensais"]
    ));

    gridSolucoes.appendChild(criarItemSolucao(
        "Gestão de Operação",
        "Segurança e controle sobre quem opera o sistema e movimenta os itens.",
        ["Cadastro de Funcionários", "Histórico de Movimentação (Logs)", "Baixa de Venda Personalizada", "Controle de Acessos"]
    ));

    solucoes.appendChild(gridSolucoes);

    // ==========================================
    // SEÇÃO 5: ÚLTIMAS MOVIMENTAÇÕES (Feed)
    // ==========================================
    const movimentacoes = document.createElement('section');
    movimentacoes.style.padding = "40px 80px";
    movimentacoes.style.backgroundColor = "#fdfdfd";

    const titMov = document.createElement('h4');
    titMov.innerText = "⚡ Atividade Recente";
    titMov.style.marginBottom = "20px";
    movimentacoes.appendChild(titMov);

    const listaMov = document.createElement('div');
    listaMov.style.fontSize = "0.9rem";
    listaMov.style.color = "#555";

    // Simulação de Logs (Depois buscaremos do Banco)
    const logs = [
        "✅ João Silva (Gerente) cadastrou 50un de 'Arroz Tio João'",
        "🛒 Venda realizada: 2x 'Coca-Cola 2L' - Total R$ 24,00",
        "⚠️ Alerta: Estoque de 'Leite Integral' atingiu o nível mínimo"
    ];

    logs.forEach(log => {
        const p = document.createElement('p');
        p.innerText = log;
        p.style.padding = "10px";
        p.style.borderLeft = "4px solid #0ABFAD";
        p.style.backgroundColor = "white";
        p.style.marginBottom = "10px";
        p.style.boxShadow = "0 2px 4px rgba(0,0,0,0.02)";
        listaMov.appendChild(p);
    });
    movimentacoes.appendChild(listaMov);

    // ==========================================
    // SEÇÃO 6: ACESSO MOBILE & FAQ
    // ==========================================
    const extras = document.createElement('section');
    extras.style.padding = "60px 80px";
    extras.style.display = "grid";
    extras.style.gridTemplateColumns = "1fr 1fr";
    extras.style.gap = "50px";

    // Bloco Mobile
    const mobileBox = document.createElement('div');
    mobileBox.style.background = "linear-gradient(135deg, #0ABFAD, #045951)";
    mobileBox.style.color = "white";
    mobileBox.style.padding = "40px";
    mobileBox.style.borderRadius = "20px";
    mobileBox.innerHTML = `
        <h3>📱 Stoque.me no seu bolso</h3>
        <p>Acesse o painel do seu mercado pelo tablet ou celular. Controle total, onde você estiver.</p>
        <small>Compatível com Android e iOS</small>
    `;

    // Bloco FAQ
    const faqBox = document.createElement('div');
    faqBox.innerHTML = `
        <h4 style="margin-bottom:15px">Dúvidas Frequentes</h4>
        <details style="margin-bottom:10px; cursor:pointer">
            <summary><b>Como o ROI é calculado?</b></summary>
            <p style="font-size:0.9rem; color:#666">Baseado no custo de entrada vs. valor real das vendas concluídas.</p>
        </details>
        <details style="cursor:pointer">
            <summary><b>O alerta de validade é automático?</b></summary>
            <p style="font-size:0.9rem; color:#666">Sim, o sistema destaca itens vencendo em até 15 dias na cor laranja.</p>
        </details>
    `;

    extras.appendChild(mobileBox);
    extras.appendChild(faqBox);

    // ==========================================
    // SEÇÃO 7: RODAPÉ (Footer)
    // ==========================================
    const footer = document.createElement('footer');
    footer.style.padding = "40px 80px";
    footer.style.backgroundColor = "#222";
    footer.style.color = "#888";
    footer.style.textAlign = "center";
    footer.style.fontSize = "0.8rem";
    footer.innerHTML = `
        <p><b>Stoque.me</b> - Sistema de Gestão Inteligente</p>
        <p>Desenvolvido para o Projeto PIM III | Unip 2026</p>
        <div style="margin-top:15px">
            <a href="#" style="color:#888; margin:0 10px; text-decoration:none">Suporte</a> | 
            <a href="#" style="color:#888; margin:0 10px; text-decoration:none">Privacidade</a>
        </div>
    `;

    // ==========================================
    // FINALIZAÇÃO: ADICIONA TUDO AO ROOT
    // ==========================================
    root.appendChild(header);
    root.appendChild(hero);
    root.appendChild(resumo);
    root.appendChild(solucoes);
    root.appendChild(movimentacoes);
    root.appendChild(extras);
    root.appendChild(footer);
});


