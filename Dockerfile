# Etapa para instalar o SQLite e preparar o ambiente
FROM alpine:latest AS db

# Atualizar os pacotes e instalar o SQLite
RUN apk update && apk add sqlite

# Definir o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copiar o script SQL que cria o banco de dados para o contêiner
COPY create_table.sql .

# Executar o script SQL para criar o banco de dados
RUN sqlite3 mydatabase.db < create_table.sql

# Fim da etapa do banco de dados, preparando para o contêiner final
FROM alpine:latest

# Definir o diretório de trabalho dentro do contêiner
WORKDIR /app

# Copiar o banco de dados gerado no estágio anterior para o contêiner final
COPY --from=db /app/mydatabase.db .

# Expor a porta 80, caso queira rodar um servidor ou outro serviço (não utilizado aqui)
EXPOSE 80

# Comando para manter o contêiner em execução
CMD ["tail", "-f", "/dev/null"]
