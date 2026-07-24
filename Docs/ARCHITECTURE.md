<!-- Glyph Markup (v1.6). Decoder: ./GLYPH_LEGEND.md. Código/tabelas em [OFF]...[ON]. -->

[SECTION: architecture]
[NT[`ARCHITECTURE.md — arquitetura, camadas e convenções (híbrido UI + API)`]]
[INS[rd `./GLYPH_LEGEND.md` antes de parsear este arquivo]]
[PRIO[`este é o doc de padrões — a IA o segue para gerar/incrementar código`]]

# ARCHITECTURE.md — Arquitetura e Padrões

O template tem **duas camadas independentes**: UI desktop (Photino.Blazor) e API
(ASP.NET Core). Cada uma tem suas regras. A UI consome a API via HttpClient.

---

## §UI — Camada desktop (Photino.Blazor)

[MAND[`não alterar a base (ViewEngine/ViewCore) — criar componentes/serviços novos ao redor`]]

[BLOCK: ui-camadas]
[INS[`Components/` — componentes .razor; markup + binding; sem regra de negócio pesada]]
[INS[`Services/` (UI) — clientes de API (HttpClient tipado) e estado de tela; injetados via DI]]
[INS[`wwwroot/` — index.html, imports.css (entrada Tailwind) e index.css (gerado)]]
[INS[`Program.cs` — PhotinoBlazorAppBuilder, registra RootComponents e serviços]]
[/BLOCK]

### Bootstrap da UI (referência — base do repo)
[OFF]
[STAThread]
static void Main(string[] args)
{
    var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);
    appBuilder.Services.AddLogging();

    // cliente da API (camada API deste template)
    appBuilder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:{porta}/api/") });
    appBuilder.Services.AddScoped<I{Entidade}ApiClient, {Entidade}ApiClient>();

    appBuilder.RootComponents.Add<App>("app");

    var app = appBuilder.Build();
    app.MainWindow.SetTitle("[PH: título da janela]");
    app.Run();
}
[ON]

### Estilo — Tailwind CSS v4
[MAND[`entrada usa sintaxe v4: `@import "tailwindcss";` + `@source "../**/*.razor";``]]
[DONT[`usar diretivas v3 (`@tailwind base/components/utilities`); reintroduzir `--watch` no target de build; editar o index.css gerado`]]
[REF[`pipeline completo de Tailwind: AGENTS.md (raiz) §Troubleshooting`]]

### Convenções de UI
[OFF]
| Elemento              | Convenção                    | Exemplo                     |
|-----------------------|------------------------------|-----------------------------|
| Componente            | PascalCase.razor             | SalesList.razor             |
| Cliente de API        | I{Entidade}ApiClient         | ISalesApiClient             |
| Classe de estilo custom| @apply em imports.css       | .divider, .cursive          |
| Utilitário Tailwind   | direto no class="..."        | class="border-2 rounded-lg" |
[ON]

---

## §API — Camada backend (ASP.NET Core Web API)

[MAND[`regra de negócio SÓ no Service; nunca no Controller nem no ViewModel`]]
[MAND[`sempre interface antes da implementação do Service`]]
[MAND[`async/await em métodos de Controller e Service`]]

### Controllers/
[BLOCK: controller]
[INS[recebe HTTP, valida parâmetros, delega ao Service, devolve resposta]]
[DONT[conter regra de negócio; acessar banco diretamente]]
[/BLOCK]
[OFF]
[ApiController]
[Route("api/[controller]")]
public class ExemploController : ControllerBase
{
    private readonly IExemploService _service;
    public ExemploController(IExemploService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExemploViewModel>>> GetAll(
        [FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim)
        => Ok(await _service.GetAll(dataInicio, dataFim));
}
[ON]

### Models/
[BLOCK: models]
[INS[`Entity` — espelho fiel da tabela; mapeada por EF Core; sem campo calculado; sem regra]]
[INS[`ViewModel` — contrato da API; serializado em JSON; pode omitir campos sensíveis; campo calculado é DECLARADO aqui mas CALCULADO no Service]]
[/BLOCK]
[OFF]
public class ExemploEntity        // espelho da tabela
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Valor { get; set; }
    public decimal Custo { get; set; }
}

public class ExemploViewModel     // contrato da API
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Valor { get; set; }
    public decimal Custo { get; set; }
    public decimal? Margem { get; set; }   // calculado no Service, declarado aqui
}
[ON]

### Data/
[INS[`AppDbContext` herda `DbContext`; declara `DbSet<T>`; configura mapeamento em `OnModelCreating`]]
[OFF]
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<ExemploEntity> Exemplo { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.Entity<ExemploEntity>().ToTable("NomeDaTabela", "dbo").HasKey(e => e.Id);
}
[ON]

### Services/
[BLOCK: service]
[REQ[uma interface `I{Entidade}Service` (contrato) + uma implementação `{Entidade}Service` (regra)]]
[MAND[campo calculado é computado aqui]]
[/BLOCK]
[OFF]
public interface IExemploService
{
    Task<IEnumerable<ExemploViewModel>> GetAll(DateTime inicio, DateTime fim);
}

public class ExemploService : IExemploService
{
    private readonly AppDbContext _context;
    public ExemploService(AppDbContext context) => _context = context;

    public async Task<IEnumerable<ExemploViewModel>> GetAll(DateTime inicio, DateTime fim)
    {
        var dados = await _context.Exemplo
            .Where(e => e.Data >= inicio && e.Data <= fim).ToListAsync();

        return dados.Select(e => new ExemploViewModel
        {
            Id = e.Id, Nome = e.Nome, Valor = e.Valor, Custo = e.Custo,
            Margem = e.Valor - e.Custo   // regra de negócio aqui
        });
    }
}
[ON]

### Tests/
[INS[um arquivo de teste por Service; `UseInMemoryDatabase`; padrão AAA; cada regra testada isoladamente]]
[REF[`detalhe: TESTS.md`]]

### Program.cs (API)
[INS[registra serviços no DI; configura pipeline de middleware]]
[OFF]
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IExemploService, ExemploService>();
[ON]

### Convenções de nomenclatura (API)
[OFF]
| Elemento               | Convenção              | Exemplo                     |
|------------------------|------------------------|-----------------------------|
| Controller             | {Entidade}Controller   | VendasController            |
| Service interface      | I{Entidade}Service     | IVendasService              |
| Service implementação  | {Entidade}Service      | VendasService               |
| ViewModel              | {Entidade}ViewModel    | VendasViewModel             |
| Teste                  | {Entidade}ServiceTests | VendasServiceTests          |
| Método de teste        | {Cenario}_{Resultado}  | GetAll_RetornaApenasPeriodo |
[ON]

### Injeção de Dependência — fluxo (API)
[OFF]
Program.cs registra → Container cria → Controller recebe via construtor
builder.Services.AddScoped<IVendasService, VendasService>()
        ↓
VendasController(IVendasService service)   // container injeta
[ON]

---

## §Contrato entre camadas

[BLOCK: contrato]
[INS[a UI NÃO acessa `AppDbContext` nem `Service` da API diretamente — só via HTTP]]
[INS[o `ViewModel` da API é o formato que a UI desserializa no seu cliente tipado]]
[NT[`os dois lados podem compartilhar um projeto `Contracts` de ViewModels/DTOs, se desejado — registrar em DECISIONS.md`]]
[/BLOCK]

[VRFY[coerência + regras Glyph]]
[/SECTION]
