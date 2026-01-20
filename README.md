# 🖥️ Asset Management System

Sistema de Gerenciamento de Ativos de TI desenvolvido como teste técnico.

## 📋 Sobre o Projeto

Sistema para controlar a alocação de equipamentos (ativos) de TI como Notebooks, Monitores e Periféricos para colaboradores, substituindo o controle manual por planilhas.

### ⭐ Importante
O sistema **não permite** alocar um ativo que já esteja **"Em Uso"** ou **"Em Manutenção"**.

---

## 🛠️ Tecnologias Utilizadas

- **.NET 10** (Framework)
- **ASP.NET Core Web API** (Backend)
- **Blazor Server** (Frontend)
- **Entity Framework Core 8** (ORM)
- **SQL Server Express** (Banco de Dados)
- **xUnit + Moq** (Testes Unitários)

---

## 🏗️ Arquitetura

O projeto segue os princípios da **Clean Architecture**, dividido em 4 camadas:
```
AssetManagementSystem/
├── AssetManagementSystem.Domain          # Entidades e Interfaces
├── AssetManagementSystem.Application     # Regras de Negócio (Services)
├── AssetManagementSystem.Infrastructure  # Banco de Dados (EF Core)
├── AssetManagementSystem.API             # Controllers (API REST)
├── AssetManagementSystem.Web             # Frontend (Blazor)
└── AssetManagementSystem.Tests           # Testes Unitários
```

---

## ✅ Funcionalidades

### Usuários
- ✅ Cadastrar, editar e excluir usuários
- ✅ Ativar/Inativar usuários
- ✅ Validação de e-mail duplicado

### Ativos
- ✅ Cadastrar, editar e excluir ativos
- ✅ Tipos: Notebook, Monitor, Periférico
- ✅ Status: Disponível, Em Uso, Manutenção
- ✅ Enviar para manutenção / Liberar

### Alocações
- ✅ Alocar ativo para usuário
- ✅ Registrar devolução
- ✅ Histórico completo de alocações
- ✅ **Regra de Ouro**: Não permite alocar ativo "Em Uso" ou "Em Manutenção"

---

## 🚀 Como Executar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server Express](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (recomendado)

### Passo a Passo

1. **Clone o repositório**
```bash
   git clone https://github.com/SEU-USUARIO/AssetManagementSystem.git
   cd AssetManagementSystem
```

2. **Configure a string de conexão**
   
   No arquivo `AssetManagementSystem/appsettings.json`, ajuste a connection string se necessário:
```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=AssetManagementDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
   }
```

3. **Crie o banco de dados**
   
   No Visual Studio, abra o **Console do Gerenciador de Pacotes** e execute:
```
   Update-Database -StartupProject AssetManagementSystem
```

4. **Execute a aplicação**
   
   - Configure os projetos de inicialização:
     - Clique com botão direito na Solution → "Configurar Projetos de Inicialização"
     - Selecione "Vários projetos de inicialização"
     - Defina `AssetManagementSystem` e `AssetManagementSystem.Web` como "Iniciar"
   
   - Pressione **F5** para executar

5. **Acesse a aplicação**
   - **Frontend**: https://localhost:7275 (ou porta configurada)
   - **API (Swagger)**: https://localhost:7282/swagger

---

## 🧪 Testes Unitários

O projeto inclui testes unitários para as regras de negócio críticas.

### Executar os testes

No Visual Studio: **Testar** → **Executar Todos os Testes** (Ctrl+R, A)

### Testes implementados

| Teste | Descrição |
|-------|-----------|
| ✅ | Não permite alocar ativo "Em Uso" |
| ✅ | Não permite alocar ativo "Em Manutenção" |
| ✅ | Não permite alocar para usuário inativo |
| ✅ | Permite alocar ativo disponível |
| ✅ | Erro quando ativo não existe |
| ✅ | Erro quando usuário não existe |

---

## 📁 Estrutura do Banco de Dados

### Tabelas

- **Users**: Colaboradores que recebem os ativos
- **Assets**: Equipamentos de TI (notebooks, monitores, etc.)
- **Allocations**: Registro de alocações (histórico)

### Relacionamentos
```
Users (1) ←――――→ (N) Allocations (N) ←――――→ (1) Assets
```

---

## 👨‍💻 Autor

**João Costa**
- Email: joaomarcelocosta.sc@gmail.com

---
