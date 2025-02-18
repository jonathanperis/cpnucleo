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
ARG Trim=true
ARG ExtraOptimize=false
ARG BuildArchitecture=linux-arm64
ARG BuildConfiguration=Release

WORKDIR /src

COPY ["src/IdentityApi/IdentityApi.csproj", "src/IdentityApi/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]

RUN dotnet restore -r $BuildArchitecture "src/IdentityApi/IdentityApi.csproj" -p:Configuration=${BuildConfiguration} -p:AOT=${AOT} -p:Trim=${Trim}

COPY . .

WORKDIR "/src/src/IdentityApi"

FROM build AS publish

RUN dotnet publish -r $BuildArchitecture "IdentityApi.csproj" --no-restore -c $BuildConfiguration -p:AOT=${AOT} -p:Trim=${Trim} -p:ExtraOptimize=${ExtraOptimize} -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0 AS final

WORKDIR /app
EXPOSE 8080

COPY --from=publish /app/publish .
ENTRYPOINT ["./IdentityApi"]