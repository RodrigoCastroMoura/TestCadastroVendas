# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Definir o diretório de trabalho
WORKDIR /src

# Copiar os arquivos de projeto
COPY ["src/TestCadastroVendas.Api/TestCadastroVendas.Api.csproj", "TestCadastroVendas.Api/"]
COPY ["src/TestCadastroVendas.Domain/TestCadastroVendas.Domain.csproj", "TestCadastroVendas.Domain/"]
COPY ["src/TestCadastroVendas.Infra/TestCadastroVendas.Infra.csproj", "TestCadastroVendas.Infra/"]


# Restaurar dependências
RUN dotnet restore "TestCadastroVendas.Api/TestCadastroVendas.Api.csproj"

# Copiar o restante do código
COPY src/ .

# Construir a aplicação
WORKDIR "/src/TestCadastroVendas.Api"
RUN dotnet build "TestCadastroVendas.Api.csproj" -c Release -o /app/build

# Publicar a aplicação
RUN dotnet publish "TestCadastroVendas.Api.csproj" -c Release -o /app/publish

# Etapa de criação do banco de dados
FROM alpine:latest AS db

# Instalar o sqlite3
RUN apk update && apk add sqlite
WORKDIR /app
COPY create_table.sql .

# Comando para criar o banco de dados e a tabela
RUN sqlite3 mydatabase.db < create_table.sql

# Etapa final - runtime
FROM base AS final
WORKDIR /app

# Copiar banco de dados criado
COPY --from=db /app/mydatabase.db .

# Copiar arquivos publicados do estágio de build
COPY --from=build /app/publish .

# Configurar variáveis de ambiente
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:80

# Expor a porta 80
EXPOSE 80

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "TestCadastroVendas.Api.dll"]
