<!-- Glyph Markup (v1.6). Decoder: ./GLYPH_LEGEND.md. Código/tabelas em [OFF]...[ON]. -->

[SECTION: data-model]
[NT[`DATA_MODEL.md — modelo de dados da camada API (entidades, tipos, banco)`]]
[INS[rd `./GLYPH_LEGEND.md` antes de parsear este arquivo]]
[INS[preencher todo `[PH: ...]` com os dados do projeto; a IA gera Entities + AppDbContext a partir daqui]]
[NT[`camada API. A UI Photino.Blazor não acessa o banco — consome via API_ENDPOINTS.md`]]

# DATA_MODEL.md — Modelo de Dados

---

## Conexão

[BLOCK: conexao]
[INS[Servidor: `[PH: ex DESKTOP-R67VAAB ou servidor-sql.empresa.com]`]]
[INS[Banco: `[PH: ex ContosoRetailDW]`]]
[INS[Schema: `[PH: ex dbo]`]]
[INS[Autenticação: `[PH: SQL Auth | Windows Auth]`]]
[INS[Banco escolhido (define o provider EF Core): `[PH: SQL Server | PostgreSQL | MySQL]`]]
[/BLOCK]
[WARN[`credenciais ficam em appsettings.json / variáveis de ambiente — nunca neste doc`]]
[REF[`provider e connection string por banco: DEPENDENCIES.md`]]

---

## Entidades

### [PH: nome da entidade, ex FactSales]
[INS[Tabela: `[PH: ex dbo.FactSales]`]]
[INS[Descrição: `[PH: o que a tabela representa]`]]
[OFF]
| Coluna       | Tipo SQL Server | Tipo C#   | PK | Nullable | Descrição        |
|--------------|-----------------|-----------|----|----------|------------------|
| [PH: col]    | INT             | int       | ✅ | Não      | [PH: descrição]  |
| [PH: col]    | INT             | int?      |    | Sim      | [PH: descrição]  |
| [PH: col]    | DATETIME        | DateTime? |    | Sim      | [PH: descrição]  |
| [PH: col]    | DECIMAL(18,4)   | decimal?  |    | Sim      | [PH: descrição]  |
| [PH: col]    | VARCHAR(100)    | string?   |    | Sim      | [PH: descrição]  |
[ON]

### [PH: segunda entidade, se houver]
[INS[Tabela: `[PH: ...]`]] [INS[Descrição: `[PH: ...]`]]
[OFF]
| Coluna    | Tipo SQL Server | Tipo C# | PK | Nullable | Descrição       |
|-----------|-----------------|---------|----|----------|-----------------|
| [PH: col] | INT             | int     | ✅ | Não      | [PH: descrição] |
| [PH: col] | VARCHAR(200)    | string? |    | Sim      | [PH: descrição] |
[ON]

---

## Relacionamentos

[INS[descrever relacionamentos entre entidades, se houver: `[PH: ...]`]]
[OFF]
Exemplo:
  FactSales.ProductKey → DimProduct.ProductKey (N:1)
  FactSales.StoreKey   → DimStore.StoreKey     (N:1)
[ON]
[COND[`leitura simples sem relacionamentos mapeados via EF Core`]][FBK[`declarar explicitamente aqui`]]

---

## Diagrama ER (opcional, Mermaid)

[INS[preencher com as entidades reais; regenerável no Draw.io: `Arrange → Insert → Advanced → Mermaid`]]
[NT[`fence verbatim — não é Glyph; renderiza como diagrama em visualizadores Markdown`]]

```mermaid
erDiagram
    [PH: EntidadeA] ||--o{ [PH: EntidadeB] : "relação"
    [PH: EntidadeA] {
        int Id PK
        string [PH: campo]
        int [PH: fkId] FK
    }
    [PH: EntidadeB] {
        int Id PK
        int [PH: fkId] FK
    }
```

---

## Decisões de modelagem (aprovar / reverter)

[INS[quando o modelo divergir do pedido literal (unificar tabelas, adicionar campo/entidade), registrar aqui a decisão + o porquê, para o dev aprovar ou reverter]]
[OFF]
| # | Decisão                          | Por quê                          | Reversível? |
|---|----------------------------------|----------------------------------|-------------|
| 1 | [PH: ex unificar A+B em "Pessoas"] | [PH: motivo]                    | [PH: sim/não + custo] |
| 2 | [PH: campo/entidade novo `*`]    | [PH: exigido pelo fluxo X]       | [PH: ...]   |
[ON]
[NT[`marque campos/entidades acrescentados além do pedido literal com `*` na coluna Descrição das tabelas acima`]]

---

## Mapeamento AppDbContext

[INS[a IA gera o `AppDbContext` com um `DbSet<T>` por entidade e o mapeamento em `OnModelCreating`]]
[OFF]
public DbSet<{NomeEntidade}> {NomeEntidade} { get; set; }

modelBuilder.Entity<{NomeEntidade}>()
    .ToTable("{NomeTabela}", "{Schema}")
    .HasKey(e => e.{NomePK});
[ON]
[COND[`PostgreSQL`]][FBK[`tabelas em snake_case, schema "public" (pode omitir) — ver DEPENDENCIES.md §Opção B`]]

---

[EX[`REFERÊNCIA — apagar antes de usar`]]
[OFF]
Entidade: FactSales — Tabela: dbo.FactSales

| Coluna        | Tipo SQL      | Tipo C#   | PK | Nullable |
|---------------|---------------|-----------|----|----------|
| SalesKey      | INT           | int       | ✅ | Não      |
| ProductKey    | INT           | int?      |    | Sim      |
| StoreKey      | INT           | int?      |    | Sim      |
| DateKey       | DATETIME      | DateTime? |    | Sim      |
| SalesQuantity | INT           | int?      |    | Sim      |
| SalesAmount   | DECIMAL(18,4) | decimal?  |    | Sim      |
| TotalCost     | DECIMAL(18,4) | decimal?  |    | Sim      |
[ON]

[VRFY[coerência + regras Glyph]]
[/SECTION]
