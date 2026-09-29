# TaskFlow

Plataforma de gerenciamento de projetos e tarefas desenvolvida com foco em demonstrar conceitos de arquitetura de software, desenvolvimento full stack, containerização e CI/CD.

O projeto foi pensado como um projeto de portfólio para demonstrar conhecimentos em:

- .NET / ASP.NET Core
- React + TypeScript
- PostgreSQL
- Entity Framework Core
- Docker
- GitLab CI/CD
- Testes automatizados
- Arquitetura em camadas
- Clean Architecture
- REST API
- Observabilidade
- Cloud / AWS / DigitalOcean

---

## Como executar

Pré-requisitos: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) e Docker.

```bash
# Banco de dados
docker compose up -d postgres

# API
cd backend
dotnet build TaskFlow.sln
dotnet run --project TaskFlow.Api
```

Com a API rodando em ambiente de desenvolvimento:

- Swagger: `http://localhost:5241/swagger`
- Health check: `http://localhost:5241/health`

As credenciais do PostgreSQL no `docker-compose.yml` são apenas para desenvolvimento local e podem ser sobrescritas pelas variáveis `POSTGRES_DB`, `POSTGRES_USER` e `POSTGRES_PASSWORD`.

---

## 1. Objetivo

O TaskFlow é uma aplicação de gerenciamento de projetos semelhante, em conceito, a ferramentas como Trello e Asana.

A aplicação permite que usuários:

- Criem projetos
- Criem tarefas
- Atribuam tarefas a usuários
- Definam prioridades
- Alterem o status das tarefas
- Definam prazos
- Acompanhem a produtividade
- Visualizem projetos em formato Kanban
- Consultem o histórico de alterações

O objetivo principal não é criar uma plataforma comercial completa, mas construir uma aplicação suficientemente robusta para demonstrar decisões de engenharia de software.

---

## 2. Stack

### Backend

- C#
- .NET / ASP.NET Core
- Entity Framework Core
- PostgreSQL
- REST API
- Swagger / OpenAPI

### Frontend

- React
- TypeScript
- Vite

### Infraestrutura

- Docker
- Docker Compose
- Linux

### CI/CD

- Git
- GitLab
- GitLab CI/CD

### Testes

- xUnit
- Integration Tests
- Unit Tests

### Evolução planejada

- MongoDB
- AWS / DigitalOcean
- Prometheus
- Grafana
- Kubernetes
- Rancher

---

## 3. Arquitetura

A aplicação é organizada utilizando separação de responsabilidades baseada em Clean Architecture.

```text
                         ┌──────────────────────┐
                         │       React          │
                         │    TypeScript        │
                         └──────────┬───────────┘
                                    │
                                    │ HTTP / JSON
                                    ▼
                         ┌──────────────────────┐
                         │    TaskFlow.Api      │
                         │                      │
                         │ Controllers          │
                         │ HTTP / Middleware    │
                         └──────────┬───────────┘
                                    │
                                    ▼
                     ┌──────────────────────────────┐
                     │    TaskFlow.Application      │
                     │                              │
                     │ Use Cases                    │
                     │ Services                     │
                     │ DTOs                         │
                     │ Business Orchestration       │
                     └──────────────┬───────────────┘
                                    │
                                    ▼
                     ┌──────────────────────────────┐
                     │       TaskFlow.Domain        │
                     │                              │
                     │ Entities                     │
                     │ Enums                        │
                     │ Business Rules               │
                     └──────────────────────────────┘
                                    ▲
                                    │
                     ┌──────────────┴───────────────┐
                     │  TaskFlow.Infrastructure     │
                     │                              │
                     │ Entity Framework Core        │
                     │ PostgreSQL                   │
                     │ Persistence                  │
                     └──────────────────────────────┘
```

Referências entre projetos (a dependência sempre aponta para o domínio):

```text
TaskFlow.Api            → TaskFlow.Application, TaskFlow.Infrastructure
TaskFlow.Infrastructure → TaskFlow.Application, TaskFlow.Domain
TaskFlow.Application    → TaskFlow.Domain
TaskFlow.Domain         → (nenhuma)
```

---

## 4. Estrutura do projeto

```text
TaskFlow/
│
├── backend/
│   │
│   ├── TaskFlow.sln
│   │
│   ├── TaskFlow.Api/
│   │   ├── Controllers/
│   │   ├── Middlewares/
│   │   ├── Extensions/
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   ├── TaskFlow.Application/
│   │   ├── Projects/
│   │   ├── Tasks/
│   │   ├── Users/
│   │   ├── DTOs/
│   │   └── Interfaces/
│   │
│   ├── TaskFlow.Domain/
│   │   ├── Entities/
│   │   ├── Enums/
│   │   ├── Exceptions/
│   │   └── Interfaces/
│   │
│   └── TaskFlow.Infrastructure/
│       ├── Persistence/
│       ├── Repositories/
│       ├── Configurations/
│       └── Migrations/
│
├── frontend/
│   │
│   └── taskflow-web/
│       ├── src/
│       │   ├── components/
│       │   ├── pages/
│       │   ├── services/
│       │   ├── hooks/
│       │   ├── types/
│       │   └── App.tsx
│       │
│       ├── package.json
│       └── vite.config.ts
│
├── tests/
│   ├── TaskFlow.UnitTests/
│   └── TaskFlow.IntegrationTests/
│
├── docker-compose.yml
├── .gitlab-ci.yml
└── README.md
```

---

## 5. Modelo de domínio

### User

Representa um usuário da plataforma.

```text
User
├── Id
├── Name
├── Email
├── PasswordHash
└── CreatedAt
```

### Project

Representa um projeto criado por um usuário.

```text
Project
├── Id
├── Name
├── Description
├── OwnerId
└── CreatedAt
```

### TaskItem

Representa uma tarefa pertencente a um projeto.

```text
TaskItem
├── Id
├── Title
├── Description
├── Status
├── Priority
├── DueDate
├── ProjectId
├── AssignedToId
└── CreatedAt
```

A entidade é chamada `TaskItem` para evitar conflito conceitual com `System.Threading.Tasks.Task`, amplamente utilizado no ecossistema .NET.

As entidades expõem propriedades com `private set` e só podem ser alteradas por métodos que validam as regras de negócio (`Project.Update`, `TaskItem.ChangeStatus`, `TaskItem.AssignTo`...). Violações lançam `DomainException`.

---

## 6. Relacionamentos

```text
User
 │
 ├────────────── owns ──────────────► Project
 │                                      │
 │                                      │ contains
 │                                      ▼
 └──────────── assigned to ─────────► TaskItem
```

- Um usuário pode possuir vários projetos.
- Um projeto pode possuir várias tarefas.
- Uma tarefa pode ser atribuída a um usuário.

---

## 7. Status das tarefas

As tarefas possuem inicialmente três estados (`TaskItemStatus`):

```text
TODO
IN_PROGRESS
DONE
```

Representação:

```text
┌─────────────┐
│    TODO     │
└──────┬──────┘
       │
       ▼
┌─────────────┐
│ IN_PROGRESS │
└──────┬──────┘
       │
       ▼
┌─────────────┐
│    DONE     │
└─────────────┘
```

---

## 8. Prioridade

As tarefas podem possuir três níveis de prioridade (`TaskPriority`):

```text
LOW
MEDIUM
HIGH
```

---

## 9. API — Projects

A API segue o padrão REST.

### Criar projeto

```http
POST /api/projects
```

Request:

```json
{
  "name": "TaskFlow Development",
  "description": "Projeto de portfólio"
}
```

### Listar projetos

```http
GET /api/projects
```

### Buscar projeto

```http
GET /api/projects/{id}
```

### Atualizar projeto

```http
PUT /api/projects/{id}
```

### Excluir projeto

```http
DELETE /api/projects/{id}
```

---

## 10. API — Tasks

### Criar tarefa

```http
POST /api/projects/{projectId}/tasks
```

Request:

```json
{
  "title": "Implementar autenticação",
  "description": "Criar autenticação utilizando JWT",
  "priority": "HIGH",
  "dueDate": "2026-10-05"
}
```

### Listar tarefas

```http
GET /api/projects/{projectId}/tasks
```

### Buscar tarefa

```http
GET /api/tasks/{id}
```

### Atualizar tarefa

```http
PUT /api/tasks/{id}
```

### Alterar status

```http
PATCH /api/tasks/{id}/status
```

Request:

```json
{
  "status": "IN_PROGRESS"
}
```

### Atribuir tarefa

```http
PATCH /api/tasks/{id}/assignee
```

Request:

```json
{
  "userId": "user-id"
}
```

---

## 11. Banco de dados

O banco principal é o PostgreSQL.

A escolha do PostgreSQL ocorre porque o domínio possui dados altamente relacionais:

```text
Users
  │
  ├── Projects
  │      │
  │      └── Tasks
  │
  └── Assigned Tasks
```

Esses relacionamentos são naturais para um banco relacional.

O Entity Framework Core será utilizado como ORM.

---

## 12. MongoDB

MongoDB será uma evolução opcional do projeto.

Não será utilizado simplesmente para adicionar uma tecnologia à stack. A proposta é utilizá-lo caso exista uma necessidade real de armazenar eventos de auditoria.

Exemplo:

```json
{
  "event": "TASK_STATUS_CHANGED",
  "taskId": "123",
  "userId": "456",
  "oldStatus": "TODO",
  "newStatus": "IN_PROGRESS",
  "timestamp": "2026-09-29T18:00:00Z"
}
```

A arquitetura ficaria:

```text
┌───────────────┐        ┌───────────────┐
│  PostgreSQL   │        │    MongoDB    │
│               │        │               │
│ Users         │        │ Audit Events  │
│ Projects      │        │               │
│ Tasks         │        │               │
└───────────────┘        └───────────────┘
```

---

## 13. Frontend

O frontend será desenvolvido utilizando React + TypeScript.

Estrutura planejada:

```text
src/
│
├── components/
│   ├── Button/
│   ├── Modal/
│   ├── TaskCard/
│   ├── ProjectCard/
│   └── KanbanBoard/
│
├── pages/
│   ├── Login/
│   ├── Dashboard/
│   ├── Projects/
│   └── ProjectDetails/
│
├── services/
│   └── api.ts
│
├── hooks/
│
├── types/
│
└── App.tsx
```

---

## 14. Dashboard

A dashboard apresentará informações resumidas sobre os projetos e tarefas.

```text
┌─────────────────────────────────────────────────────────┐
│ TaskFlow                                Alexandre       │
├──────────────┬──────────────────────────────────────────┤
│              │                                          │
│ Dashboard    │ Projects                                 │
│              │                                          │
│ Projects     │ ┌────────────────┐ ┌────────────────┐    │
│              │ │ Website        │ │ Mobile App     │    │
│ Tasks        │ │ 12 tasks       │ │ 8 tasks        │    │
│              │ │ 4 completed    │ │ 3 completed    │    │
│              │ └────────────────┘ └────────────────┘    │
│              │                                          │
└──────────────┴──────────────────────────────────────────┘
```

---

## 15. Kanban

Dentro de cada projeto será possível visualizar as tarefas em um quadro Kanban.

```text
┌──────────────┐ ┌────────────────┐ ┌──────────────┐
│ TODO         │ │ IN PROGRESS    │ │ DONE         │
├──────────────┤ ├────────────────┤ ├──────────────┤
│              │ │                │ │              │
│ Login        │ │ Dashboard      │ │ Database     │
│ HIGH         │ │ MEDIUM         │ │ HIGH         │
│              │ │                │ │              │
├──────────────┤ ├────────────────┤ ├──────────────┤
│ Register     │ │ API            │ │ Docker       │
│ LOW          │ │ HIGH           │ │ MEDIUM       │
│              │ │                │ │              │
└──────────────┘ └────────────────┘ └──────────────┘
```

---

## 16. Docker

A aplicação será executada através de containers.

```text
Docker Compose
│
├── taskflow-frontend
│   └── React
│
├── taskflow-api
│   └── ASP.NET Core
│
└── taskflow-postgres
    └── PostgreSQL
```

---

## 17. Docker Compose

Configuração atual (apenas o PostgreSQL) — ver [`docker-compose.yml`](docker-compose.yml):

```yaml
services:

  postgres:
    image: postgres:16
    container_name: taskflow-postgres
    restart: unless-stopped

    environment:
      POSTGRES_DB: ${POSTGRES_DB:-taskflow}
      POSTGRES_USER: ${POSTGRES_USER:-taskflow}
      POSTGRES_PASSWORD: ${POSTGRES_PASSWORD:-taskflow}

    ports:
      - "5432:5432"

    volumes:
      - taskflow_postgres_data:/var/lib/postgresql/data

volumes:
  taskflow_postgres_data:
```

---

## 18. CI/CD

O projeto utilizará GitLab CI/CD.

Pipeline planejado:

```text
Git Push → Build → Tests → Docker Build → Push Registry → Deploy
```

---

## 19. GitLab CI

Pipeline inicial — ver [`.gitlab-ci.yml`](.gitlab-ci.yml):

```yaml
stages:
  - build
  - test

backend_build:
  stage: build
  image: mcr.microsoft.com/dotnet/sdk:8.0

  script:
    - dotnet restore backend/TaskFlow.sln
    - dotnet build backend/TaskFlow.sln --no-restore

backend_test:
  stage: test
  image: mcr.microsoft.com/dotnet/sdk:8.0

  script:
    - dotnet test backend/TaskFlow.sln
```

Posteriormente serão adicionados:

- Build das imagens Docker
- Push para Container Registry
- Deploy automático
- Ambientes de staging e produção
- Variáveis protegidas
- Secrets

---

## 20. Testes

O projeto terá diferentes níveis de testes.

### Unit Tests

Testarão principalmente regras de domínio e casos de uso.

```text
CreateProject
    │
    ├── Name válido
    ├── Name vazio
    └── Description válida
```

### Integration Tests

Serão utilizados para validar a integração entre:

```text
API → Application → Infrastructure → PostgreSQL
```

---

## 21. Tratamento de erros

A API deverá possuir um mecanismo centralizado de tratamento de exceções, evitando que cada controller implemente seu próprio tratamento de erros.

Exemplo de resposta:

```json
{
  "status": 400,
  "title": "Validation Error",
  "message": "Project name is required",
  "timestamp": "2026-09-29T18:00:00Z"
}
```

---

## 22. Validação

As entradas da API deverão ser validadas antes de chegar às regras de negócio.

```text
POST /api/projects
        │
        ▼
┌───────────────────┐
│ Request Validation│
└────────┬──────────┘
         ▼
┌───────────────────┐
│ Application       │
│ Use Case          │
└────────┬──────────┘
         ▼
┌───────────────────┐
│ Domain Rules      │
└────────┬──────────┘
         ▼
┌───────────────────┐
│ PostgreSQL        │
└───────────────────┘
```

---

## 23. Logs

A aplicação deverá utilizar logs estruturados.

```text
INFO
ProjectCreated
projectId=123
userId=456
timestamp=...
```

Posteriormente os logs poderão ser centralizados utilizando ferramentas de observabilidade.

---

## 24. Health Checks

A API possui um endpoint de health check:

```http
GET /health
```

Resposta:

```json
{
  "status": "Healthy"
}
```

Posteriormente serão verificadas também as dependências (PostgreSQL, MongoDB).

---

## 25. Segurança

A aplicação deverá evoluir para utilizar:

- JWT
- Password hashing
- Authentication
- Authorization
- Role-based access control
- HTTPS
- CORS configurado
- Secrets através de variáveis de ambiente
- Validação de entrada
- Proteção contra exposição de credenciais

As credenciais nunca deverão ser armazenadas diretamente no código-fonte.

---

## 26. Evolução para Cloud

Depois do MVP, a aplicação poderá ser hospedada em AWS ou DigitalOcean.

```text
                  Internet
                     │
                     ▼
              ┌──────────────┐
              │ Load Balancer│
              └──────┬───────┘
             ┌───────┴────────┐
             ▼                ▼
      ┌────────────┐   ┌────────────┐
      │ API        │   │ API        │
      │ Container  │   │ Container  │
      └─────┬──────┘   └─────┬──────┘
            └───────┬────────┘
                    ▼
             ┌──────────────┐
             │ PostgreSQL   │
             └──────────────┘
```

---

## 27. Observabilidade

```text
TaskFlow ── Logs / Metrics / Health Checks ──► Prometheus ──► Grafana
```

Métricas possíveis:

- Número de requisições
- Latência
- Taxa de erro
- CPU
- Memória
- Número de tarefas criadas
- Número de tarefas concluídas

---

## 28. Kubernetes

Kubernetes será considerado uma evolução posterior (não faz parte do MVP).

```text
Kubernetes Cluster
│
├── Frontend Pod
├── API Pod
├── PostgreSQL
└── MongoDB
```

O projeto poderá posteriormente utilizar Rancher para gerenciamento do cluster.

---

## 29. Roadmap

### Fase 1 — Backend

- [x] Criar solution .NET
- [x] Criar Domain
- [x] Criar Application
- [x] Criar Infrastructure
- [x] Criar API
- [x] Criar entidade Project
- [x] Criar entidade TaskItem
- [ ] Configurar Entity Framework Core
- [ ] Configurar PostgreSQL
- [ ] Criar migrations
- [ ] Implementar CRUD de Projects
- [ ] Implementar CRUD de Tasks
- [x] Configurar Swagger

### Fase 2 — Frontend

- [ ] Criar projeto React
- [ ] Configurar TypeScript
- [ ] Configurar comunicação com API
- [ ] Criar Login
- [ ] Criar Dashboard
- [ ] Criar Projects
- [ ] Criar Project Details
- [ ] Criar Kanban
- [ ] Criar Task Card
- [ ] Implementar criação de tarefas
- [ ] Implementar alteração de status

### Fase 3 — Qualidade

- [ ] Unit Tests
- [ ] Integration Tests
- [ ] Validation
- [ ] Global Exception Handler
- [ ] Structured Logging
- [x] Health Checks
- [ ] Documentação da API

### Fase 4 — Docker

- [ ] Dockerfile do backend
- [ ] Dockerfile do frontend
- [ ] Docker Compose
- [x] PostgreSQL container
- [ ] Network
- [x] Volumes
- [ ] Environment Variables

### Fase 5 — GitLab CI/CD

- [ ] GitLab Repository
- [x] Build pipeline
- [x] Test pipeline
- [ ] Docker build
- [ ] Docker Registry
- [ ] Deployment
- [ ] Environment variables
- [ ] Secrets

### Fase 6 — Cloud

- [ ] Criar infraestrutura
- [ ] Deploy da API
- [ ] Deploy do frontend
- [ ] PostgreSQL
- [ ] HTTPS
- [ ] Domain
- [ ] Backup
- [ ] Monitoring

### Fase 7 — Evolução

- [ ] MongoDB
- [ ] Audit Events
- [ ] Prometheus
- [ ] Grafana
- [ ] Kubernetes
- [ ] Rancher
- [ ] Horizontal Scaling

---

## 30. Requisitos da vaga x TaskFlow

| Requisito       | Implementação                  |
| --------------- | ------------------------------ |
| .NET Core       | ASP.NET Core Web API           |
| C#              | Backend                        |
| JavaScript      | React / TypeScript             |
| React           | Frontend                       |
| PostgreSQL      | Banco principal                |
| SQL             | Entity Framework Core          |
| MongoDB         | Auditoria como evolução        |
| Git             | Versionamento                  |
| GitLab          | Repositório e CI/CD            |
| Docker          | Containerização                |
| CI/CD           | GitLab CI/CD                   |
| AWS             | Deploy futuro                  |
| DigitalOcean    | Alternativa de deploy          |
| Kubernetes      | Evolução                       |
| Rancher         | Gerenciamento do cluster       |
| Testes          | Unit + Integration Tests       |
| Arquitetura     | Clean Architecture             |
| Observabilidade | Logs + Metrics + Health Checks |

---

## 31. Decisões arquiteturais

### Por que PostgreSQL?

O domínio possui relacionamentos fortes entre usuários, projetos e tarefas. Um banco relacional permite representar essas relações de forma natural e utilizar transações e constraints.

### Por que MongoDB?

MongoDB não será utilizado para substituir o PostgreSQL. Sua utilização será avaliada para dados de auditoria e eventos que possuem estrutura documental e podem evoluir independentemente do modelo transacional principal.

### Por que ASP.NET Core?

O ASP.NET Core fornece:

- Performance
- Dependency Injection
- Middleware
- REST APIs
- Entity Framework Core
- Authentication/Authorization
- Health Checks
- Ecossistema maduro para aplicações corporativas

### Por que React?

React permite construir uma interface moderna e componentizada, além de atender ao requisito de JavaScript/React da stack proposta.

### Por que Docker?

Docker permite padronizar o ambiente de execução e reduzir diferenças entre desenvolvimento, testes e produção.

### Por que GitLab CI/CD?

O GitLab permite integrar Git, build, testes, Docker, registry e deployment em um único pipeline.

---

## 32. Objetivo para a entrevista

O projeto deve permitir demonstrar conhecimentos sobre:

- **Backend:** APIs REST, C#, ASP.NET Core, Entity Framework, PostgreSQL, arquitetura em camadas, Dependency Injection, validação, tratamento de erros
- **Frontend:** React, TypeScript, componentização, consumo de APIs, gerenciamento de estado
- **DevOps:** Git, GitLab, Docker, Docker Compose, CI/CD, Linux
- **Cloud:** containers, deploy, environment variables, secrets, health checks, observabilidade, escalabilidade

---

## 33. Estratégia de desenvolvimento

O projeto será desenvolvido incrementalmente. A prioridade será:

1. Aplicação funcionando
2. Arquitetura organizada
3. Testes
4. Docker
5. CI/CD
6. Cloud
7. Observabilidade
8. Kubernetes

A ideia é evitar adicionar tecnologias apenas para aumentar a quantidade de ferramentas utilizadas. Cada tecnologia deverá possuir uma justificativa técnica.

---

## 34. MVP

O MVP será considerado concluído quando for possível:

- [x] Executar PostgreSQL via Docker
- [x] Executar a API ASP.NET Core
- [ ] Criar projetos
- [ ] Listar projetos
- [ ] Atualizar projetos
- [ ] Excluir projetos
- [ ] Criar tarefas
- [ ] Listar tarefas
- [ ] Atualizar tarefas
- [ ] Alterar status das tarefas
- [ ] Executar React
- [ ] Visualizar projetos no frontend
- [ ] Visualizar tarefas no frontend
- [ ] Utilizar o Kanban
- [ ] Executar testes
- [ ] Executar tudo através de Docker Compose
- [ ] Executar pipeline básico no GitLab

---

## 35. Próximo passo

A solution já foi criada:

```text
TaskFlow.sln
│
├── TaskFlow.Api
├── TaskFlow.Application
├── TaskFlow.Domain
└── TaskFlow.Infrastructure
```

Ordem de implementação:

```text
Domain → Infrastructure → Application → API → React → Docker → GitLab CI/CD
```

Próximo passo: configurar o Entity Framework Core com PostgreSQL na camada `TaskFlow.Infrastructure` e criar a primeira migration.

O objetivo é terminar cada etapa com uma aplicação funcional antes de avançar para a próxima.
