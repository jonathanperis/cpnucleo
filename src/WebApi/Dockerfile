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
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["src/WebApi/WebApi.csproj", "src/WebApi/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]

RUN dotnet restore -r linux-arm64 "src/WebApi/WebApi.csproj" -p:Configuration=${BUILD_CONFIGURATION} -p:AOT=${AOT} -p:Trim=${Trim}

COPY . .

WORKDIR "/src/src/WebApi"

FROM build AS publish

RUN dotnet publish -r linux-arm64 "WebApi.csproj" --no-restore -c $BUILD_CONFIGURATION -p:AOT=${AOT} -p:Trim=${Trim} -p:ExtraOptimize=${ExtraOptimize} -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0 AS final

WORKDIR /app
EXPOSE 8080

COPY --from=publish /app/publish .
ENTRYPOINT ["./WebApi"]