# DATA_MODEL.md — Modelo de Dados

> Substitua os blocos `[SUBSTITUIR]` com as informações das suas tabelas.
> O Claude Code usa este arquivo para gerar as Entities e o AppDbContext.

---

## Conexão

- **Servidor:** [SUBSTITUIR — ex: DESKTOP-R67VAAB, servidor-sql.empresa.com]
- **Banco de dados:** [SUBSTITUIR — ex: ContosoRetailDW]
- **Schema:** [SUBSTITUIR — ex: dbo]
- **Autenticação:** SQL Auth / Windows Auth

> As credenciais ficam no `appsettings.json` — não incluir aqui.

---

## Entidades

### [SUBSTITUIR — nome da entidade, ex: FactSales]

**Tabela:** `[SUBSTITUIR — ex: dbo.FactSales]`
**Descrição:** [SUBSTITUIR — o que essa tabela representa]

| Coluna | Tipo SQL Server | Tipo C# | PK | Nullable | Descrição |
|---|---|---|---|---|---|
| [SUBSTITUIR] | INT | int | ✅ | Não | [SUBSTITUIR] |
| [SUBSTITUIR] | INT | int? | | Sim | [SUBSTITUIR] |
| [SUBSTITUIR] | DATETIME | DateTime? | | Sim | [SUBSTITUIR] |
| [SUBSTITUIR] | DECIMAL(18,4) | decimal? | | Sim | [SUBSTITUIR] |
| [SUBSTITUIR] | VARCHAR(100) | string? | | Sim | [SUBSTITUIR] |

---

### [SUBSTITUIR — segunda entidade, se houver]

**Tabela:** `[SUBSTITUIR]`
**Descrição:** [SUBSTITUIR]

| Coluna | Tipo SQL Server | Tipo C# | PK | Nullable | Descrição |
|---|---|---|---|---|---|
| [SUBSTITUIR] | INT | int | ✅ | Não | [SUBSTITUIR] |
| [SUBSTITUIR] | VARCHAR(200) | string? | | Sim | [SUBSTITUIR] |

---

## Relacionamentos

[SUBSTITUIR — descreva os relacionamentos entre as entidades, se houver]

```
Exemplo:
FactSales.ProductKey → DimProduct.ProductKey (N:1)
FactSales.StoreKey   → DimStore.StoreKey     (N:1)
```

> Se não houver relacionamentos mapeados via EF Core (leitura simples), informar aqui.

---

## Mapeamento AppDbContext

O Claude Code deve gerar o `AppDbContext` com:

```csharp
// Para cada entidade listada acima:
public DbSet<{NomeEntidade}> {NomeEntidade} { get; set; }

// Em OnModelCreating:
modelBuilder.Entity<{NomeEntidade}>()
    .ToTable("{NomeTabela}", "{Schema}")
    .HasKey(e => e.{NomePK});
```

---

## Exemplo preenchido (REFERÊNCIA — apagar antes de usar)

```
Entidade: FactSales
Tabela: dbo.FactSales

| Coluna        | Tipo SQL    | Tipo C#    | PK | Nullable |
|---------------|-------------|------------|----|----------|
| SalesKey      | INT         | int        | ✅ | Não      |
| ProductKey    | INT         | int?       |    | Sim      |
| StoreKey      | INT         | int?       |    | Sim      |
| DateKey       | DATETIME    | DateTime?  |    | Sim      |
| SalesQuantity | INT         | int?       |    | Sim      |
| SalesAmount   | DECIMAL(18,4)| decimal?  |    | Sim      |
| TotalCost     | DECIMAL(18,4)| decimal?  |    | Sim      |
```
