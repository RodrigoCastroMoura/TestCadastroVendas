# TestCadastroVendas

## Descrição
TestCadastroVendas é uma aplicação .NET Core 6 que utiliza SQLite como banco de dados. Esta aplicação permite o cadastro de vendas e é configurada para rodar em um contêiner Docker.

## Pré-requisitos
Antes de começar, certifique-se de ter os seguintes softwares instalados em sua máquina:

- [Docker](https://www.docker.com/get-started)

## Estrutura do Projeto
- **src/**: Contém o código-fonte da aplicação
  - **TestCadastroVendas.Api/**: Projeto da API
  - **TestCadastroVendas.Domain/**: Camada de domínio
  - **TestCadastroVendas.Infra/**: Infraestrutura (repositórios, contexto de dados)
- **create_table.sql**: Script SQL para criar a tabela `Vendas e itensVendas`
- **Dockerfile**: Arquivo de configuração do Docker

## Configuração

### 1. Clone o repositório
Clone este repositório para sua máquina local usando o comando abaixo:

```bash
git https://github.com/RodrigoCastroMoura/TestCadastroVendas.git
cd TestCadastroVendas

```
## Comandos Docker

### 1. Construir a imagem Docker
```bash
docker build -t myproject_image .
```
### 2. Construir a Conterner Docker
```bash
docker run -d -p 5000:80 --name myproject_container ou myproject_image

curl http://localhost:5000/swagger/index.html

```