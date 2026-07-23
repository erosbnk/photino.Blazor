# ARCHITECTURE.md — Arquitetura e Padrões

## Camadas e responsabilidades

### Controllers/
- Recebe requisições HTTP
- Valida parâmetros de entrada
- Delega para o Service
- Devolve resposta HTTP
- **NÃO contém regra de negócio**
- **NÃO acessa banco de dados diretamente**

```csharp
// Exemplo de Controller correto
[ApiController]
[Route("api/[controller]")]
public class ExemploController : ControllerBase
{
    private readonly IExemploService _service;

    public ExemploController(IExemploService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExemploViewModel>>> GetAll(
        [FromQuery] DateTime dataInicio,
        [FromQuery] DateTime dataFim)
    {
        return Ok(await _service.GetAll(dataInicio, dataFim));
    }
}
```

---

### Models/
Contém dois tipos de classes:

**Entities** — espelho fiel da tabela do banco:
- Mapeadas com EF Core
- Sem campos calculados
- Sem regra de negócio

**ViewModels** — contrato da API:
- Serializado para JSON
- Pode omitir campos sensíveis
- Pode combinar dados de múltiplas entidades
- Campos calculados declarados aqui, mas calculados no Service

```csharp
// Entity — espelho da tabela
public class ExemploEntity
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Valor { get; set; }
    public decimal Custo { get; set; }
}

// ViewModel — contrato da API
public class ExemploViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Valor { get; set; }
    public decimal Custo { get; set; }
    public decimal? Margem { get; set; } // calculado no Service, declarado aqui
}
```

---

### Data/
Contém o `AppDbContext`:
- Herda de `DbContext`
- Declara `DbSet<T>` para cada entidade
- Configura mapeamento em `OnModelCreating`

```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ExemploEntity> Exemplo { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExemploEntity>()
            .ToTable("NomeDaTabela", "dbo")
            .HasKey(e => e.Id);
    }
}
```

---

### Services/
Cada domínio tem:
- Uma **interface** (`IExemploService.cs`) — contrato
- Uma **implementação** (`ExemploService.cs`) — regra de negócio

```csharp
// Interface — contrato
public interface IExemploService
{
    Task<IEnumerable<ExemploViewModel>> GetAll(DateTime inicio, DateTime fim);
}

// Implementação — regra de negócio aqui
public class ExemploService : IExemploService
{
    private readonly AppDbContext _context;

    public ExemploService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ExemploViewModel>> GetAll(DateTime inicio, DateTime fim)
    {
        var dados = await _context.Exemplo
            .Where(e => e.Data >= inicio && e.Data <= fim)
            .ToListAsync();

        return dados.Select(e => new ExemploViewModel
        {
            Id     = e.Id,
            Nome   = e.Nome,
            Valor  = e.Valor,
            Custo  = e.Custo,
            Margem = e.Valor - e.Custo  // regra de negócio aqui
        });
    }
}
```

---

### Tests/
- Um arquivo de teste por Service
- Usa `UseInMemoryDatabase` — sem dependência de banco real
- Padrão AAA: Arrange / Act / Assert
- Testa cada regra de negócio isoladamente

---

### Program.cs
Responsável por:
1. Registrar serviços no container de DI
2. Configurar o pipeline de middleware

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IExemploService, ExemploService>();
```

---

## Convenções de nomenclatura

| Elemento | Convenção | Exemplo |
|---|---|---|
| Controller | `{Entidade}Controller` | `VendasController` |
| Service interface | `I{Entidade}Service` | `IVendasService` |
| Service implementação | `{Entidade}Service` | `VendasService` |
| ViewModel | `{Entidade}ViewModel` | `VendasViewModel` |
| Teste | `{Entidade}ServiceTests` | `VendasServiceTests` |
| Método de teste | `{Cenario}_{Resultado}` | `GetAll_RetornaApenasPeriodo` |

---

## Injeção de Dependência — fluxo

```
Program.cs registra → Container cria → Controller recebe via construtor
builder.Services.AddScoped<IVendasService, VendasService>()
                    ↓
        VendasController(IVendasService service)
                    ↓
        _service = service  // container injetou automaticamente
```
