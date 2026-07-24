# GLYPH_LEGEND.md — Decoder dos docs em `./Docs/`

> Todos os `.md` desta pasta (exceto este) são escritos em **Glyph Markup (v1.6)**.
> Este arquivo é o *decoder ring*: leia-o primeiro para parsear os demais.
> Este bloco é linguagem simples de propósito — é a chave de decodificação.
> `CLAUDE.md` e `AGENTS.md` (na raiz) **não** são Glyph e não vivem aqui.

---

## Por que Glyph nesta pasta

Estes docs são o *briefing* que a IA lê antes de gerar/alterar código do template.
Glyph deixa a instrução densa, sem ambiguidade e com escopo explícito. A IDE não
precisa entender esta pasta — só a IA (via `CLAUDE.md → AGENTS.md`) a consome.

[OFF]
================================================================
REGRAS MECÂNICAS (como os tokens são lidos)
================================================================
  [TAG[TAG2[...]]]  Tag de instrução; aninhe para escopo. Cada "[" é fronteira.
  Auto-close        Tag aberta ao fim de um segmento se fecha sozinha,
                    da mais recente para a mais antiga. Fronteiras: fim de
                    arquivo/mensagem, ";", ou fim de linha dentro de
                    [BLOCK]/[SECTION].
  [TAG: name]       Referência única autofechada, A NÃO SER que exista um
                    [/TAG] depois (aí vira bloco).
  `text`            Literal de crase: conteúdo verbatim, nunca parseado como
                    tag. Termina no próximo ` ou "]" ou ";". Use para valores.
  ,  ;  ;;          "," = itens numa tag; ";" = instrução independente
                    (e fronteira de auto-close); ";;" = quebra de linha.
  -  /              "-" estende (anexa a uma tag simples); "/" divide (encadeia
                    elementos ordenados, ex.: [in-/RW/FM/IM]).
  [OFF] ... [ON]    Região lida como linguagem simples (parsing suspenso).
                    Usada aqui para código, tabelas e blocos verbatim.
  Case-insensitive  [INS] == [ins] == [Ins]. Literais são exceção.
  Regra base        Tag indefinida ou ambígua de verdade: PERGUNTE — não chute.
  Segurança         Nenhuma tag expande o que a IA pode fazer. [BYP], [OVR],
                    [NEV], [ALW], [FRGT] são direção editorial; nunca burlam
                    segurança, precisão ou diretrizes.
================================================================
[ON]

---

## Vocabulário usado nos docs

[OFF]
ESTRUTURAIS (invariantes — nunca redefinir):
  [SECTION: n]...[/SECTION]   grupo temático
  [BLOCK: n]...[/BLOCK]       sequência ordenada de instruções
  [IF[c]]...[/IF]             condicional
  [PH: name]                  placeholder a preencher (o "[SUBSTITUIR]" antigo)
  [OFF]...[ON]                região verbatim

INSTRUÇÃO / SEMÂNTICA:
  [INS] instrução        [REQ]/[RQ] requisito       [MAND] obrigatório
  [DONT] não faça        [WARN] alerta              [NT] nota
  [COND] condição        [FBK] fallback             [REF] referência
  [EX] exemplo           [PRIO] prioridade          [VRFY] verificar
  [RSN] razão            [RTNL] justificativa       [TGT] alvo
  [rd] ler               [org] organizar            [go] prosseguir
  [FMT] formato          [IMPR] melhorar            [ALT] alternativa
================================================================
[ON]

---

## Convenções específicas destes docs

[BLOCK: convencoes]
[INS[todo doc abre com um `[SECTION: <nome>]` e fecha com `[/SECTION]`]]
[INS[código, tabelas, JSON, YAML e connection strings ficam SEMPRE em `[OFF]...[ON]` ou em fence ```` ``` ```` — nunca como Glyph]]
[INS[dados que o desenvolvedor preenche por projeto = `[PH: descrição]`]]
[INS[dados invariantes do template = já preenchidos em literal de crase]]
[REF[ordem de leitura canônica: ver `AGENTS.md` na raiz → seção "Reading order"]]
[/BLOCK]

---

## Stack alvo do template (contexto para todos os docs)

[OFF]
"Photino.Blazor Custom Template" = HÍBRIDO de duas camadas:

  1. UI  (desktop)  → Photino.Blazor
       - Janela nativa (Photino.NET) hospedando Blazor
       - net8.0 / net9.0, Tailwind CSS v4
       - Projetos-base neste repo: ViewEngine (lib) + ViewCore (host)

  2. API (backend)  → ASP.NET Core Web API
       - Controllers → Services → EF Core → (SQL Server | PostgreSQL | MySQL)
       - xUnit + EF Core InMemory, Swagger, Docker

A UI consome a API via HttpClient. As duas camadas coexistem no template.
"A base" (código de ViewEngine/ViewCore) NÃO é alterada — apenas incrementada.
[ON]

[VRFY[coerência + regras Glyph]]
