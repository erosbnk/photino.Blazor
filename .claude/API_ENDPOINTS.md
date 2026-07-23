# API_ENDPOINTS.md — Endpoints da API

> Substitua os blocos `[SUBSTITUIR]` com os endpoints do seu projeto.
> O Claude Code usa este arquivo para gerar os Controllers.

---

## Base URL

```
https://localhost:{porta}/api
```

---

## [SUBSTITUIR — nome do recurso, ex: sales, products, inventory]

### GET /api/[SUBSTITUIR]

**Descrição:** [SUBSTITUIR — o que retorna]
**Controller:** [SUBSTITUIR — ex: FactSalesController]
**Service:** [SUBSTITUIR — ex: IFactSalesService.GetAll]

**Parâmetros de query:**

| Parâmetro | Tipo | Obrigatório | Descrição |
|---|---|---|---|
| [SUBSTITUIR] | [SUBSTITUIR] | Sim/Não | [SUBSTITUIR] |
| [SUBSTITUIR] | [SUBSTITUIR] | Sim/Não | [SUBSTITUIR] |

**Resposta de sucesso (200):**
```json
[
  {
    "[SUBSTITUIR]": [SUBSTITUIR],
    "[SUBSTITUIR]": [SUBSTITUIR],
    "[campo calculado]": [SUBSTITUIR]
  }
]
```

**Resposta de lista vazia (200):**
```json
[]
```

---

### GET /api/[SUBSTITUIR]/{id}

**Descrição:** [SUBSTITUIR — retorna um registro por ID]
**Parâmetro de rota:** `id` — [SUBSTITUIR — tipo e descrição]

**Resposta de sucesso (200):**
```json
{
  "[SUBSTITUIR]": [SUBSTITUIR]
}
```

**Resposta quando não encontrado (404):**
```json
{
  "message": "Registro não encontrado"
}
```

---

## [SUBSTITUIR — segundo recurso, se houver]

### GET /api/[SUBSTITUIR]

[SUBSTITUIR — mesmo formato acima]

---

## Configurações do Swagger

- **Título:** [SUBSTITUIR — ex: "MinhaEmpresa API v1"]
- **Versão:** v1
- **Endpoint OpenAPI:** `/openapi/v1.json`
- **Swagger UI:** `/swagger`

---

## Exemplo preenchido (REFERÊNCIA — apagar antes de usar)

```
Recurso: sales

GET /api/sales
  Parâmetros: dataInicio (DateTime, obrigatório), dataFim (DateTime, obrigatório)
  Retorno 200: lista de FactSalesViewModel com GrossMargin calculado
  Retorno 200 vazio: []

GET /api/sales/{id}
  Parâmetro: id (int) — SalesKey
  Retorno 200: FactSalesViewModel
  Retorno 404: registro não encontrado

Recurso: products

GET /api/products
  Sem parâmetros
  Retorno 200: lista de DimProductViewModel

GET /api/products/{id}
  Parâmetro: id (int) — ProductKey
  Retorno 200: DimProductViewModel
  Retorno 404: produto não encontrado
```
