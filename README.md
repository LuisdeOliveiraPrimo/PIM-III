# PIM III - Sistema de Gestão (C# .NET)

Este projeto é uma aplicação web desenvolvida em **ASP.NET Core (Razor Pages)** para o projeto acadêmico PIM III. A estrutura segue as melhores práticas de separação de responsabilidades (Back-end e Front-end).

## 📂 Estrutura de Pastas e Arquivos

### 1. Pastas de Organização do Código
* **`Data/`**: Responsável pela camada de persistência. Contém as classes que lidam com o banco de dados ou manipulação de arquivos (CSV/Excel).
* **`Models/`**: Contém as classes de modelo (objetos). Define a estrutura dos dados do sistema (ex: `Usuario.cs`, `Funcionario.cs`).
* **`Services/`**: Camada de lógica de negócio. Aqui ficam os cálculos, validações e regras que não devem ficar misturadas com a interface.
* **`Properties/`**: Contém configurações de inicialização do projeto, como a porta onde o servidor roda (`launchSettings.json`).

### 2. Interface e Front-end
* **`Pages/`**: É o coração da interface. Cada página possui dois arquivos:
    * `.cshtml`: O arquivo de marcação (HTML + Razor) que define o visual.
    * `.cshtml.cs`: O "Code-behind" (C#) que processa a lógica específica daquela página (recebe dados, envia para o service, etc).
    * **`Index.cshtml`**: A página inicial do sistema (acessada pela raiz `/`).
    * **`Cadastro.cshtml`**: Exemplo de página de entrada de dados.
* **`wwwroot/`**: Pasta de arquivos estáticos acessíveis pelo navegador:
    * `css/`: Estilos personalizados da aplicação.
    * `img/`: Imagens, logos e ícones.
    * `js/`: Scripts JavaScript para interações no cliente.
    * `lib/`: Bibliotecas externas como Bootstrap e jQuery.

### 3. Arquivos de Configuração da Raiz
* **`Program.cs`**: O ponto de entrada da aplicação. Configura os serviços, injeção de dependência e o pipeline de requisições HTTP.
* **`appsettings.json`**: Arquivo de configuração global. Usado para armazenar strings de conexão com banco de dados e chaves de API.
* **`PIM_3.csproj`**: Arquivo de definição do projeto .NET. Lista todas as dependências (NuGet) e a versão do framework utilizada.

---

## 🚀 Como executar o projeto

1. Certifique-se de ter o **.NET SDK** instalado.
2. Abra o terminal na pasta raiz do projeto (`PIM_3`).
3. Execute o comando:
   ```bash
   dotnet watch