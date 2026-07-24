<!-- Glyph Markup (v1.6). Decoder: ./GLYPH_LEGEND.md. Código/comandos em [OFF]...[ON]. -->

[SECTION: dependencies]
[NT[`DEPENDENCIES.md — pacotes NuGet (API) + toolchain de UI (Photino/Tailwind)`]]
[INS[rd `./GLYPH_LEGEND.md` antes de parsear este arquivo]]

# DEPENDENCIES.md — Dependências

---

## §UI — Photino.Blazor (a base do repo)

[NT[`já presentes em ViewEngine/ViewCore — não reinstalar; listados para referência`]]
[OFF]
# ViewEngine (lib) — multi-target net8.0;net9.0
Photino.NET                                  4.0.16
Microsoft.AspNetCore.Components.WebView      8.0.12 (net8.0) / 9.0.1 (net9.0)
Microsoft.Extensions.Logging.Console         8.0.1  (net8.0) / 9.0.1 (net9.0)

# ViewCore (host) — toolchain de estilo (npm, em ViewCore/package.json)
@tailwindcss/cli                             ^4.3.3
tailwindcss                                  ^4.3.3
[ON]
[MAND[`Tailwind v4: entrada `@import "tailwindcss";` + `@source "../**/*.razor";``]]
[DONT[`reintroduzir `--watch` no target MSBuild TailwindCSS (trava o build)`]]

[INS[cliente HTTP para consumir a API — adicionar na UI se ainda não houver]]
[OFF]
dotnet add package Microsoft.Extensions.Http     # HttpClient/IHttpClientFactory
# System.Net.Http.Json já vem no runtime (GetFromJsonAsync)
[ON]

---

## §API — escolha do banco (preencher UMA opção)

[INS[banco definido em DATA_MODEL.md §Conexão]]

### Opção A — SQL Server (on-prem ou Azure)
[OFF]
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools

// Program.cs
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Connection string
"DefaultConnection": "Server={SERVIDOR};Database={BANCO};User Id={USUARIO};Password={SENHA};TrustServerCertificate=True;"
[ON]

### Opção B — PostgreSQL (Cloud SQL / Supabase / Neon / on-prem)
[OFF]
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Tools

// Program.cs
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Connection string
"DefaultConnection": "Host={IP_OU_HOST};Database={BANCO};Username={USUARIO};Password={SENHA};SSL Mode=Require;Trust Server Certificate=true"

// Convenção: tabelas em snake_case; schema "public" (pode omitir)
modelBuilder.Entity<MinhaEntidade>().ToTable("minha_tabela");
[ON]
[NT[`GCP Cloud SQL: IP público da instância ou Cloud SQL Auth Proxy para conexão privada`]]

### Opção C — MySQL (Cloud SQL / PlanetScale / on-prem)
[OFF]
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package Microsoft.EntityFrameworkCore.Tools

// Program.cs
var serverVersion = new MySqlServerVersion(new Version(8, 0, 0));
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), serverVersion));

// Connection string
"DefaultConnection": "Server={IP_OU_HOST};Database={BANCO};User={USUARIO};Password={SENHA};SslMode=Required;"
[ON]

---

## §API — pacotes obrigatórios (todos os bancos)
[OFF]
dotnet add package Swashbuckle.AspNetCore              # Swagger UI
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package Microsoft.EntityFrameworkCore.InMemory   # testes
[ON]

---

## Versões do .NET

[BLOCK: tfm]
[INS[API: `net10.0` — latest (LTS); evolui de forma independente da UI]]
[INS[UI (ViewEngine/ViewCore): `net8.0;net9.0` — teto atual é net9.0 (pinado por Photino.NET + WebView); não alterar a base]]
[INS[Contracts compartilhado (se existir): `netstandard2.0` — consumível por net8 (UI) e net10 (API)]]
[/BLOCK]
[NT[`camadas desacopladas por HTTP (HttpClient), sem project reference entre elas → TFMs divergentes não geram atrito. Decisão: DECISIONS.md ADR-006.`]]
[INS[Photino aos poucos: ViewEngine é multi-target; quando Photino/WebView suportarem net10, acrescente `net10.0` sem largar net8]]
[OFF]
<TargetFrameworks>net8.0;net9.0;net10.0</TargetFrameworks>
[ON]
[WARN[`regra de project reference: o consumidor precisa ter TFM ≥ o da lib. net8 (UI) NÃO referencia lib net10 → nunca mire o Contracts em net10 (use netstandard2.0 ou o menor TFM comum).`]]

---

## Program.cs completo (API)
[OFF]
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// substitua UseSqlServer/UseNpgsql/UseMySql conforme o banco
builder.Services.AddDbContext<AppDbContext>(o =>
    o.Use{Provider}(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<I{Entidade}Service, {Entidade}Service>();

var app = builder.Build();
if (app.Environment.IsDevelopment()) app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI(o => o.SwaggerEndpoint("/openapi/v1.json", "{NomeProjeto} v1"));
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
[ON]

---

## appsettings.json (API)
[OFF]
{
  "ConnectionStrings": { "DefaultConnection": "{connection string do banco escolhido}" },
  "Logging": { "LogLevel": { "Default": "Information", "Microsoft.AspNetCore": "Warning" } },
  "AllowedHosts": "*"
}
[ON]
[WARN[`credenciais reais em appsettings.Development.json (não versionado); em produção, variáveis de ambiente (DOCKER.md)`]]

[VRFY[coerência + regras Glyph]]
[/SECTION]
