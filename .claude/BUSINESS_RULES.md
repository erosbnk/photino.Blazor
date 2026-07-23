# BUSINESS_RULES.md — Regras de Negócio

> **INSTRUÇÕES PARA PREENCHIMENTO:**
> Substitua todos os blocos marcados com `[SUBSTITUIR]` pelas informações do seu projeto.
> Mantenha o formato — o Claude Code usa este arquivo para gerar os Services e os testes.

---

## Domínio: [SUBSTITUIR — ex: Vendas, Inventário, Financeiro]

### Descrição geral
[SUBSTITUIR — descreva em 2-3 frases o que esse domínio representa no negócio]

---

## Entidade principal: [SUBSTITUIR — ex: FactSales, InventoryKpi]

### Campos calculados

#### [SUBSTITUIR — nome do campo calculado, ex: GrossMargin]
- **Descrição:** [SUBSTITUIR — o que representa esse campo no negócio]
- **Fórmula:** [SUBSTITUIR — ex: SalesAmount - TotalCost]
- **Tipo de retorno:** [SUBSTITUIR — ex: decimal?, int?]
- **Casos de nulo:** [SUBSTITUIR — ex: retorna null se SalesAmount ou TotalCost for null]
- **Casos de borda:** [SUBSTITUIR — ex: retorna 0 se denominador for zero]

```
Exemplo:
Entrada: SalesAmount = 1000, TotalCost = 600
Saída esperada: GrossMargin = 400

Entrada: SalesAmount = null, TotalCost = 600
Saída esperada: GrossMargin = null
```

---

#### [SUBSTITUIR — segundo campo calculado, se houver]
- **Descrição:** [SUBSTITUIR]
- **Fórmula:** [SUBSTITUIR]
- **Tipo de retorno:** [SUBSTITUIR]
- **Casos de nulo:** [SUBSTITUIR]
- **Casos de borda:** [SUBSTITUIR]

---

### Filtros obrigatórios

#### Filtro de período
- **Campo:** [SUBSTITUIR — ex: DateKey, DataVenda]
- **Tipo:** [SUBSTITUIR — ex: DateTime, DateOnly]
- **Parâmetros:** `dataInicio`, `dataFim`
- **Comportamento com período vazio:** retorna lista vazia, não erro
- **Comportamento com datas invertidas:** retorna lista vazia, não erro

#### [SUBSTITUIR — outros filtros, se houver]
- **Campo:** [SUBSTITUIR]
- **Tipo:** [SUBSTITUIR]
- **Comportamento:** [SUBSTITUIR]

---

### Validações de entrada

| Campo | Obrigatório | Validação | Erro esperado |
|---|---|---|---|
| [SUBSTITUIR] | Sim/Não | [SUBSTITUIR] | [SUBSTITUIR] |
| [SUBSTITUIR] | Sim/Não | [SUBSTITUIR] | [SUBSTITUIR] |

---

### Regras de negócio adicionais

#### [SUBSTITUIR — nome da regra]
[SUBSTITUIR — descreva a regra em linguagem de negócio, não técnica]

```
Exemplo:
Situação: [SUBSTITUIR]
Comportamento esperado: [SUBSTITUIR]
```

---

## Dados de teste representativos

> Use estes dados para os testes unitários — massa pequena e controlada que cobre todos os cenários.

| [Campo 1] | [Campo 2] | [Campo 3] | [Campo calculado] | Cenário |
|---|---|---|---|---|
| [SUBSTITUIR] | [SUBSTITUIR] | [SUBSTITUIR] | [SUBSTITUIR] | Caso normal |
| [SUBSTITUIR] | null | [SUBSTITUIR] | null | Campo nulo |
| [SUBSTITUIR] | [SUBSTITUIR] | [SUBSTITUIR] | 0 | Margem zero |
| [SUBSTITUIR] | [SUBSTITUIR] | [SUBSTITUIR] | [SUBSTITUIR] | Caso de borda |

---

## Exemplo preenchido (REFERÊNCIA — apagar antes de usar)

```
Domínio: Vendas

Entidade principal: FactSales

Campos calculados:

  GrossMargin
  - Descrição: Margem bruta da venda — valor recebido menos o custo
  - Fórmula: SalesAmount - TotalCost
  - Tipo: decimal?
  - Casos de nulo: null se SalesAmount ou TotalCost for null
  - Casos de borda: nenhum — pode ser negativo (venda com prejuízo)

Filtros:
  - DateKey entre dataInicio e dataFim (DateTime)
  - Período vazio retorna lista vazia

Dados de teste:
  SalesKey=1, SalesAmount=1000, TotalCost=600  → GrossMargin=400  (caso normal)
  SalesKey=2, SalesAmount=null, TotalCost=600  → GrossMargin=null (SalesAmount nulo)
  SalesKey=3, SalesAmount=400,  TotalCost=400  → GrossMargin=0    (sem margem)
  SalesKey=4, SalesAmount=300,  TotalCost=null → GrossMargin=null (TotalCost nulo)
```

---

## Processo de mudança de regra de negócio

> Siga esta ordem obrigatoriamente — docs antes de código.

### Durante o desenvolvimento (refinamento de entendimento)

```
1. Atualiza este arquivo (BUSINESS_RULES.md)
        ↓
2. Atualiza o teste — ele vai falhar intencionalmente (red)
        ↓
3. Atualiza o Service para o teste passar (green)
        ↓
4. Atualiza CHANGELOG.md → seção [Unreleased]
        ↓
5. Abre PR com: docs + teste + código juntos
```

### Após produção (novo projeto de inclusão de regras)

```
1. Abre issue ou tarefa descrevendo a mudança em linguagem de negócio
        ↓
2. Atualiza este arquivo — registra fórmula anterior e nova
        ↓
3. Cria ADR em DECISIONS.md se a decisão não for óbvia
        ↓
4. Adiciona novo teste cobrindo o novo comportamento (red)
        ↓
5. Os testes existentes protegem — se quebrarem, a mudança
   afeta comportamento anterior — avaliar impacto antes de prosseguir
        ↓
6. Atualiza o Service (green)
        ↓
7. Atualiza CHANGELOG.md com versão e data
        ↓
8. PR com: issue + docs + teste + código
```

### Prompt para o Claude Code em mudanças pontuais

```
O BUSINESS_RULES.md foi atualizado com a seguinte mudança:
[descreva a mudança em linguagem de negócio]

Atualize apenas:
- o teste em {Entidade}ServiceTests.cs cobrindo o novo cenário
- o método {NomeMetodo} em {Entidade}Service.cs

Não toque em Controllers, Models, ViewModels ou outros arquivos.
```

### Campos com histórico de mudança

Quando uma fórmula mudar, registre aqui para rastreabilidade:

#### [SUBSTITUIR — nome do campo]

| Versão | Data | Fórmula | Motivo |
|---|---|---|---|
| 1.0.0 | AAAA-MM-DD | [fórmula original] | Implementação inicial |
| 1.1.0 | AAAA-MM-DD | [fórmula nova] | [motivo da mudança] |
