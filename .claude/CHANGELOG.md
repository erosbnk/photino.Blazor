# CHANGELOG.md — Histórico de Mudanças

> Formato: [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/)
> Versioning: [Semantic Versioning](https://semver.org/lang/pt-BR/)
>
> **MAJOR.MINOR.PATCH**
> - MAJOR → mudança incompatível na API (quebra contrato com consumidores)
> - MINOR → nova funcionalidade retrocompatível (novo endpoint, novo campo)
> - PATCH → correção de bug ou ajuste de regra sem mudança de contrato
>
> Mantenha o `[Unreleased]` sempre atualizado durante o desenvolvimento.
> Na hora do release, mova para uma versão numerada com a data.

---

## [Unreleased]

### Adicionado
- *(descreva aqui o que foi adicionado mas ainda não foi para produção)*

### Alterado
- *(descreva mudanças em funcionalidades existentes)*

### Corrigido
- *(descreva correções de bugs ou ajustes de regra)*

### Removido
- *(descreva o que foi removido)*

---

## [1.0.0] — AAAA-MM-DD

### Adicionado
- Endpoint `GET /api/{entidade}` com filtro por período
- Endpoint `GET /api/{entidade}/{id}` com retorno 404 quando não encontrado
- Campo calculado `{NomeCampo}` — *(descreva a fórmula em linguagem de negócio)*
- Swagger UI em `/swagger`
- Testes unitários cobrindo todos os campos calculados e filtros

---

## Exemplo preenchido (REFERÊNCIA — apagar antes de usar)

```
## [1.2.0] — 2026-07-15

### Adicionado
- Campo GrossMarginPercent na API de vendas
  Fórmula: (GrossMargin / SalesAmount) * 100
  Solicitado por: [nome do cliente/projeto]

### Alterado
- GrossMargin agora desconota o campo Discount antes de calcular
  Fórmula anterior: SalesAmount - TotalCost
  Fórmula atual:   (SalesAmount - Discount) - TotalCost
  Motivo: alinhamento com definição contábil do cliente

---

## [1.1.0] — 2026-06-01

### Adicionado
- Filtro por StoreKey no endpoint de vendas
- Endpoint GET /api/sales/summary com totais agrupados por mês

### Corrigido
- GrossMargin retornava 0 em vez de null quando TotalCost era nulo

---

## [1.0.0] — 2026-05-20

### Adicionado
- Versão inicial da API
- Endpoints GET /api/products e GET /api/sales
- Campo calculado GrossMargin = SalesAmount - TotalCost
```

---

## Instruções para o time

**Quando abrir um PR**, adicione a entrada no `[Unreleased]` descrevendo:
- O que mudou em linguagem de negócio (não técnica)
- A fórmula anterior e a nova (quando for alteração de cálculo)
- O motivo da mudança

**Quando fazer release**, mova o conteúdo de `[Unreleased]` para uma nova versão numerada com a data de hoje.

**Quem lê este arquivo:** desenvolvedores, gestores de projeto e clientes — use linguagem acessível.
