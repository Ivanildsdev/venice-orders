# Venice Orders - Microserviço de Pedidos

## Sobre o Projeto

Venice Orders é um microserviço API REST desenvolvido em .NET 8 para gerenciamento de pedidos, integrando múltiplas tecnologias seguindo boas práticas de arquitetura e design.

## Arquitetura

O projeto segue os princípios de **Clean Architecture** com separação clara de responsabilidades em camadas:

```
Venice.Orders.Api (Presentation)
    ↓
Venice.Orders.Application (Use Cases)
    ↓
Venice.Orders.Domain (Business Logic)
    ↑
Venice.Orders.Infrastructure (Data Access, External Services)
```

### Camadas

- **Venice.Orders.Domain**: Camada mais interna, contém entidades, value objects, enums, eventos e interfaces de repositórios. **Sem dependências externas**.
- **Venice.Orders.Application**: Contém use cases (commands/queries), DTOs, mappers e validações. Depende apenas do Domain.
- **Venice.Orders.Infrastructure**: Implementações concretas de repositórios, serviços externos (SQL Server, MongoDB, RabbitMQ, Redis). Depende do Domain e Application.
- **Venice.Orders.Api**: Controllers REST, middleware, configuração. Depende de todas as outras camadas.

## Decisões Técnicas e Arquiteturais

### Arquitetura e Padrões de Design

#### Clean Architecture + Domain-Driven Design (DDD)

- **Separação em 4 camadas**: Domain, Application, Infrastructure e Api
- **Independência de frameworks**: Domain não depende de nenhuma tecnologia externa
- **Domínio rico**: Lógica de negócio encapsulada nas entidades (`Order`, `OrderItem`)
- **Entidades base**: Classe abstrata `Entity` com campos comuns (`Id`, `CreatedAt`, `UpdatedAt`)
- **Eventos de domínio**: `OrderCreatedEvent` para comunicação assíncrona

#### CQRS (Command Query Responsibility Segregation)

- **Separação de responsabilidades**: Commands para escrita, Queries para leitura
- **MediatR**: Implementação do padrão Mediator para desacoplamento
- **Otimizações independentes**: Cache aplicado apenas em queries
- **Pipeline behaviors**: `ValidationBehavior` para validação automática de requests

#### Repository Pattern + Unit of Work

- **Abstração de dados**: Interfaces no Domain, implementações na Infrastructure
- **Coordenação de transações**: `UnitOfWork` coordena operações entre SQL Server e MongoDB
- **Múltiplos bancos**: Suporte a armazenamento híbrido com transações distribuídas

#### Notification Pattern

- **Eventos assíncronos**: Eventos de domínio transformados em notificações MediatR
- **Handlers múltiplos**: Cada notificação pode ter vários handlers
- **Desacoplamento**: Cache invalidation e RabbitMQ publishing separados

### Armazenamento Híbrido

#### SQL Server Express

- **Dados principais**: Entidade `Order` armazenada em SQL Server
- **Garantias ACID**: Transações para consistência de dados
- **EF Core**: Migrations automáticas na inicialização
- **Consultas complexas**: Suporte a queries relacionais avançadas

#### MongoDB

- **Itens do pedido**: Entidade `OrderItem` armazenada em MongoDB
- **Escalabilidade horizontal**: Estrutura NoSQL para crescimento
- **Flexibilidade**: Schema dinâmico para evolução de dados
- **Índices**: Criação automática de índices otimizados

#### Redis

- **Cache distribuído**: Melhoria de performance em queries frequentes
- **TTL configurável**: Expiração automática de dados em cache
- **Invalidação**: Cache limpo automaticamente em eventos de domínio

#### RabbitMQ

- **Message broker**: Eventos assíncronos (ex: `pedido.criado`)
- **Desacoplamento**: Comunicação entre serviços sem acoplamento direto
- **Resiliência**: Garantia de entrega de mensagens

### Validação e Segurança

#### FluentValidation

- **Validação em camadas**: Application layer com mensagens claras
- **Validação automática**: Pipeline behavior integrado ao MediatR
- **Model binding**: Validação automática de requisições HTTP

#### Autenticação e Autorização

- **JWT Bearer**: Autenticação stateless com tokens
- **Validação completa**: Issuer, Audience, Lifetime e SigningKey
- **Rate limiting**: Proteção contra brute force (5 tentativas/minuto no login)
- **Security headers**: X-Frame-Options, CSP, HSTS configurados

#### Tratamento de Erros

- **Middleware global**: `ExceptionHandlingMiddleware` para tratamento centralizado
- **Respostas padronizadas**: Estrutura consistente de erros
- **Logging estruturado**: Serilog com logs detalhados
- **Segurança**: Detalhes de exceção apenas em desenvolvimento

### Organização de Código

#### Extension Methods

- **Configuração modular**: Extension methods em `Extensions/` para organização
- **Separação de responsabilidades**: Cada extensão tem uma responsabilidade específica
- **Manutenibilidade**: Código mais limpo e fácil de manter


## Tecnologias

- **.NET 8**: Framework principal
- **SQL Server**: Banco relacional para dados principais do pedido
- **MongoDB**: Banco NoSQL para itens do pedido
- **RabbitMQ**: Message broker para eventos assíncronos
- **Redis**: Cache distribuído
- **Entity Framework Core**: ORM para SQL Server
- **xUnit**: Framework de testes
- **JWT**: Autenticação e autorização

## Estrutura do Projeto

```
venice-orders/
│
├── src/                              # Código fonte
│   ├── Venice.Orders.Domain/         # Camada de domínio
│   │   ├── Entities/                 # Entidades (Order, OrderItem, Entity)
│   │   ├── Enums/                    # OrderStatus
│   │   ├── Events/                    # OrderCreatedEvent
│   │   ├── Interfaces/               # Contratos
│   │   │   ├── Repositories/         # IOrderRepository, IOrderItemRepository
│   │   │   └── Services/             # IMessageBus
│   │   └── Exceptions/               # DomainException, InvalidOrderException
│   │
│   ├── Venice.Orders.Application/    # Camada de aplicação
│   │   ├── Commands/                 # CreateOrderCommand + Handler + Validator
│   │   ├── Queries/                  # GetOrderByIdQuery + Handler
│   │   ├── DTOs/                     # OrderDto, OrderItemDto
│   │   ├── Mappings/                 # AutoMapper MappingProfile
│   │   ├── Notifications/            # OrderCreatedNotification + Handlers
│   │   ├── Behaviors/                # ValidationBehavior
│   │   ├── Exceptions/               # ValidationException, NotFoundException
│   │   ├── Interfaces/               # ICacheService, IMessageBus
│   │   └── Extensions/               # ServiceCollectionExtensions
│   │
│   ├── Venice.Orders.Infrastructure/ # Camada de infraestrutura
│   │   ├── Data/
│   │   │   ├── SqlServer/            # OrdersDbContext, OrderRepository
│   │   │   └── MongoDb/              # MongoDbContext, OrderItemRepository
│   │   ├── Cache/                     # RedisCacheService
│   │   ├── Messaging/                 # RabbitMqService
│   │   ├── UnitOfWork/                # UnitOfWork
│   │   └── Extensions/               # ServiceCollectionExtensions, WebApplicationExtensions
│   │
│   └── Venice.Orders.Api/            # Camada de apresentação
│       ├── Controllers/               # OrdersController, AuthController
│       ├── Middleware/                # ExceptionHandlingMiddleware, SecurityHeadersMiddleware
│       ├── Filters/                   # ValidationFilter
│       ├── Extensions/                # Extension methods para configuração
│       ├── Models/                    # CreateOrderRequest, LoginRequest, LoginResponse
│       └── Program.cs                 # Configuração da aplicação
│
├── tests/                             # Projetos de teste
│   ├── Venice.Orders.Tests/           # Testes unitários
│   │   ├── Unit/
│   │   │   ├── Domain/                # OrderTests
│   │   │   └── Application/           # CreateOrderCommandHandlerTests, CreateOrderCommandValidatorTests
│   │   └── Builders/                  # AutoMapperBuilder, OrderBuilder
│   │
│   └── Venice.Orders.IntegrationTests/ # Testes de integração
│       ├── Controllers/               # OrdersControllerTests, AuthControllerTests
│       └── Helpers/                   # IntegrationTestBase, TestAuthHandler, TokenBuilder
│
├── docker-compose.yml                 # Orquestração Docker
├── Dockerfile                          # Build da API
├── .dockerignore                      # Arquivos ignorados no build
└── README.md                           # Este arquivo
```

## Status do Projeto

### Funcionalidades Implementadas

-  **Clean Architecture** com 4 camadas bem definidas e extension methods organizados
-  **Domain-Driven Design** com entidades ricas, eventos de domínio e classe base `Entity`
-  **CQRS** com MediatR (Commands e Queries separados)
-  **Repository Pattern** + Unit of Work para múltiplos bancos
-  **Armazenamento Híbrido**: SQL Server (Orders) + MongoDB (OrderItems)
-  **Cache Distribuído** com Redis e implementação in-memory para testes
-  **Message Broker** com RabbitMQ para eventos assíncronos
-  **Autenticação JWT** com validação completa e rate limiting
-  **Validação** com FluentValidation em múltiplas camadas
-  **Swagger/OpenAPI** com documentação completa e autenticação JWT
-  **Health Checks** para todos os serviços externos
-  **Testes Unitários** e de Integração organizados na pasta `tests/`
-  **Migrations Automáticas** do EF Core na inicialização
-  **Security Headers** configurados (OWASP Top 10)
-  **Rate Limiting** global e específico para login
-  **Exception Handling** centralizado com middleware
-  **Logging Estruturado** com Serilog

##  Como Executar com Docker

### Pré-requisitos

- **Docker Desktop** instalado e rodando
- **.NET 8 SDK** (opcional, apenas para desenvolvimento local)
- **Git** (para clonar o repositório)

### Execução Completa com Docker Compose

#### 1. Clonar o Repositório

```bash
git clone https://github.com/Ivanildsdev/venice-orders.git
cd venice-orders
```

#### 2. Iniciar Todos os Serviços

```bash
docker-compose up -d --build
```

Este comando irá:

- **Construir a imagem** da API Venice Orders
- **Iniciar todos os containers**:
  - `venice-api` - API REST (porta 5069)
  - `venice-sqlserver` - SQL Server Express (porta 1433)
  - `venice-mongodb` - MongoDB (porta 27017)
  - `venice-redis` - Redis (porta 6379)
  - `venice-rabbitmq` - RabbitMQ (portas 5672 e 15672)

#### 3. Verificar Status dos Containers

```bash
docker-compose ps
```

Todos os containers devem estar com status `healthy`. A API pode levar até 60 segundos para ficar pronta.

#### 4. Verificar Logs

```bash
# Logs de todos os serviços
docker-compose logs -f

# Logs apenas da API
docker-compose logs -f api

# Logs de um serviço específico
docker-compose logs -f sqlserver
```

#### 5. Acessar a Aplicação

**Swagger UI**: http://localhost:5069

A aplicação irá automaticamente:

-  Aplicar migrations do EF Core no SQL Server
-  Criar índices no MongoDB
-  Configurar exchanges e queues no RabbitMQ
-  Configurar health checks para todos os serviços

#### 6. Autenticação

Para testar endpoints protegidos:

1. **Obter Token JWT**:

   ```bash
   curl -X POST http://localhost:5069/api/auth/login \
     -H "Content-Type: application/json" \
     -d '{"username":"testuser","password":"testpass"}'
   ```

2. **Usar o Token**:
   ```bash
   curl -X GET http://localhost:5069/api/orders/{id} \
     -H "Authorization: Bearer {seu_token_jwt}"
   ```

#### 7. Parar os Serviços

```bash
# Parar containers (mantém volumes)
docker-compose down

# Parar e remover volumes (limpa dados)
docker-compose down -v

# Parar e remover imagens também
docker-compose down -v --rmi all
```

### Configuração dos Serviços Docker

#### Connection Strings (Configuradas Automaticamente)

O `docker-compose.yml` configura automaticamente as connection strings para usar os nomes dos serviços Docker:

- **SQL Server**: `Server=sqlserver,1433`
- **MongoDB**: `mongodb://mongodb:27017`
- **Redis**: `redis:6379`
- **RabbitMQ**: `amqp://guest:guest@rabbitmq:5672`

#### Variáveis de Ambiente da API

Todas as configurações são passadas via variáveis de ambiente no `docker-compose.yml`:

- Connection strings para todos os serviços
- Configurações JWT
- Configurações MongoDB e RabbitMQ
- Ambiente: `Development`

### Acessar Interfaces de Gerenciamento

#### RabbitMQ Management UI

- **URL**: http://localhost:15672
- **Usuário**: `guest`
- **Senha**: `guest`

#### SQL Server

- **Servidor**: `localhost,1433`
- **Usuário**: `sa`
- **Senha**: `YourStrong@Passw0rd`
- **Database**: `VeniceOrders_Dev`
- **Ferramentas**: SQL Server Management Studio, Azure Data Studio, ou `sqlcmd`

#### MongoDB

- **Connection String**: `mongodb://localhost:27017`
- **Database**: `VeniceOrders_Dev`
- **Ferramentas**: MongoDB Compass ou `mongosh`

#### Redis

- **Host**: `localhost`
- **Porta**: `6379`
- **Ferramentas**: `redis-cli` ou RedisInsight

### Executar Testes

```bash
# Todos os testes
dotnet test

# Apenas testes unitários
dotnet test --filter "FullyQualifiedName~Unit"

# Apenas testes de integração (requer Docker rodando)
dotnet test --filter "FullyQualifiedName~Integration"

# Testes com output detalhado
dotnet test --verbosity normal
```


## Segurança

### Proteções Implementadas (OWASP Top 10)

-  **A01 - Broken Access Control**: Autenticação JWT obrigatória em todos os endpoints protegidos
-  **A02 - Cryptographic Failures**: HTTPS redirection, JWT com algoritmo seguro (HMAC SHA256)
-  **A03 - Injection**: Proteção contra SQL Injection (EF Core) e NoSQL Injection (MongoDB Driver tipado)
-  **A05 - Security Misconfiguration**: Security headers configurados (X-Frame-Options, X-Content-Type-Options, HSTS, CSP)
-  **A07 - Authentication Failures**: Rate limiting no endpoint de login (5 tentativas/minuto), validação completa de JWT
-  **A09 - Logging Failures**: Serilog com logging estruturado e health checks

### Medidas de Segurança

- **Validação de Entrada**: FluentValidation em todas as camadas
- **Rate Limiting**: Proteção contra brute force (login) e DoS (global: 100 req/min)
- **Security Headers**: X-Frame-Options, X-Content-Type-Options, X-XSS-Protection, HSTS, CSP
- **Tratamento de Erros**: Detalhes de exceção apenas em desenvolvimento
- **Swagger**: Disponível apenas em ambiente de desenvolvimento
- **Validação de Tamanho**: Limite de 100 itens por coleção em requisições

## Licença

Este projeto foi desenvolvido como teste técnico para processo seletivo Venice por Ivan Santos em 14/11/2025.
