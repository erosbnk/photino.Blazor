# DEPENDENCIES.md — Dependências NuGet e Banco de Dados

> O Claude Code deve instalar os pacotes do banco escolhido e os obrigatórios.
> Indique o banco de dados no campo `DATABASE` do `DATA_MODEL.md` — o Claude Code
> escolhe automaticamente o provider correto.

---

## Escolha o banco de dados

Preencha apenas UMA das seções abaixo conforme o banco do projeto.

---

### Opção A — SQL Server (on-prem ou Azure)

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

**Program.cs:**
```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
```

**Connection string:**
```json
"DefaultConnection": "Server={SERVIDOR};Database={BANCO};User Id={USUARIO};Password={SENHA};TrustServerCertificate=True;"
```

---

### Opção B — PostgreSQL (Cloud SQL / Supabase / Neon / on-prem)

```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

**Program.cs:**
```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));
```

**Connection string:**
```json
"DefaultConnection": "Host={IP_OU_HOST};Database={BANCO};Username={USUARIO};Password={SENHA};SSL Mode=Require;Trust Server Certificate=true"
```

> GCP Cloud SQL: use o IP público da instância ou Cloud SQL Auth Proxy para conexão privada.

**Convenção PostgreSQL:** nomes de tabela em `snake_case`. Ajuste o `OnModelCreating`:
```csharp
modelBuilder.Entity<MinhaEntidade>().ToTable("minha_tabela");
// PostgreSQL não usa schema "dbo" — usa "public" por padrão (pode omitir)
```

---

### Opção C — MySQL (Cloud SQL / PlanetScale / on-prem)

```bash
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

**Program.cs:**
```csharp
var serverVersion = new MySqlServerVersion(new Version(8, 0, 0));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        serverVersion));
```

**Connection string:**
```json
"DefaultConnection": "Server={IP_OU_HOST};Database={BANCO};User={USUARIO};Password={SENHA};SslMode=Required;"
```

---

## Pacotes obrigatórios (todos os bancos)

```bash
# Swagger UI
dotnet add package Swashbuckle.AspNetCore

# Testes
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

---

## Versão do .NET

```xml
<!-- no .csproj -->
<TargetFramework>net10.0</TargetFramework>
```

---

## Configuração completa do Program.cs

```csharp
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// DbContext — substitua UseSqlServer/UseNpgsql/UseMySql conforme o banco
builder.Services.AddDbContext<AppDbContext>(options =>
    options.Use{Provider}(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Services de negócio — um por domínio
builder.Services.AddScoped<I{Entidade}Service, {Entidade}Service>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "{NomeProjeto} v1");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

---

## Configuração do appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "{substituir pela connection string do banco escolhido}"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

> Credenciais reais ficam no `appsettings.Development.json` — não versionado.
> Em produção (Docker/Cloud), usar variáveis de ambiente (ver `DOCKER.md`).
