FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["shop-db.csproj", "."]
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet restore "shop-db.csproj"
COPY . .

# Build a migration script, convert it to ASCII
RUN dotnet ef migrations script -o create_tables.sql
RUN iconv -f UTF-8 -t ASCII//TRANSLIT//IGNORE create_tables.sql -o create_tables2.sql
SHELL ["/bin/bash", "-c"]
RUN echo $'CREATE DATABASE shop_db;\n\\c shop_db\n' >> create.sql;
RUN cat create_tables2.sql >> create.sql

FROM postgres:11.5
LABEL author="Pete Gestewitz"
LABEL description="shop-db demo database."
LABEL version="1.0"

# Copy migration script to the init directory
WORKDIR /docker-entrypoint-initdb.d
COPY --from=build "/src/create.sql" "."