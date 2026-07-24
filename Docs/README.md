<!-- Glyph Markup (v1.6). Decoder: ./GLYPH_LEGEND.md. Código/tabelas em [OFF]...[ON]. -->

[SECTION: readme]
[NT[`README.md — Photino.Blazor Custom Template (híbrido UI + API)`]]
[INS[rd `./GLYPH_LEGEND.md` antes de parsear este arquivo]]

# Photino.Blazor Custom Template

[RSN[`template pessoal para apps desktop híbridos: UI Photino.Blazor + backend ASP.NET Core Web API, com geração de código assistida por IA`]]

---

## Stack

[OFF]
| Camada        | Componente        | Tecnologia                                         |
|---------------|-------------------|----------------------------------------------------|
| UI (desktop)  | Janela nativa     | Photino.NET 4.x                                     |
| UI (desktop)  | Web UI            | Blazor (Microsoft.AspNetCore.Components.WebView)    |
| UI (desktop)  | Estilo            | Tailwind CSS v4 (@tailwindcss/cli)                  |
| UI (desktop)  | Frameworks        | net8.0 / net9.0                                     |
| API (backend) | Web framework     | ASP.NET Core Web API                               |
| API (backend) | ORM               | Entity Framework Core                               |
| API (backend) | Banco             | SQL Server | PostgreSQL | MySQL                     |
| API (backend) | Docs              | Swagger UI (Swashbuckle)                            |
| API (backend) | Testes            | xUnit + EF Core InMemory                            |
| Ambos         | Geração de código | Claude Code                                         |
[ON]

[NT[`a UI consome a API via HttpClient; as duas camadas coexistem no mesmo template`]]

---

## Projetos-base neste repo (a "base" — não alterar, só incrementar)

[OFF]
| Projeto     | Caminho                        | Tipo             | Frameworks   | Papel                                    |
|-------------|--------------------------------|------------------|--------------|------------------------------------------|
| ViewEngine  | ViewEngine/ViewEngine.csproj   | Library          | net8.0;net9.0| Motor Blazor-sobre-Photino (Photino.NET) |
| ViewCore    | ViewCore/ViewCore.csproj       | WinExe (Razor)   | net8.0       | Host de exemplo; UI Blazor + Tailwind v4 |
[ON]

[MAND[`o código de ViewEngine/ViewCore é a base — incremente ao redor, não reescreva`]]
[REF[`detalhe de build/estilo: AGENTS.md (raiz)`]]

---

## Como usar este template

[BLOCK: uso]
[INS[1. criar repo a partir do template (GitHub → "Use this template" → "Create a new repository")]]
[WARN[`não use Fork — o template repository cria um repo limpo, sem histórico`]]
[INS[2. clonar e abrir]]
[OFF]
git clone https://github.com/[PH: org]/[PH: nome-do-projeto]
cd [PH: nome-do-projeto]
[ON]
[INS[3. preencher os docs ANTES de acionar a IA — ordem em `[BLOCK: reading-order]` abaixo]]
[INS[4. gerar/incrementar com Claude Code — o `CLAUDE.md` é lido automaticamente]]
[INS[5. rodar a UI e/ou a API — ver `[SECTION: rodar]`]]
[/BLOCK]

---

## Ordem de leitura dos docs (canônica)

[BLOCK: reading-order]
[REF[`1. GLYPH_LEGEND.md — decoder deste conjunto`]]
[REF[`2. ARCHITECTURE.md — camadas, padrões e convenções (UI + API)`]]
[REF[`3. DATA_MODEL.md — entidades, tipos e banco (camada API)`]]
[REF[`4. BUSINESS_RULES.md — regras de negócio e campos calculados`]]
[REF[`5. API_ENDPOINTS.md — endpoints da API e como a UI os consome`]]
[REF[`6. DEPENDENCIES.md — pacotes NuGet + toolchain de UI (Tailwind)`]]
[REF[`7. TESTS.md — testes de Service (API) e de componente (UI)`]]
[REF[`8. DOCKER.md — dockerização do backend (a UI desktop não é containerizada)`]]
[REF[`9. DECISIONS.md — ADRs (não contrariar sem novo ADR)`]]
[REF[`10. CHANGELOG.md — histórico (atualizar após mudanças)`]]
[/BLOCK]

---

## Prompt de geração para a IA

[OFF]
Leia GLYPH_LEGEND.md e depois todos os arquivos em ./Docs/ na ordem canônica.
Gere/incremente:
  - a camada UI (Photino.Blazor) conforme ARCHITECTURE.md §UI, sem alterar ViewEngine/ViewCore;
  - a camada API conforme ARCHITECTURE.md §API, DATA_MODEL.md, BUSINESS_RULES.md e API_ENDPOINTS.md.
Respeite as convenções e não contrarie DECISIONS.md.
[ON]

---

## Rodar

[SECTION: rodar]

### UI (Photino.Blazor desktop)
[OFF]
# hot-reload do CSS (terminal separado)
cd ViewCore && npx @tailwindcss/cli -i ./wwwroot/imports.css -o ./wwwroot/index.css --watch

# rodar o app desktop
dotnet run --project ViewCore
[ON]

### API (ASP.NET Core)
[OFF]
cd [PH: projeto-api]
dotnet restore
dotnet run          # Swagger em https://localhost:{porta}/swagger
dotnet test         # testes unitários
[ON]
[/SECTION]

---

## Estrutura dos docs

[OFF]
Docs/
├── GLYPH_LEGEND.md    ← decoder do conjunto Glyph
├── README.md          ← este arquivo (visão geral do template)
├── ARCHITECTURE.md    ← padrões e convenções (UI + API)
├── DATA_MODEL.md      ← entidades, tipos e banco (PREENCHER)
├── BUSINESS_RULES.md  ← regras de negócio e campos calculados (PREENCHER)
├── API_ENDPOINTS.md   ← endpoints e consumo pela UI (PREENCHER)
├── DEPENDENCIES.md    ← pacotes NuGet + toolchain de UI
├── TESTS.md           ← testes de Service e de componente
├── DOCKER.md          ← dockerização do backend
├── DECISIONS.md       ← ADRs
└── CHANGELOG.md       ← histórico de versões
[ON]

---

## Checklist de novo projeto

[BLOCK: checklist-novo-projeto]
[INS[repo criado via "Use this template" (não fork)]]
[INS[identidade ajustada: renomear solution/projeto, `PackageId`, URLs (ver AGENTS.md §Notes)]]
[INS[Docs preenchidos: DATA_MODEL, BUSINESS_RULES, API_ENDPOINTS (todos os `[PH:]` resolvidos)]]
[INS[banco escolhido em DATA_MODEL.md → provider confirmado em DEPENDENCIES.md]]
[INS[Claude Code executado → camada API gerada; clientes/telas da UI incrementados (base ViewEngine/ViewCore intacta)]]
[INS[appsettings.Development.json criado com credenciais locais (não versionado)]]
[INS[API: `dotnet run` → Swagger abre; `dotnet test` verde]]
[INS[UI: `dotnet run --project ViewCore` + watch do Tailwind → janela nativa abre]]
[INS[Docker (se API em container): `.env` a partir de `.env.example`; `docker-compose up`]]
[INS[CHANGELOG.md [Unreleased] iniciado; primeiro commit realizado]]
[/BLOCK]

---

## Regra de ouro

[PRIO[`docs antes de código — sempre; regra de negócio muda em BUSINESS_RULES.md primeiro, depois teste (red), depois Service (green), depois CHANGELOG.md`]]
[DONT[`alterar ViewEngine/ViewCore (a base); contrariar DECISIONS.md; commitar credenciais`]]

[VRFY[coerência + regras Glyph]]
[/SECTION]
