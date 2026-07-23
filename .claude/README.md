# dataholds-dotnet-api-template

Template padrão da Dataholds para APIs REST em .NET com arquitetura em camadas, testes unitários e geração assistida por IA.

---

## Stack

| Componente | Tecnologia |
|---|---|
| Plataforma | .NET 10 / C# |
| Framework web | ASP.NET Core Web API |
| ORM | Entity Framework Core |
| Banco de dados | SQL Server |
| Documentação | Swagger UI (Swashbuckle) |
| Testes | xUnit + EF Core InMemory |
| Geração de código | Claude Code |

---

## Como usar este template

### 1. Criar repositório a partir do template

Clica em **"Use this template"** no GitHub → **"Create a new repository"**.

> ⚠️ Não use Fork — o template repository cria um repositório limpo sem histórico de commits.

---

### 2. Clonar e abrir o projeto

```bash
git clone https://github.com/dataholds/{nome-do-seu-projeto}
cd {nome-do-seu-projeto}
```

---

### 3. Preencher os docs (FAÇA ANTES de abrir o Claude Code)

Edite os arquivos na pasta `docs/` na seguinte ordem:

#### 3.1 `docs/DATA_MODEL.md`
Descreva as tabelas do banco de dados:
- Nome das tabelas e schema
- Colunas, tipos SQL Server e tipos C# correspondentes
- Chaves primárias e campos anuláveis

#### 3.2 `docs/BUSINESS_RULES.md`
Descreva as regras de negócio:
- Campos calculados com fórmulas
- Casos de nulo e casos de borda
- Dados de teste representativos para cada cenário

#### 3.3 `docs/API_ENDPOINTS.md`
Descreva os endpoints:
- Rotas e verbos HTTP
- Parâmetros de query e rota
- Exemplos de resposta JSON

#### 3.4 `docs/DEPENDENCIES.md` *(raramente precisa alterar)*
Revise se há pacotes adicionais necessários para o projeto.

#### 3.5 `docs/TESTS.md` *(raramente precisa alterar)*
Revise se há categorias de teste específicas do domínio.

---

### 4. Gerar o projeto com Claude Code

Abre o Claude Code na raiz do repositório:

```bash
claude
```

O arquivo `CLAUDE.md` é lido automaticamente. Em seguida, envie o prompt:

```
Leia todos os arquivos em docs/ e gere o projeto completo
seguindo a arquitetura e convenções definidas em ARCHITECTURE.md.
```

O Claude Code irá:
1. Ler a arquitetura e convenções
2. Instalar os pacotes NuGet necessários
3. Gerar as entities baseadas no DATA_MODEL.md
4. Gerar os ViewModels com os campos do BUSINESS_RULES.md
5. Gerar o AppDbContext
6. Gerar os Services com as regras de negócio
7. Gerar os Controllers
8. Configurar o Program.cs e appsettings.json
9. Gerar os testes unitários

---

### 5. Configurar as credenciais

Crie o arquivo `appsettings.Development.json` na raiz do projeto (não versionado):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server={SERVIDOR};Database={BANCO};User Id={USUARIO};Password={SENHA};TrustServerCertificate=True;"
  }
}
```

> Este arquivo já está no `.gitignore` — nunca commite credenciais.

---

### 6. Rodar o projeto

```bash
# Restaurar dependências
dotnet restore

# Rodar a API
dotnet run

# Acessar o Swagger
# https://localhost:{porta}/swagger
```

---

### 7. Rodar os testes

```bash
# Todos os testes
dotnet test

# Com detalhes
dotnet test --verbosity normal

# Teste específico
dotnet test --filter "NomeDoTeste"
```

---

## Arquitetura gerada

```
{NomeProjeto}/
├── Controllers/
│   └── {Entidade}Controller.cs       ← recebe HTTP, delega ao Service
├── Data/
│   └── AppDbContext.cs               ← mapeamento EF Core
├── Models/
│   ├── {Entidade}.cs                 ← entity — espelho da tabela
│   └── {Entidade}ViewModel.cs        ← contrato da API
├── Services/
│   ├── I{Entidade}Service.cs         ← interface (contrato)
│   └── {Entidade}Service.cs          ← regras de negócio
├── Tests/
│   └── {Entidade}ServiceTests.cs     ← testes unitários
├── appsettings.json                  ← configuração (sem credenciais)
├── appsettings.Development.json      ← credenciais locais (não versionado)
└── Program.cs                        ← DI e pipeline
```

### Responsabilidade de cada camada

```
Request HTTP
    ↓
Controller    →  recebe, valida parâmetros, devolve resposta
    ↓
Service       →  regras de negócio, campos calculados, transformações
    ↓
AppDbContext  →  acesso ao banco via EF Core
    ↓
SQL Server
```

**Regras obrigatórias:**
- Controller não acessa banco diretamente
- Controller não contém regra de negócio
- ViewModel declara campos calculados, Service os calcula
- Todo campo calculado tem teste unitário

---

## Princípios adotados

| Princípio | Como aparece no código |
|---|---|
| Single Responsibility | Cada classe tem uma responsabilidade — Controller faz HTTP, Service faz negócio |
| Dependency Inversion | Controller depende de `IService`, não de `Service` |
| Injeção de Dependência | Registrado em `Program.cs`, injetado via construtor |
| Inversão de Controle | ASP.NET Core cria e gerencia os objetos |
| Testabilidade | Service testado com banco em memória, sem SQL Server real |

---

## Estrutura dos docs

```
docs/
├── ARCHITECTURE.md     ← padrões, convenções e exemplos de código
├── BUSINESS_RULES.md   ← regras de negócio e dados de teste (PREENCHER)
├── DATA_MODEL.md       ← entidades, tipos e banco de dados (PREENCHER)
├── API_ENDPOINTS.md    ← endpoints, parâmetros e respostas (PREENCHER)
├── DEPENDENCIES.md     ← pacotes NuGet por banco de dados
├── TESTS.md            ← categorias e padrão de testes
└── DOCKER.md           ← Dockerfile, docker-compose e variáveis de ambiente
```

---

## Bancos de dados suportados

O template suporta três providers — escolha no `DATA_MODEL.md`:

| Banco | Provider NuGet | Uso típico |
|---|---|---|
| SQL Server | `Microsoft.EntityFrameworkCore.SqlServer` | On-prem, Azure |
| PostgreSQL | `Npgsql.EntityFrameworkCore.PostgreSQL` | GCP Cloud SQL, Supabase, Neon |
| MySQL | `Pomelo.EntityFrameworkCore.MySql` | GCP Cloud SQL, PlanetScale |

A troca de banco é cirúrgica — muda o pacote e a connection string. O restante do código não muda.

---

## Docker

O template inclui suporte completo a Docker para desenvolvimento e produção.

### Subir com Docker Compose

```bash
# Copiar template de variáveis
cp .env.example .env
# Editar .env com as credenciais

# Subir API + banco
docker-compose up

# Acessar Swagger
# http://localhost:8080/swagger
```

### Comandos úteis

```bash
# Subir em background
docker-compose up -d

# Ver logs
docker-compose logs -f api

# Rebuild após mudança no código
docker-compose up --build

# Derrubar tudo
docker-compose down

# Resetar banco (apaga volumes)
docker-compose down -v
```

### Conectar a banco externo (GCP, Azure, on-prem)

Basta apontar a variável de ambiente — sem subir container de banco:

```bash
# .env
DB_CONNECTION_STRING=Host=34.xxx.xxx.xxx;Database=producao;Username=api;Password=...
```

Consulte `docs/DOCKER.md` para configuração completa por banco.

---

## Checklist de novo projeto

```
[ ] Repositório criado a partir do template (não fork)
[ ] docs/DATA_MODEL.md preenchido — banco escolhido, tabelas e tipos
[ ] docs/BUSINESS_RULES.md preenchido com fórmulas e casos de borda
[ ] docs/API_ENDPOINTS.md preenchido com rotas e exemplos
[ ] Claude Code executado e projeto gerado
[ ] appsettings.Development.json criado com credenciais locais
[ ] dotnet run — API subiu e Swagger abre
[ ] dotnet test — todos os testes verdes
[ ] .env criado a partir do .env.example
[ ] docker-compose up — API sobe em localhost:8080/swagger
[ ] Primeiro commit realizado
```

---

## Gerenciando mudanças de regra de negócio

### A regra de ouro

```
Docs antes de código — sempre.
```

Nenhuma mudança de regra de negócio vai direto para o código. O fluxo é:

```
BUSINESS_RULES.md atualizado
        ↓
Teste atualizado (falha — red)
        ↓
Service atualizado (passa — green)
        ↓
CHANGELOG.md atualizado
        ↓
Pull Request
```

### Durante o desenvolvimento

Entendimento refinado, requisito mudou, cliente pediu ajuste:

1. Atualiza `docs/BUSINESS_RULES.md` com a fórmula correta
2. Atualiza o teste correspondente — vai falhar intencionalmente
3. Atualiza o Service para o teste passar
4. Os testes existentes avisam se algo foi quebrado sem querer
5. Abre PR com docs + teste + código juntos

### Após produção — novo projeto de inclusão de regras

1. Abre issue descrevendo a mudança em linguagem de negócio
2. Atualiza `docs/BUSINESS_RULES.md` — registra fórmula anterior e nova
3. Cria ADR em `docs/DECISIONS.md` se a decisão não for óbvia
4. Segue o mesmo fluxo de desenvolvimento acima
5. Atualiza `docs/CHANGELOG.md` com versão e data

### Usando o Claude Code para mudanças pontuais

Não use "gere o projeto" para mudanças — use prompts cirúrgicos:

```
O BUSINESS_RULES.md foi atualizado com a seguinte mudança:
GrossMargin agora desconta o campo Discount antes de calcular.
Fórmula anterior: SalesAmount - TotalCost
Fórmula atual:   (SalesAmount - Discount) - TotalCost

Atualize apenas:
- o teste em FactSalesServiceTests.cs cobrindo o novo cenário
- o método GetAll em FactSalesService.cs

Não toque em Controllers, Models, ViewModels ou outros arquivos.
```

---

## Estrutura dos docs

```
docs/
├── ARCHITECTURE.md     ← padrões, convenções e exemplos de código
├── BUSINESS_RULES.md   ← regras de negócio, fórmulas e processo de mudança
├── DATA_MODEL.md       ← entidades, tipos e banco de dados (PREENCHER)
├── API_ENDPOINTS.md    ← endpoints, parâmetros e respostas (PREENCHER)
├── DEPENDENCIES.md     ← pacotes NuGet por banco de dados
├── TESTS.md            ← categorias e padrão de testes
├── DOCKER.md           ← Dockerfile, docker-compose e variáveis de ambiente
├── CHANGELOG.md        ← histórico de versões legível pelo negócio
└── DECISIONS.md        ← registro de decisões arquiteturais (ADR)
```

---

## Contribuindo com o template

Melhorias no template são bem-vindas. Antes de abrir um PR:

- A mudança se aplica a **todos** os projetos? Se for específica de um domínio, não pertence ao template.
- Testes continuam passando?
- A documentação em `docs/` reflete a mudança?

---

## Dataholds

[dataholds.com.br](https://dataholds.com.br)

Projetos de análise de dados, BI e pipelines de dados.