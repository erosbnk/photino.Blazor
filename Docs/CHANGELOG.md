<!-- Glyph Markup (v1.6). Decoder: ./GLYPH_LEGEND.md. Blocos verbatim em [OFF]...[ON]. -->

[SECTION: changelog]
[NT[`CHANGELOG.md — histórico de mudanças do template`]]
[INS[rd `./GLYPH_LEGEND.md` antes de parsear este arquivo]]
[REF[`formato: Keep a Changelog; versionamento: SemVer`]]

# CHANGELOG.md — Histórico

[OFF]
MAJOR.MINOR.PATCH
  MAJOR → mudança incompatível (quebra contrato com consumidores)
  MINOR → nova funcionalidade retrocompatível (novo endpoint/campo/tela)
  PATCH → correção de bug ou ajuste de regra sem mudança de contrato

Mantenha [Unreleased] atualizado durante o desenvolvimento.
No release, mova para uma versão numerada com a data.
[ON]

---

## [Unreleased]

[BLOCK: unreleased-adicionado]
[NT[Adicionado]]
[INS[`Docs/` convertidos para Glyph nativo + decoder `GLYPH_LEGEND.md` (ADR-003)]]
[INS[template re-orientado para híbrido UI Photino.Blazor + API ASP.NET (ADR-001)]]
[INS[`AGENTS.md` na raiz com padrões reais do repo + ordem de leitura dos docs]]
[INS[TFMs por camada definidos: API `net10.0`, UI `net8.0;net9.0`, Contracts `netstandard2.0` (ADR-006)]]
[INS[DATA_MODEL: diagrama ER opcional (Mermaid) + bloco de decisões de modelagem]]
[INS[README: restaurado o checklist de novo projeto (hibridizado UI+API)]]
[/BLOCK]
[BLOCK: unreleased-alterado]
[NT[Alterado]]
[INS[Tailwind migrado para sintaxe v4 (`@import`/`@source`); watch removido do build (ADR-002)]]
[/BLOCK]
[BLOCK: unreleased-pendente]
[NT[Corrigido / Removido]]
[INS[`[PH: descreva correções e remoções ainda não liberadas]`]]
[/BLOCK]

---

## [1.0.0] — [PH: AAAA-MM-DD]
[BLOCK: v1]
[NT[Adicionado]]
[INS[UI: app desktop Photino.Blazor (ViewEngine + ViewCore) com Tailwind v4]]
[INS[API: `GET /api/{entidade}` com filtro por período; `GET /api/{entidade}/{id}` com 404]]
[INS[API: campo calculado `[PH: nome]` — `[PH: fórmula em linguagem de negócio]`]]
[INS[API: Swagger UI em `/swagger`; testes unitários de filtros e campos calculados]]
[/BLOCK]

---

[EX[`REFERÊNCIA — apagar antes de usar`]]
[OFF]
## [1.2.0] — 2026-07-15
### Adicionado
- Campo GrossMarginPercent na API de vendas — (GrossMargin / SalesAmount) * 100
### Alterado
- GrossMargin agora desconta Discount antes de calcular
  anterior: SalesAmount - TotalCost   atual: (SalesAmount - Discount) - TotalCost
  motivo: alinhamento com definição contábil do cliente

## [1.1.0] — 2026-06-01
### Adicionado
- Filtro por StoreKey; GET /api/sales/summary (totais por mês)
### Corrigido
- GrossMargin retornava 0 em vez de null quando TotalCost era nulo
[ON]

---

## Instruções para o time
[BLOCK: instrucoes]
[INS[ao abrir PR: adicionar entrada em [Unreleased] — o que mudou em linguagem de negócio, fórmula anterior/nova, motivo]]
[INS[ao fazer release: mover [Unreleased] para versão numerada com a data de hoje]]
[NT[`quem lê: devs, gestores e clientes — use linguagem acessível`]]
[/BLOCK]

[VRFY[coerência + regras Glyph]]
[/SECTION]
