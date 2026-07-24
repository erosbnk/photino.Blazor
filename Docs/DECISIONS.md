<!-- Glyph Markup (v1.6). Decoder: ./GLYPH_LEGEND.md. Blocos verbatim em [OFF]...[ON]. -->

[SECTION: decisions]
[NT[`DECISIONS.md — Registro de Decisões Arquiteturais (ADR)`]]
[INS[rd `./GLYPH_LEGEND.md` antes de parsear este arquivo]]
[MAND[`não contrariar um ADR "Aceito" sem criar um novo ADR que o substitua`]]

# DECISIONS.md — ADRs

[OFF]
O que é um ADR? Architecture Decision Record — registro curto de uma decisão
arquitetural, o contexto que levou a ela e suas consequências.

Status: Proposta | Aceita | Depreciada | Substituída por ADR-XXX
[ON]

---

## ADR-001 — Template híbrido: UI Photino.Blazor + API ASP.NET Core
[INS[Data: `2026-07-24` — Status: `Aceita`]]
[BLOCK: adr1]
[NT[Contexto]]
[OFF]
O repo base é Photino.Blazor (desktop). Os docs herdados descreviam uma API REST
.NET. Em vez de escolher um lado, o template une os dois: um front desktop
Photino.Blazor consumindo um backend ASP.NET Core Web API.
[ON]
[NT[Decisão]]
[INS[o template mantém DUAS camadas: UI (Photino.Blazor) + API (ASP.NET Core), integradas por HttpClient]]
[NT[Consequências]]
[OFF]
+ cobre apps desktop que precisam de backend/BD sem reescrever
+ cada camada evolui e testa isoladamente
− dois runtimes para orquestrar em dev; contrato (ViewModel) precisa de disciplina
[ON]
[/BLOCK]

---

## ADR-002 — Tailwind CSS v4 com sintaxe nativa; watch fora do build
[INS[Data: `2026-07-24` — Status: `Aceita`]]
[BLOCK: adr2]
[NT[Contexto]]
[OFF]
ViewCore usava sintaxe v3 (@tailwind base/components/utilities) num projeto v4,
e o target MSBuild rodava o CLI com --watch (travava o build). Classes soltas no
.razor não eram geradas por falta de scan dos componentes.
[ON]
[NT[Decisão]]
[INS[entrada em v4: `@import "tailwindcss";` + `@source "../**/*.razor";`; `@apply` e classes de componente no imports.css global]]
[INS[build roda o CLI uma vez (sem `--watch`); watch só em terminal separado no dev]]
[NT[Consequências]]
[OFF]
+ classes usadas no markup passam a ser geradas; build não trava
+ .razor.css volta a ser CSS isolation puro (sem Tailwind)
− dev precisa lembrar de rodar o watch à parte para hot-reload de CSS
[ON]
[/BLOCK]

---

## ADR-003 — Docs auxiliares em Glyph nativo, dentro de ./Docs/
[INS[Data: `2026-07-24` — Status: `Aceita`]]
[BLOCK: adr3]
[NT[Contexto]]
[OFF]
Os docs são briefing para geração de código por IA. Markdown solto é ambíguo e
sem escopo. A IDE não precisa desses docs; só a IA os consome (via CLAUDE.md →
AGENTS.md).
[ON]
[NT[Decisão]]
[INS[todos os `.md` de `./Docs/` (exceto GLYPH_LEGEND.md) são escritos em Glyph Markup v1.6; código/tabelas em `[OFF]...[ON]`; `GLYPH_LEGEND.md` é o decoder]]
[NT[Consequências]]
[OFF]
+ instrução densa, sem ambiguidade, com escopo explícito para a IA
+ CLAUDE.md/AGENTS.md permanecem Markdown normal (a IDE os lê)
− exige o decoder; humano precisa da legenda para ler confortavelmente
[ON]
[/BLOCK]

---

## ADR-004 — Não alterar a base (ViewEngine/ViewCore); só incrementar
[INS[Data: `2026-07-24` — Status: `Aceita`]]
[BLOCK: adr4]
[NT[Contexto]]
[OFF]
ViewEngine (lib) + ViewCore (host) são a fundação Photino.Blazor. Reescrevê-los
arriscaria a base estável do template.
[ON]
[NT[Decisão]]
[INS[código de ViewEngine/ViewCore é imutável no scaffold do template; features novas entram como projetos/arquivos NOVOS ao redor]]
[NT[Consequências]]
[OFF]
+ base previsível; upgrades do Photino não colidem com customizações
− customização mais profunda pode exigir, no futuro, um ADR que revise isto
[ON]
[/BLOCK]

---

## ADR-005 — Regra de negócio no Service; EF Core InMemory nos testes
[INS[Data: `2026-07-24` — Status: `Aceita` (herdado do template API)]]
[BLOCK: adr5]
[NT[Decisão]]
[INS[campos calculados/regras vivem no Service (nunca no Controller ou ViewModel); testes usam `UseInMemoryDatabase`, sem banco real]]
[NT[Consequências]]
[OFF]
+ Service testável isoladamente; testes em milissegundos, sem infra
− InMemory não replica collation/tipos/procs do banco real; casos complexos
  exigem teste de integração (fora do template por ora)
[ON]
[/BLOCK]

---

## ADR-006 — TFMs divergentes por camada, desacoplados por HTTP
[INS[Data: `2026-07-24` — Status: `Aceita`]]
[BLOCK: adr6]
[NT[Contexto]]
[OFF]
Objetivo: manter a API sempre no latest do .NET. Mas a UI depende de Photino.NET +
Microsoft.AspNetCore.Components.WebView, cujo teto atual é net9.0. Um TFM único
travaria a API no ritmo do Photino.
[ON]
[NT[Decisão]]
[INS[API em `net10.0`; UI (ViewEngine/ViewCore) em `net8.0;net9.0`; Contracts compartilhado (se existir) em `netstandard2.0`]]
[INS[camadas se comunicam só por HTTP (HttpClient) — sem project reference cruzando a fronteira de versão]]
[INS[Photino aos poucos: ViewEngine é multi-target; acrescentar `net10.0` quando Photino/WebView suportarem, sem largar net8]]
[NT[Consequências]]
[OFF]
+ API anda no latest independente do Photino; UI estável até o Photino alcançar
+ multi-target permite validar net10 na UI sem quebrar net8
− regra de project reference: consumidor precisa TFM ≥ lib; net8 (UI) não
  referencia lib net10 → o Contracts nunca mira net10 (netstandard2.0 ou menor
  TFM comum)
[ON]
[/BLOCK]

---

## Quando criar um novo ADR
[BLOCK: quando]
[INS[decisão técnica importante e não óbvia para quem chega depois]]
[INS[alternativa considerada e descartada — registre o porquê]]
[INS[regra de negócio ambígua onde uma escolha foi feita]]
[INS[um padrão do template foi quebrado intencionalmente neste projeto]]
[DONT[criar ADR para: trivialidades, nomenclatura, formatação]]
[/BLOCK]

[VRFY[coerência + regras Glyph]]
[/SECTION]
