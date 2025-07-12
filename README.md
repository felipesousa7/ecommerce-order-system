# Sistema de Pedidos - E-commerce

Sistema completo de e-commerce desenvolvido com .NET 8 no backend e React/TypeScript no frontend, implementando autenticação JWT, gerenciamento de produtos e pedidos, com simulação de processos de negócio.

## 🚀 Tecnologias Utilizadas

### Backend
- **.NET 8** - Framework principal
- **Entity Framework Core** - ORM para PostgreSQL
- **PostgreSQL** - Banco de dados
- **JWT Authentication** - Autenticação e autorização
- **Swagger/OpenAPI** - Documentação da API
- **BCrypt** - Hash de senhas
- **Docker** - Containerização

### Frontend
- **React 18** - Framework principal
- **TypeScript** - Tipagem estática
- **Vite** - Build tool e dev server
- **Material-UI (MUI)** - Componentes de UI
- **Axios** - Cliente HTTP
- **React Router DOM** - Roteamento
- **React Query** - Gerenciamento de estado e cache
- **React Toastify** - Notificações

### Infraestrutura
- **Docker** - Containerização
- **Docker Compose** - Orquestração de containers

## 📋 Funcionalidades

### 🔐 Autenticação e Usuários
- Registro de novos usuários com validação
- Login com JWT (60 minutos de expiração)
- Proteção de rotas autenticadas
- Hash de senhas com BCrypt
- Claims estruturados no token

### 📦 Produtos
- Listagem de produtos disponíveis
- Produtos pré-cadastrados no banco de dados
- Validação de disponibilidade
- Imagens dos produtos
- Preços e descrições

### 🛒 Pedidos
- Criação de pedidos com múltiplos itens
- Cálculo automático de valores
- Listagem de pedidos do usuário
- Consulta de pedido por ID
- **Simulação de pagamento** (80% sucesso, 20% falha)
- **Simulação de reserva de estoque**
- **Status do pedido** com 7 estados diferentes:
  - Recebido
  - Aguardando Pagamento
  - Pagamento Aprovado
  - Pagamento Recusado
  - Estoque Reservado
  - Reserva de Estoque Cancelada
  - Erro

### 🎨 Frontend
- Interface responsiva com Material-UI
- Tela de login e registro
- Lista de produtos com botão de pedido
- Lista de pedidos com status em tempo real
- Navegação entre telas
- Tratamento de erros e feedback visual
- Loading states e validações

## 🏗️ Arquitetura

### Backend
- **Arquitetura em camadas**: Controllers → Services → Repositories
- **Repository Pattern**: Abstração da camada de dados
- **DTOs**: Transferência de dados tipada
- **Middleware**: Tratamento global de exceções e JWT
- **Validação**: Data Annotations e validações customizadas
- **Processamento assíncrono**: Pedidos processados em background
- **Logging**: ILogger para rastreamento de operações

### Frontend
- **Componentes reutilizáveis**: Card, Button, Form components
- **Gerenciamento de estado**: React Context + Hooks
- **Interceptors**: Tratamento automático de tokens JWT
- **Cache**: React Query para cache de dados
- **Roteamento**: Proteção de rotas com PrivateRoute
- **Feedback visual**: Toast notifications para todas as ações

## 🚀 Como Executar

### Pré-requisitos
- Docker
- Docker Compose

### Passos para Execução

1. **Clone o repositório:**
```bash
git clone [URL_DO_REPOSITÓRIO]
cd [NOME_DO_DIRETÓRIO]
```

2. **Execute o docker-compose:**
```bash
docker-compose up
```

3. **Acesse as aplicações:**
- **Frontend**: http://localhost:5173
- **Backend API**: http://localhost:5000
- **Swagger**: http://localhost:5000/swagger

### Estrutura dos Containers
- **PostgreSQL**: Banco de dados (porta 5432)
- **Backend**: API .NET (porta 5000)
- **Frontend**: React (porta 5173)

## 🔧 Configurações

### Variáveis de Ambiente
- **JWT Secret**: Configurado no appsettings
- **Connection String**: PostgreSQL via Docker
- **CORS**: Configurado para desenvolvimento

### Banco de Dados
- **Migrations**: Aplicadas automaticamente
- **Seeds**: Produtos pré-cadastrados
- **Relacionamentos**: User → Order → OrderItem → Product


## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.

## 👨‍💻 Autor

Desenvolvido como projeto de demonstração de habilidades em .NET 8 e React.

