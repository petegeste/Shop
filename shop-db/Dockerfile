FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["shop-db.csproj", "."]
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet restore "shop-db.csproj"
COPY . .
RUN dotnet ef migrations bundle --self-contained

FROM postgres:11.5

LABEL author="Pete Gestewitz"
LABEL description="shop-db demo database."
LABEL version="1.0"

WORKDIR /init
COPY --from=build "/src/efbundle" "."
COPY --from=build "/src/Launch.sh" "."