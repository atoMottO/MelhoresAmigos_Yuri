# MelhoresAmigos_Yuri - EmpregaAI

## Membros
- Matteo Procare - 10418276
- Otto Mota - 10418170
- Renan Garrido - 10417093
- Rodrigo Roveratti - 10417090
- Tomy Boimel - 10417109
- Yuri Milliet - 10417884
  
## Sobre o Projeto

EmpregaAI é uma aplicação full-stack moderna projetada para simplificar e aprimorar a criação, edição e gerenciamento de currículos profissionais. A plataforma foi desenvolvida especialmente para auxiliar pessoas em situação de subemprego ou inseridas na informalidade do mercado de trabalho, ajudando-as a formatar seus currículos de forma profissional e informativa para os contratantes, aumentando suas chances de recolocação no mercado formal. 

---

## Principais Funcionalidades

* **Assistência por IA:** Geração e aprimoramento de textos, resumos e descrições usando a Groq API.
* **Editor Completo:** Interface intuitiva para criação e edição com visualização em tempo real.
* **Templates Modernos:** Diversos modelos profissionais e totalmente customizáveis.
* **Gerenciamento de Dados (CRUD):** Controle completo sobre informações pessoais, experiências, formação e habilidades.
* **Design Responsivo:** Interface adaptável para uma experiência fluida em **desktop** e **mobile**.
* **Exportação:** Download de currículos em **PDF**.

---

## Arquitetura

O projeto adota uma arquitetura full-stack moderna, com separação clara entre Frontend e Backend.

### Frontend
| Característica | Detalhes |
| :--- | :--- |
| **Framework** | Vue.js 3 (Composition API + Options API) |
| **Linguagem** | TypeScript |
| **Gerenciamento de Estado** | Pinia/Vuex |
| **Roteamento** | Vue Router |
| **Estilização** | CSS3 com Scoped Styles, Glassmorphism |
| **Requisições** | Axios |

### Backend
| Característica | Detalhes |
| :--- | :--- |
| **Framework** | ASP.NET Core (.NET 8) |
| **Linguagem** | C# |
| **ORM** | Entity Framework Core |
| **Banco de Dados** | PostgreSQL |
| **Autenticação** | JWT (JSON Web Tokens) |
| **API** | RESTful API |

### Integrações
* **Groq API:** Para consumo da API de Inteligência Artificial.
* **AWS:** Para hospedagem e deployment em produção.

---

## Começando

Siga os passos abaixo para configurar e rodar o projeto localmente.

### Pré-requisitos
Certifique-se de ter as seguintes ferramentas instaladas:
* Node.js **>= 18.x**
* .NET SDK **>= 8.0**
* PostgreSQL **>= 15**
* Git

### Instalação

#### 1. Clone o repositório
```bash
git clone [https://github.com/seu-usuario/EmpregaAI.git](https://github.com/seu-usuario/EmpregaAI.git)
cd EmpregaAI

```
#### 2. Configure o BackEnd (ASP.NET Core)
```bash
cd backend
dotnet restore
```
Crie o appsettings.json no diretório do backend
```bash
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=empregaai;Username=seu_usuario;Password=sua_senha"
  },
  "JwtSettings": {
    "SecretKey": "sua_chave_secreta_aqui",
    "Issuer": "EmpregaAI",
    "Audience": "EmpregaAI-Users",
    "ExpirationMinutes": 60
  }
}
```
Execute as migrations no terminal para criar o banco de dados
```bash
dotnet ef database update
```
Rode a aplicação
```bash
dotnet run
```
O backend estará rodando no http://localhost:5000

#### 3. Configure o FrontEnd (Vue.js + Typescript)
```bash
cd ../frontend
npm install
```
Crie o .env no diretório frontend com sua chave do grok para funcionamento do sistema de auxilio de descrição
```bash
VITE_GROQ_API_KEY=sua_chave_groq_aqui
```
Inicie o servidor
```bash
npm run dev
```
O frontend estará rodando no http://localhost:5173

### Estrutura do Projeto
```bash
EmpregaAI/
├── Backend/
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Data/
│   └── Migrations/
│
├── EmpregaAIVue/
│   ├── src/
│   │   ├── components/
│   │   ├── views/
│   │   ├── router/
│   │   ├── services/
│   │   └── store/
│   └── .env
│
└── README.md
```
