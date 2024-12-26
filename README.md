# File Analysis API

Uma API robusta para análise de arquivos, integrada com múltiplos serviços de verificação de segurança. O projeto oferece um sistema completo de gerenciamento de usuários, autenticação via API Key e análise de arquivos para detecção de códigos maliciosos.

## 📋 Funcionalidades Principais

- Autenticação via API Key
- CRUD completo de usuários
- Rastreamento de uso da API por usuário

## 🛠️ Tecnologias Utilizadas

- .NET 7.0
- PostgreSQL
- Entity Framework Core
- Swagger/OpenAPI
- Clean Architecture
- RESTful API

## 📦 Pré-requisitos

- .NET SDK 7.0 ou superior
- PostgreSQL 13 ou superior
- Visual Studio 2022/VS Code ou editor de sua preferência
- Docker (opcional)

## 🚀 Como Executar

1. Clone o repositório
```bash
git clone https://github.com/seu-usuario/file-analysis-api.git
cd file-analysis-api
```

2. Configure o banco de dados PostgreSQL

Atualize o arquivo `appsettings.json` com suas credenciais:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=fileanalysis;Username=seu_usuario;Password=sua_senha"
  }
}
```

3. Execute as migrations
```bash
dotnet ef database update
```

4. Execute o projeto
```bash
dotnet run --project FileAnalysis.API
```

A API estará disponível em `https://localhost:5001` (ou `http://localhost:5000`)

### 🐳 Executando com Docker

1. Construa a imagem
```bash
docker build -t file-analysis-api .
```

2. Execute o container
```bash
docker-compose up -d
```

## 📝 Como Usar

### Criar um novo usuário

```http
POST /api/users
Content-Type: application/json

{
    "email": "usuario@exemplo.com",
    "password": "password",
    "active": true
}
```

### Analisar um arquivo

```http
POST /api/fileanalysis/analyze
X-API-Key: sua_api_key
Content-Type: multipart/form-data

file: [arquivo binário]
```

## 🤝 Como Contribuir

1. Faça um Fork do projeto
2. Crie uma branch para sua feature
   ```bash
   git checkout -b feature/MinhaFeature
   ```
3. Commit suas mudanças
   ```bash
   git commit -m 'Adicionando uma nova feature'
   ```
4. Push para a branch
   ```bash
   git push origin feature/MinhaFeature
   ```
5. Abra um Pull Request

### 📋 Guidelines para Contribuição

- Siga o padrão de código existente
- Adicione testes para novas funcionalidades
- Atualize a documentação quando necessário
- Verifique se todos os testes estão passando

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo [LICENSE](LICENSE) para mais detalhes.

## 📧 Contato

Para questões e sugestões, por favor abra uma issue no repositório.

---
⭐ Se este projeto te ajudou, considere dar uma estrela no GitHub!
