# TESTS.md — Casos de Teste

> O Claude Code deve gerar um arquivo de teste por Service.
> Use os dados definidos em `BUSINESS_RULES.md` como massa de teste.
> Padrão obrigatório: AAA (Arrange / Act / Assert)

---

## Estrutura obrigatória de cada arquivo de teste

```csharp
public class {Entidade}ServiceTests
{
    // 1. Cria banco em memória — isolado por teste
    private AppDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    // 2. Popula massa de teste
    private void PopularDados(AppDbContext context)
    {
        context.{Entidade}.AddRange(/* dados de BUSINESS_RULES.md */);
        context.SaveChanges();
    }

    // 3. Testes organizados por categoria
}
```

---

## Categorias de teste obrigatórias

### Categoria 1 — Filtros

| Teste | Cenário | Resultado esperado |
|---|---|---|
| `GetAll_Filtro{Campo}_Retorna_Apenas_Periodo` | Filtro válido com dados | Retorna só registros do período |
| `GetAll_Filtro_PeriodoVazio_RetornaListaVazia` | Período sem dados | Retorna `[]` |
| `GetAll_Filtro_DatasInvertidas_RetornaListaVazia` | Início > Fim | Retorna `[]` |
| `GetAll_FiltroPeriodoCompleto_RetornaTodos` | Período cobre todos | Retorna todos os registros |

---

### Categoria 2 — Campos calculados

> Um teste por campo calculado listado em `BUSINESS_RULES.md`

| Teste | Cenário | Resultado esperado |
|---|---|---|
| `GetAll_{Campo}_CalculadoCorretamente` | Valores normais | Resultado da fórmula |
| `GetAll_{Campo}_Nulo_Quando{CampoBase}Nulo` | Campo base é null | Campo calculado é null |
| `GetAll_{Campo}_Zero_QuandoResultadoZero` | Custo = Valor | Campo calculado = 0 |

---

### Categoria 3 — Integridade dos dados

| Teste | Cenário | Resultado esperado |
|---|---|---|
| `GetAll_TodosCampos_PopuladosCorretamente` | Registro completo | Todos os campos batem com a entrada |
| `GetById_RetornaRegistroCorreto` | ID existente | Registro correto |
| `GetById_RetornaNotFound_QuandoIdInexistente` | ID que não existe | 404 Not Found |

---

## Exemplo de teste gerado (REFERÊNCIA)

```csharp
[Fact]
public async Task GetAll_GrossMargin_CalculadaCorretamente()
{
    // Arrange
    var context = CriarContexto();
    context.FactSales.Add(new FactSales
    {
        SalesKey    = 1,
        DateKey     = new DateTime(2009, 1, 5),
        SalesAmount = 1000.00m,
        TotalCost   = 600.00m
    });
    context.SaveChanges();

    var service = new FactSalesService(context);

    // Act
    var resultado = await service.GetAll(
        new DateTime(2009, 1, 1),
        new DateTime(2009, 1, 31));

    // Assert
    var venda = resultado.First();
    Assert.Equal(400.00m, venda.GrossMargin);
}
```

---

## Cobertura mínima esperada

- [ ] Todos os filtros testados
- [ ] Cada campo calculado com pelo menos 3 cenários (normal, nulo, borda)
- [ ] GetById com ID existente e inexistente
- [ ] Integridade de todos os campos do ViewModel
