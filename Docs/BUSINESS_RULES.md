<!-- Glyph Markup (v1.6). Decoder: ./GLYPH_LEGEND.md. Código/tabelas em [OFF]...[ON]. -->

[SECTION: business-rules]
[NT[`BUSINESS_RULES.md — regras de negócio e campos calculados`]]
[INS[rd `./GLYPH_LEGEND.md` antes de parsear este arquivo]]
[INS[preencher todo `[PH: ...]`; a IA gera Services e testes a partir daqui]]
[MAND[`regra de negócio vive no Service (API). A UI só exibe/valida entrada de tela.`]]

# BUSINESS_RULES.md — Regras de Negócio

---

## Domínio: [PH: ex Vendas, Inventário, Financeiro]
[INS[descrição geral (2-3 frases): `[PH: o que o domínio representa no negócio]`]]

---

## Entidade principal: [PH: ex FactSales]

### Campos calculados

#### [PH: nome do campo, ex GrossMargin]
[BLOCK: campo-calc]
[INS[Descrição: `[PH: o que representa no negócio]`]]
[INS[Fórmula: `[PH: ex SalesAmount - TotalCost]`]]
[INS[Tipo de retorno: `[PH: ex decimal?]`]]
[INS[Casos de nulo: `[PH: ex retorna null se SalesAmount ou TotalCost for null]`]]
[INS[Casos de borda: `[PH: ex pode ser negativo (venda com prejuízo)]`]]
[/BLOCK]
[OFF]
Entrada: SalesAmount = 1000, TotalCost = 600   → GrossMargin = 400
Entrada: SalesAmount = null, TotalCost = 600   → GrossMargin = null
[ON]

#### [PH: segundo campo calculado, se houver]
[INS[Descrição: `[PH: ...]`]] [INS[Fórmula: `[PH: ...]`]] [INS[Tipo: `[PH: ...]`]]
[INS[Casos de nulo: `[PH: ...]`]] [INS[Casos de borda: `[PH: ...]`]]

---

### Filtros obrigatórios

[BLOCK: filtro-periodo]
[INS[Campo: `[PH: ex DateKey]` — Tipo: `[PH: ex DateTime]`]]
[INS[Parâmetros: `dataInicio`, `dataFim`]]
[REQ[período vazio → retorna lista vazia, não erro]]
[REQ[datas invertidas (início > fim) → retorna lista vazia, não erro]]
[/BLOCK]
[INS[outros filtros: `[PH: campo, tipo, comportamento]`]]

---

### Validações de entrada
[OFF]
| Campo    | Obrigatório | Validação   | Erro esperado |
|----------|-------------|-------------|---------------|
| [PH]     | Sim/Não     | [PH]        | [PH]          |
| [PH]     | Sim/Não     | [PH]        | [PH]          |
[ON]

---

### Regras adicionais
#### [PH: nome da regra]
[INS[descrever em linguagem de negócio (não técnica): `[PH: ...]`]]
[OFF]
Situação: [PH: ...]
Comportamento esperado: [PH: ...]
[ON]

---

## Dados de teste representativos
[INS[massa pequena e controlada que cobre todos os cenários — usada nos testes unitários]]
[OFF]
| [Campo 1] | [Campo 2] | [Campo 3] | [Calculado] | Cenário       |
|-----------|-----------|-----------|-------------|---------------|
| [PH]      | [PH]      | [PH]      | [PH]        | Caso normal   |
| [PH]      | null      | [PH]      | null        | Campo nulo    |
| [PH]      | [PH]      | [PH]      | 0           | Resultado zero|
| [PH]      | [PH]      | [PH]      | [PH]        | Caso de borda |
[ON]

---

[EX[`REFERÊNCIA — apagar antes de usar`]]
[OFF]
Domínio: Vendas — Entidade: FactSales

GrossMargin
  Descrição: margem bruta (valor recebido menos custo)
  Fórmula: SalesAmount - TotalCost
  Tipo: decimal?
  Nulo: null se SalesAmount ou TotalCost for null
  Borda: pode ser negativo (prejuízo)

Filtro: DateKey entre dataInicio e dataFim; período vazio → []

Dados de teste:
  SalesKey=1, SalesAmount=1000, TotalCost=600  → 400  (normal)
  SalesKey=2, SalesAmount=null, TotalCost=600  → null (SalesAmount nulo)
  SalesKey=3, SalesAmount=400,  TotalCost=400  → 0    (sem margem)
  SalesKey=4, SalesAmount=300,  TotalCost=null → null (TotalCost nulo)
[ON]

---

## Processo de mudança de regra de negócio

[MAND[`docs antes de código — sempre`]]

[BLOCK: mudanca-dev]
[NT[`durante o desenvolvimento (refinamento de entendimento)`]]
[INS[1. atualiza este arquivo]]
[INS[2. atualiza o teste — deve FALHAR (red) antes de tocar no Service]]
[INS[3. atualiza o Service para o teste passar (green)]]
[INS[4. atualiza CHANGELOG.md → seção [Unreleased]]]
[INS[5. abre PR com docs + teste + código juntos]]
[/BLOCK]

[BLOCK: mudanca-prod]
[NT[`após produção (novo projeto de inclusão de regras)`]]
[INS[1. abre issue descrevendo a mudança em linguagem de negócio]]
[INS[2. atualiza este arquivo — registra fórmula anterior e nova]]
[INS[3. cria ADR em DECISIONS.md se a decisão não for óbvia]]
[INS[4. adiciona teste do novo comportamento (red)]]
[WARN[`se testes existentes quebrarem, a mudança afeta comportamento anterior — avaliar impacto antes de prosseguir`]]
[INS[5. atualiza o Service (green)]]
[INS[6. atualiza CHANGELOG.md com versão e data]]
[/BLOCK]

[NT[`prompt cirúrgico para a IA em mudança pontual`]]
[OFF]
O BUSINESS_RULES.md foi atualizado com: [descreva a mudança em linguagem de negócio]
Atualize apenas:
  - o teste em {Entidade}ServiceTests.cs cobrindo o novo cenário
  - o método {NomeMetodo} em {Entidade}Service.cs
Não toque em Controllers, Models, ViewModels, na UI, nem em outros arquivos.
[ON]

### Campos com histórico de mudança
[INS[quando uma fórmula mudar, registre para rastreabilidade]]
[OFF]
#### [PH: nome do campo]
| Versão | Data       | Fórmula          | Motivo               |
|--------|------------|------------------|----------------------|
| 1.0.0  | AAAA-MM-DD | [fórmula inicial]| Implementação inicial|
| 1.1.0  | AAAA-MM-DD | [fórmula nova]   | [motivo]             |
[ON]

[VRFY[coerência + regras Glyph]]
[/SECTION]
