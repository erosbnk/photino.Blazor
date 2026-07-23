# DOCKER.md — Dockerização do Backend

> O Claude Code deve gerar os arquivos desta seção quando solicitado.
> Indique o banco de dados no `DATA_MODEL.md` — o `docker-compose.yml`
> sobe o container de banco correto automaticamente.

---

## Arquivos a gerar

```
raiz do projeto/
├── Dockerfile
├── docker-compose.yml
├── docker-compose.override.yml   ← desenvolvimento local
├── .dockerignore
└── .env.example                  ← template de variáveis — versionado
    .env                          ← valores reais — NÃO versionado
```

---

## Dockerfile

```dockerfile
# ── Estágio 1: build ──────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia e restaura dependências primeiro (cache de camadas)
COPY *.csproj .
RUN dotnet restore

# Copia o restante e publica
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# ── Estágio 2: runtime ────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copia apenas o publicado — imagem final sem SDK
COPY --from=build /app/publish .

# Porta padrão ASP.NET Core
EXPOSE 8080

# Variável de ambiente para rodar na porta 8080 dentro do container
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "{NomeProjeto}.dll"]
```

> Substitua `{NomeProjeto}` pelo nome do seu `.csproj`.
> Multi-stage build: imagem final tem apenas o runtime (~200MB vs ~800MB com SDK).

---

## .dockerignore

```
bin/
obj/
.vs/
*.user
appsettings.Development.json
.env
**/.git
**/node_modules
```

---

## .env.example (versionado — sem valores reais)

```env
# Banco de dados
DB_HOST=localhost
DB_PORT=5432
DB_NAME=nome_do_banco
DB_USER=usuario
DB_PASSWORD=

# API
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080
```

---

## docker-compose.yml por banco de dados

### Opção A — com SQL Server

```yaml
services:
  api:
    build: .
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT:-Production}
      - ConnectionStrings__DefaultConnection=Server=${DB_HOST},${DB_PORT:-1433};Database=${DB_NAME};User Id=${DB_USER};Password=${DB_PASSWORD};TrustServerCertificate=True;
    depends_on:
      sqlserver:
        condition: service_healthy
    restart: unless-stopped

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      ACCEPT_EULA: "Y"
      MSSQL_SA_PASSWORD: ${DB_PASSWORD}
    ports:
      - "${DB_PORT:-1433}:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql
    healthcheck:
      test: ["CMD", "/opt/mssql-tools/bin/sqlcmd", "-S", "localhost",
             "-U", "sa", "-P", "${DB_PASSWORD}", "-Q", "SELECT 1"]
      interval: 10s
      timeout: 5s
      retries: 5
    restart: unless-stopped

volumes:
  sqlserver_data:
```

---

### Opção B — com PostgreSQL (Cloud SQL local ou on-prem)

```yaml
services:
  api:
    build: .
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT:-Production}
      - ConnectionStrings__DefaultConnection=Host=${DB_HOST};Database=${DB_NAME};Username=${DB_USER};Password=${DB_PASSWORD};
    depends_on:
      postgres:
        condition: service_healthy
    restart: unless-stopped

  postgres:
    image: postgres:16-alpine
    environment:
      POSTGRES_DB: ${DB_NAME}
      POSTGRES_USER: ${DB_USER}
      POSTGRES_PASSWORD: ${DB_PASSWORD}
    ports:
      - "${DB_PORT:-5432}:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data
    healthcheck:
      test: ["CMD-SHELL", "pg_isready -U ${DB_USER} -d ${DB_NAME}"]
      interval: 10s
      timeout: 5s
      retries: 5
    restart: unless-stopped

volumes:
  postgres_data:
```

---

### Opção C — com MySQL

```yaml
services:
  api:
    build: .
    ports:
      - "8080:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT:-Production}
      - ConnectionStrings__DefaultConnection=Server=${DB_HOST};Database=${DB_NAME};User=${DB_USER};Password=${DB_PASSWORD};
    depends_on:
      mysql:
        condition: service_healthy
    restart: unless-stopped

  mysql:
    image: mysql:8.0
    environment:
      MYSQL_DATABASE: ${DB_NAME}
      MYSQL_USER: ${DB_USER}
      MYSQL_PASSWORD: ${DB_PASSWORD}
      MYSQL_ROOT_PASSWORD: ${DB_PASSWORD}
    ports:
      - "${DB_PORT:-3306}:3306"
    volumes:
      - mysql_data:/var/lib/mysql
    healthcheck:
      test: ["CMD", "mysqladmin", "ping", "-h", "localhost",
             "-u", "${DB_USER}", "-p${DB_PASSWORD}"]
      interval: 10s
      timeout: 5s
      retries: 5
    restart: unless-stopped

volumes:
  mysql_data:
```

---

## docker-compose.override.yml (desenvolvimento local)

```yaml
# Sobrescreve o compose principal apenas em desenvolvimento
# docker-compose up usa este arquivo automaticamente

services:
  api:
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
    volumes:
      # hot reload em desenvolvimento
      - .:/app
```

---

## Como as variáveis chegam no appsettings.json

O ASP.NET Core lê variáveis de ambiente com `__` como separador de hierarquia:

```env
# Variável de ambiente
ConnectionStrings__DefaultConnection=Server=...

# Equivale a no appsettings.json:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=..."
  }
}
```

Isso significa que o `appsettings.json` pode ter a connection string vazia — em produção ela vem da variável de ambiente, sem alterar o arquivo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  }
}
```

---

## Comandos essenciais

```bash
# Copiar template de variáveis e preencher
cp .env.example .env

# Build da imagem
docker build -t {nome-projeto} .

# Subir API + banco (desenvolvimento)
docker-compose up

# Subir em background
docker-compose up -d

# Ver logs da API
docker-compose logs -f api

# Derrubar tudo (mantém volumes)
docker-compose down

# Derrubar tudo e apagar volumes (reseta o banco)
docker-compose down -v

# Rebuild após mudança no código
docker-compose up --build

# Rodar em produção (sem override de dev)
docker-compose -f docker-compose.yml up -d
```

---

## Conectando a banco externo (Cloud SQL, Azure, RDS)

Quando o banco já existe fora do Docker (GCP, Azure, on-prem), remove o serviço de banco do `docker-compose.yml` e aponta direto:

```yaml
services:
  api:
    build: .
    ports:
      - "8080:8080"
    environment:
      - ConnectionStrings__DefaultConnection=${DB_CONNECTION_STRING}
    restart: unless-stopped
```

```env
# .env
DB_CONNECTION_STRING=Host=34.xxx.xxx.xxx;Database=producao;Username=api_user;Password=...
```

---

## Checklist de dockerização

```
[ ] Dockerfile criado com multi-stage build
[ ] .dockerignore criado
[ ] .env.example criado e versionado
[ ] .env criado localmente (não versionado)
[ ] docker-compose.yml criado com banco escolhido
[ ] docker-compose up — API sobe e Swagger abre em localhost:8080/swagger
[ ] Variáveis de ambiente sobrescrevem appsettings.json corretamente
[ ] Volumes persistem dados entre restarts
[ ] healthcheck garante que banco está pronto antes da API subir
```
