<!-- Glyph Markup (v1.6). Decoder: ./GLYPH_LEGEND.md. Código/YAML em [OFF]...[ON]. -->

[SECTION: docker]
[NT[`DOCKER.md — dockerização do BACKEND (API). A UI desktop não é containerizada.`]]
[INS[rd `./GLYPH_LEGEND.md` antes de parsear este arquivo]]

# DOCKER.md — Dockerização do Backend

[WARN[`escopo = camada API. A UI Photino.Blazor é um app desktop NATIVO — distribua como binário por SO (win/mac/linux via `dotnet publish`), NÃO em container. Docker aqui só sobe a API + banco que a UI consome.`]]

---

## Arquivos a gerar (na raiz do projeto API)
[OFF]
Dockerfile
docker-compose.yml
docker-compose.override.yml   ← desenvolvimento local
.dockerignore
.env.example                  ← template de variáveis (versionado)
.env                          ← valores reais (NÃO versionado)
[ON]

---

## Dockerfile (multi-stage)
[OFF]
# ── build ──────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# ── runtime ────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "{NomeProjeto}.dll"]
[ON]
[INS[substituir `{NomeProjeto}` pelo nome do `.csproj` da API]]
[NT[`imagem final só com runtime (~200MB vs ~800MB com SDK)`]]

---

## .dockerignore
[OFF]
bin/
obj/
.vs/
*.user
appsettings.Development.json
.env
**/.git
**/node_modules
[ON]

---

## .env.example (versionado — sem valores reais)
[OFF]
DB_HOST=localhost
DB_PORT=5432
DB_NAME=nome_do_banco
DB_USER=usuario
DB_PASSWORD=
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080
[ON]

---

## docker-compose.yml por banco (preencher UMA opção)

[NT[`Opção A — SQL Server`]]
[OFF]
services:
  api:
    build: .
    ports: ["8080:8080"]
    environment:
      - ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT:-Production}
      - ConnectionStrings__DefaultConnection=Server=${DB_HOST},${DB_PORT:-1433};Database=${DB_NAME};User Id=${DB_USER};Password=${DB_PASSWORD};TrustServerCertificate=True;
    depends_on:
      sqlserver: { condition: service_healthy }
    restart: unless-stopped
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment: { ACCEPT_EULA: "Y", MSSQL_SA_PASSWORD: "${DB_PASSWORD}" }
    ports: ["${DB_PORT:-1433}:1433"]
    volumes: ["sqlserver_data:/var/opt/mssql"]
    healthcheck:
      test: ["CMD","/opt/mssql-tools/bin/sqlcmd","-S","localhost","-U","sa","-P","${DB_PASSWORD}","-Q","SELECT 1"]
      interval: 10s
      timeout: 5s
      retries: 5
    restart: unless-stopped
volumes: { sqlserver_data: }
[ON]

[NT[`Opção B — PostgreSQL`]]
[OFF]
services:
  api:
    build: .
    ports: ["8080:8080"]
    environment:
      - ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT:-Production}
      - ConnectionStrings__DefaultConnection=Host=${DB_HOST};Database=${DB_NAME};Username=${DB_USER};Password=${DB_PASSWORD};
    depends_on:
      postgres: { condition: service_healthy }
    restart: unless-stopped
  postgres:
    image: postgres:16-alpine
    environment: { POSTGRES_DB: "${DB_NAME}", POSTGRES_USER: "${DB_USER}", POSTGRES_PASSWORD: "${DB_PASSWORD}" }
    ports: ["${DB_PORT:-5432}:5432"]
    volumes: ["postgres_data:/var/lib/postgresql/data"]
    healthcheck:
      test: ["CMD-SHELL","pg_isready -U ${DB_USER} -d ${DB_NAME}"]
      interval: 10s
      timeout: 5s
      retries: 5
    restart: unless-stopped
volumes: { postgres_data: }
[ON]

[NT[`Opção C — MySQL`]]
[OFF]
services:
  api:
    build: .
    ports: ["8080:8080"]
    environment:
      - ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT:-Production}
      - ConnectionStrings__DefaultConnection=Server=${DB_HOST};Database=${DB_NAME};User=${DB_USER};Password=${DB_PASSWORD};
    depends_on:
      mysql: { condition: service_healthy }
    restart: unless-stopped
  mysql:
    image: mysql:8.0
    environment:
      MYSQL_DATABASE: "${DB_NAME}"
      MYSQL_USER: "${DB_USER}"
      MYSQL_PASSWORD: "${DB_PASSWORD}"
      MYSQL_ROOT_PASSWORD: "${DB_PASSWORD}"
    ports: ["${DB_PORT:-3306}:3306"]
    volumes: ["mysql_data:/var/lib/mysql"]
    healthcheck:
      test: ["CMD","mysqladmin","ping","-h","localhost","-u","${DB_USER}","-p${DB_PASSWORD}"]
      interval: 10s
      timeout: 5s
      retries: 5
    restart: unless-stopped
volumes: { mysql_data: }
[ON]

---

## docker-compose.override.yml (dev local)
[OFF]
services:
  api:
    environment: [ "ASPNETCORE_ENVIRONMENT=Development" ]
    volumes: [ ".:/app" ]   # hot reload em desenvolvimento
[ON]

---

## Variáveis → appsettings.json
[INS[ASP.NET Core lê env vars com `__` como separador de hierarquia]]
[OFF]
ConnectionStrings__DefaultConnection=Server=...
# equivale a { "ConnectionStrings": { "DefaultConnection": "Server=..." } }
[ON]
[NT[`o appsettings.json pode ter connection string vazia — em produção ela vem da env var`]]

---

## Comandos essenciais
[OFF]
cp .env.example .env                       # copiar e preencher
docker build -t {nome-projeto} .
docker-compose up            (-d)          # subir API + banco (background)
docker-compose logs -f api                 # logs
docker-compose down          (-v)          # derrubar (apagar volumes = resetar banco)
docker-compose up --build                  # rebuild após mudança
docker-compose -f docker-compose.yml up -d # produção (sem override de dev)
[ON]

---

## Banco externo (Cloud SQL, Azure, RDS)
[INS[banco já existe fora do Docker → remover o serviço de banco do compose e apontar direto]]
[OFF]
services:
  api:
    build: .
    ports: ["8080:8080"]
    environment: [ "ConnectionStrings__DefaultConnection=${DB_CONNECTION_STRING}" ]
    restart: unless-stopped
# .env
DB_CONNECTION_STRING=Host=34.xxx.xxx.xxx;Database=producao;Username=api_user;Password=...
[ON]

---

## Checklist
[BLOCK: checklist]
[INS[Dockerfile multi-stage criado]]
[INS[.dockerignore criado]]
[INS[.env.example versionado; .env local não versionado]]
[INS[docker-compose.yml com o banco escolhido]]
[INS[`docker-compose up` → API sobe e Swagger abre em localhost:8080/swagger]]
[INS[env vars sobrescrevem appsettings.json; volumes persistem; healthcheck garante banco pronto antes da API]]
[/BLOCK]

[VRFY[coerência + regras Glyph]]
[/SECTION]
