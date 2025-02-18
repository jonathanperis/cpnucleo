FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

LABEL maintainer="Jonathan Peris"

USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    clang zlib1g-dev

ARG AOT=false
ARG TRIM=true
ARG EXTRA_OPTIMIZE=false
ARG BUILD_ARCHITECTURE=linux-arm64
ARG BUILD_CONFIGURATION=Release

# ARG ConnectionStrings__DefaultConnection

WORKDIR /src

COPY ["src/WebApi/WebApi.csproj", "src/WebApi/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]

RUN dotnet restore -r $BUILD_ARCHITECTURE "src/WebApi/WebApi.csproj" -p:Configuration=${BUILD_CONFIGURATION} -p:AOT=${AOT} -p:Trim=${TRIM}

COPY . .

WORKDIR "/src/src/WebApi"

# RUN dotnet user-secrets set "ConnectionStrings__DefaultConnection" "${ConnectionStrings__DefaultConnection}"

FROM build AS publish

RUN dotnet publish -r $BUILD_ARCHITECTURE "WebApi.csproj" --no-restore -c $BUILD_CONFIGURATION -p:AOT=${AOT} -p:Trim=${TRIM} -p:ExtraOptimize=${EXTRA_OPTIMIZE} -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0 AS final

WORKDIR /app
EXPOSE 8080

COPY --from=publish /app/publish .
ENTRYPOINT ["./WebApi"]