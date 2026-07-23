# DECISIONS.md — Registro de Decisões Arquiteturais (ADR)

> **O que é um ADR?**
> Architecture Decision Record — um registro curto que captura uma decisão
> arquitetural importante, o contexto que levou a ela e suas consequências.
>
> **Por que registrar?**
> Decisões técnicas perdem contexto com o tempo. Quem entra no projeto depois
> — ou você mesmo em 6 meses — vai se perguntar "por que foi feito assim?".
> O ADR responde antes da pergunta ser feita.
>
> **Status possíveis:**
> - `Proposta` — em discussão, ainda não adotada
> - `Aceita` — adotada e em vigor
> - `Depreciada` — foi válida, não é mais recomendada
> - `Substituída por ADR-XXX` — outra decisão tomou o lugar

---

## ADR-001 — [SUBSTITUIR: título curto da decisão]

**Data:** AAAA-MM-DD
**Status:** Aceita

### Contexto
[SUBSTITUIR — descreva o problema ou situação que gerou a necessidade de decidir.
O que estava acontecendo? Quais eram as alternativas disponíveis?]

### Decisão
[SUBSTITUIR — descreva a decisão tomada de forma clara e direta.]

### Consequências
**Positivas:**
- [SUBSTITUIR]

**Negativas / trade-offs:**
- [SUBSTITUIR]

---

## ADR-002 — [SUBSTITUIR: título curto da decisão]

**Data:** AAAA-MM-DD
**Status:** Aceita

### Contexto
[SUBSTITUIR]

### Decisão
[SUBSTITUIR]

### Consequências
**Positivas:**
- [SUBSTITUIR]

**Negativas / trade-offs:**
- [SUBSTITUIR]

---

## Exemplo preenchido (REFERÊNCIA — apagar antes de usar)

```
## ADR-001 — Regra de negócio no Service, não no ViewModel

Data: 2026-05-20
Status: Aceita

### Contexto
O Pydantic (Python) e o EF Core (.NET) oferecem atalhos para calcular campos
diretamente no ViewModel/Schema usando propriedades computadas. Isso é conveniente
mas mistura responsabilidades — o ViewModel passa a conter lógica de negócio.

### Decisão
Campos calculados são sempre computados no Service e passados prontos ao ViewModel.
O ViewModel apenas declara o campo e seu tipo.

### Consequências
Positivas:
- Service é testável isoladamente sem instanciar o ViewModel
- Mudança de fórmula exige alteração em apenas um lugar (Service)
- ViewModel é um contrato puro de dados — sem comportamento

Negativas / trade-offs:
- Um pouco mais de código — o Service precisa mapear cada campo manualmente
- Não aproveita os atalhos do framework (@computed_field, etc.)

---

## ADR-002 — EF Core InMemory para testes, sem banco real

Data: 2026-05-20
Status: Aceita

### Contexto
Testes que dependem de banco de dados real são lentos, frágeis (dependem de
infraestrutura disponível) e difíceis de rodar em CI/CD sem configuração adicional.

### Decisão
Todos os testes unitários usam UseInMemoryDatabase com massa de dados
controlada definida em BUSINESS_RULES.md. Nenhum teste acessa SQL Server real.

### Consequências
Positivas:
- Testes rodam em milissegundos sem infraestrutura externa
- Massa controlada garante resultados determinísticos
- Funciona em qualquer máquina e em CI/CD sem configuração

Negativas / trade-offs:
- InMemory não replica comportamentos específicos do SQL Server
  (collation, tipos especiais, stored procedures)
- Testes de integração com banco real são necessários em cenários complexos
  — mas ficam fora deste template por ora

---

## ADR-003 — Arquitetura em camadas com DI explícita

Data: 2026-05-20
Status: Aceita

### Contexto
Poderíamos usar um padrão mais simples (Controller acessando banco diretamente)
para projetos pequenos. A cerimônia de Controllers/Services/Interfaces tem custo
de setup inicial.

### Decisão
Adotar sempre a separação em camadas com DI explícita registrada no Program.cs,
independentemente do tamanho do projeto.

### Consequências
Positivas:
- Qualquer projeto nasce testável — o custo de adicionar testes depois é muito maior
- Onboarding de novos desenvolvedores é previsível — sempre a mesma estrutura
- Regras de negócio têm um lugar único e claro

Negativas / trade-offs:
- Mais arquivos para projetos muito simples (3-4 endpoints)
- Curva de aprendizado inicial para devs vindos de scripts procedurais
```

---

## Quando criar um novo ADR

Crie um ADR sempre que:
- Uma decisão técnica importante for tomada e não for óbvia para quem chegar depois
- Uma abordagem alternativa for considerada e descartada — registre por quê
- Uma regra de negócio tiver interpretação ambígua e uma escolha for feita
- Um padrão do template for quebrado intencionalmente neste projeto

**Não crie ADR para:** decisões triviais, escolhas de nomenclatura, formatação de código.
