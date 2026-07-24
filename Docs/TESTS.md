<!-- Glyph Markup (v1.6). Decoder: ./GLYPH_LEGEND.md. Código/tabelas em [OFF]...[ON]. -->

[SECTION: tests]
[NT[`TESTS.md — testes de Service (API) e de componente (UI)`]]
[INS[rd `./GLYPH_LEGEND.md` antes de parsear este arquivo]]
[MAND[`padrão AAA (Arrange / Act / Assert); massa vinda de BUSINESS_RULES.md`]]

# TESTS.md — Casos de Teste

---

## §API — testes de Service (xUnit + EF Core InMemory)

[REQ[um arquivo de teste por Service]]

### Estrutura obrigatória
[OFF]
public class {Entidade}ServiceTests
{
    private AppDbContext CriarContexto()          // banco em memória, isolado por teste
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        return new AppDbContext(options);
    }

    private void PopularDados(AppDbContext context)  // massa de BUSINESS_RULES.md
    {
        context.{Entidade}.AddRange(/* dados */);
        context.SaveChanges();
    }
}
[ON]

### Categoria 1 — Filtros
[OFF]
| Teste                                          | Cenário             | Esperado           |
|------------------------------------------------|---------------------|--------------------|
| GetAll_Filtro{Campo}_Retorna_Apenas_Periodo    | Filtro válido       | só registros do período |
| GetAll_Filtro_PeriodoVazio_RetornaListaVazia   | Período sem dados    | []                 |
| GetAll_Filtro_DatasInvertidas_RetornaListaVazia| Início > Fim        | []                 |
| GetAll_FiltroPeriodoCompleto_RetornaTodos      | Período cobre todos  | todos os registros |
[ON]

### Categoria 2 — Campos calculados
[REQ[um teste por campo calculado de BUSINESS_RULES.md]]
[OFF]
| Teste                                    | Cenário          | Esperado           |
|------------------------------------------|------------------|--------------------|
| GetAll_{Campo}_CalculadoCorretamente     | Valores normais  | resultado da fórmula |
| GetAll_{Campo}_Nulo_Quando{CampoBase}Nulo| Campo base null  | campo calculado null |
| GetAll_{Campo}_Zero_QuandoResultadoZero  | Custo = Valor    | campo calculado = 0 |
[ON]

### Categoria 3 — Integridade
[OFF]
| Teste                                   | Cenário          | Esperado         |
|-----------------------------------------|------------------|------------------|
| GetAll_TodosCampos_PopuladosCorretamente| Registro completo| todos os campos batem |
| GetById_RetornaRegistroCorreto          | ID existente     | registro correto |
| GetById_RetornaNotFound_QuandoIdInexistente| ID inexistente| 404 Not Found    |
[ON]

[EX[`teste gerado (REFERÊNCIA)`]]
[OFF]
[Fact]
public async Task GetAll_GrossMargin_CalculadaCorretamente()
{
    // Arrange
    var context = CriarContexto();
    context.FactSales.Add(new FactSales {
        SalesKey = 1, DateKey = new DateTime(2009, 1, 5),
        SalesAmount = 1000.00m, TotalCost = 600.00m });
    context.SaveChanges();
    var service = new FactSalesService(context);

    // Act
    var resultado = await service.GetAll(new DateTime(2009,1,1), new DateTime(2009,1,31));

    // Assert
    Assert.Equal(400.00m, resultado.First().GrossMargin);
}
[ON]

---

## §UI — testes de componente (Photino.Blazor)

[NT[`opcional mas recomendado — a UI usa Blazor, então bUnit testa componentes sem abrir a janela nativa`]]
[OFF]
dotnet add package bunit
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit
[ON]
[BLOCK: ui-tests]
[INS[renderizar o componente com massa mockada e afirmar sobre o markup gerado]]
[INS[mockar o cliente de API (ISalesApiClient) — não bater na API real no teste]]
[EX[`Assert que <SalesList> renderiza N linhas dado um IReadOnlyList<SalesViewModel> mockado`]]
[/BLOCK]
[NT[`Photino/janela nativa não entram em teste unitário — cubra a lógica de componente, não o host`]]

---

## Cobertura mínima esperada
[BLOCK: cobertura]
[INS[API: todos os filtros testados]]
[INS[API: cada campo calculado com ≥3 cenários (normal, nulo, borda)]]
[INS[API: GetById com ID existente e inexistente]]
[INS[API: integridade de todos os campos do ViewModel]]
[INS[UI: cada componente com estado de dados renderiza corretamente (se adotar bUnit)]]
[/BLOCK]

[VRFY[coerência + regras Glyph]]
[/SECTION]
