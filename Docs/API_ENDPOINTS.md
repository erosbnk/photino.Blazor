<!-- Glyph Markup (v1.6). Decoder: ./GLYPH_LEGEND.md. Código/tabelas em [OFF]...[ON]. -->

[SECTION: api-endpoints]
[NT[`API_ENDPOINTS.md — endpoints da API e como a UI Photino.Blazor os consome`]]
[INS[rd `./GLYPH_LEGEND.md` antes de parsear este arquivo]]
[INS[preencher todo `[PH: ...]`; a IA gera os Controllers (API) e os clientes tipados (UI) a partir daqui]]

# API_ENDPOINTS.md — Endpoints da API

---

## Base URL
[OFF]
https://localhost:{porta}/api
[ON]
[NT[`a UI Photino.Blazor aponta o HttpClient.BaseAddress para esta URL (ver ARCHITECTURE.md §UI)`]]

---

## Recurso: [PH: ex sales, products, inventory]

### GET /api/[PH: recurso]
[BLOCK: get-all]
[INS[Descrição: `[PH: o que retorna]`]]
[INS[Controller: `[PH: ex FactSalesController]` — Service: `[PH: ex IFactSalesService.GetAll]`]]
[INS[Cliente UI: `[PH: ex ISalesApiClient.GetAllAsync]`]]
[/BLOCK]
[NT[`parâmetros de query`]]
[OFF]
| Parâmetro | Tipo | Obrigatório | Descrição |
|-----------|------|-------------|-----------|
| [PH]      | [PH] | Sim/Não     | [PH]      |
| [PH]      | [PH] | Sim/Não     | [PH]      |
[ON]
[NT[`resposta 200`]]
[OFF]
[ { "[PH: campo]": [PH], "[PH: campo]": [PH], "[campo calculado]": [PH] } ]
[ON]
[NT[`resposta 200 vazia`]] [OFF][][ON]

### GET /api/[PH: recurso]/{id}
[INS[Descrição: `[PH: retorna um registro por ID]`]]
[INS[Parâmetro de rota: `id` — `[PH: tipo e descrição]`]]
[OFF]
200 → { "[PH: campo]": [PH] }
404 → { "message": "Registro não encontrado" }
[ON]

---

## Recurso: [PH: segundo recurso, se houver]
[INS[mesmo formato acima: `[PH: ...]`]]

---

## Consumo pela UI (cliente tipado)

[INS[a IA gera, na camada UI, um cliente HttpClient por recurso]]
[OFF]
public interface ISalesApiClient
{
    Task<IReadOnlyList<SalesViewModel>> GetAllAsync(DateTime inicio, DateTime fim);
    Task<SalesViewModel?> GetByIdAsync(int id);
}

public class SalesApiClient : ISalesApiClient
{
    private readonly HttpClient _http;
    public SalesApiClient(HttpClient http) => _http = http;

    public async Task<IReadOnlyList<SalesViewModel>> GetAllAsync(DateTime inicio, DateTime fim)
        => await _http.GetFromJsonAsync<List<SalesViewModel>>(
               $"sales?dataInicio={inicio:o}&dataFim={fim:o}") ?? new();

    public async Task<SalesViewModel?> GetByIdAsync(int id)
        => await _http.GetFromJsonAsync<SalesViewModel>($"sales/{id}");
}
[ON]
[NT[`o ViewModel desserializado na UI é o mesmo contrato exposto pela API (ARCHITECTURE.md §Contrato)`]]

---

## Configurações do Swagger
[BLOCK: swagger]
[INS[Título: `[PH: ex "MinhaEmpresa API v1"]`]]
[INS[Versão: `v1`]]
[INS[OpenAPI: `/openapi/v1.json` — Swagger UI: `/swagger`]]
[/BLOCK]

---

[EX[`REFERÊNCIA — apagar antes de usar`]]
[OFF]
Recurso: sales
  GET /api/sales
    Parâmetros: dataInicio (DateTime, obrigatório), dataFim (DateTime, obrigatório)
    200: lista de FactSalesViewModel com GrossMargin calculado
    200 vazio: []
  GET /api/sales/{id}
    Parâmetro: id (int) — SalesKey
    200: FactSalesViewModel   404: registro não encontrado

Recurso: products
  GET /api/products                → 200: lista de DimProductViewModel
  GET /api/products/{id}           → 200: DimProductViewModel  404: não encontrado
[ON]

[VRFY[coerência + regras Glyph]]
[/SECTION]
