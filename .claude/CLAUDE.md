# CLAUDE.md — Instruções para o Claude Code

> Este arquivo é lido automaticamente pelo Claude Code ao iniciar a sessão.
> Leia todos os arquivos em `/docs` antes de gerar qualquer código.

---

## Stack

- **Plataforma:** .NET 10 / C#
- **Framework web:** ASP.NET Core Web API
- **ORM:** Entity Framework Core com SQL Server, PostgreSQL ou MySQL
- **Documentação:** Swagger UI (Swashbuckle)
- **Testes:** xUnit + EF Core InMemory
- **Configuração:** appsettings.json + variáveis de ambiente

## Ordem de leitura dos docs

1. `docs/ARCHITECTURE.md` — padrões, camadas e convenções obrigatórias
2. `docs/DATA_MODEL.md` — entidades, tipos de dados e banco escolhido
3. `docs/BUSINESS_RULES.md` — regras de negócio e campos calculados
4. `docs/API_ENDPOINTS.md` — endpoints a gerar
5. `docs/DEPENDENCIES.md` — pacotes NuGet por banco de dados
6. `docs/TESTS.md` — casos de teste a implementar
7. `docs/DOCKER.md` — Dockerfile, docker-compose e variáveis de ambiente
8. `docs/DECISIONS.md` — decisões arquiteturais já tomadas (não contrariar)
9. `docs/CHANGELOG.md` — histórico de versões (atualizar após mudanças)

## Estrutura de pastas a gerar

```
{NomeProjeto}/
├── Controllers/
├── Data/
├── Models/
├── Services/
├── Tests/
├── appsettings.json
├── appsettings.Development.json
├── Program.cs
└── {NomeProjeto}.csproj
```

## Restrições obrigatórias

- NUNCA colocar regra de negócio no Controller
- NUNCA colocar regra de negócio no ViewModel
- SEMPRE criar interface antes da implementação do Service
- SEMPRE criar testes para cada regra de negócio
- SEMPRE usar async/await nos métodos de Controller e Service
- Credenciais NUNCA no código — usar appsettings.json ou variáveis de ambiente
- NUNCA contrariar uma decisão registrada em DECISIONS.md sem criar um novo ADR

## Processo de mudança de regra de negócio

Quando receber instrução de mudança em uma regra existente, siga esta ordem:

```
1. Confirme que BUSINESS_RULES.md foi atualizado pelo desenvolvedor
2. Atualize o teste — deve falhar antes de tocar no Service
3. Atualize o Service para o teste passar
4. Informe quais outros testes foram afetados
5. Lembre o desenvolvedor de atualizar CHANGELOG.md
```

Nunca atualize o código sem o doc correspondente estar atualizado primeiro.
