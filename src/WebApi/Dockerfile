FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

LABEL maintainer="Jonathan Peris"

USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    clang zlib1g-dev

ARG AOT
ARG TRIM
ARG EXTRA_OPTIMIZE
ARG BUILD_CONFIGURATION

# ARG DB_CONNECTION_STRING

WORKDIR /src

COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore "./WebApi/WebApi.csproj" -p:Configuration=${BUILD_CONFIGURATION} -p:AOT=${AOT} -p:Trim=${TRIM}

COPY . .

WORKDIR "/src/WebApi"

# RUN dotnet user-secrets set "ConnectionStrings__DefaultConnection" "${DB_CONNECTION_STRING}"

RUN dotnet build "WebApi.csproj" -c $BUILD_CONFIGURATION -p:AOT=${AOT} -p:Trim=${TRIM} -p:ExtraOptimize=${EXTRA_OPTIMIZE} -o /app/build

FROM build AS publish

RUN dotnet publish "WebApi.csproj" --no-restore -c $BUILD_CONFIGURATION -p:AOT=${AOT} -p:Trim=${TRIM} -p:ExtraOptimize=${EXTRA_OPTIMIZE} -o /app/publish

FROM base AS final

WORKDIR /app
EXPOSE 8080

COPY --from=publish /app/publish .
ENTRYPOINT ["./WebApi"]