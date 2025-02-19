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
ARG BUILD_ARCHITECTURE
ARG BUILD_CONFIGURATION

WORKDIR /src

COPY ["src/IdentityApi/IdentityApi.csproj", "src/IdentityApi/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]

RUN dotnet restore -r $BUILD_ARCHITECTURE "src/IdentityApi/IdentityApi.csproj" -p:Configuration=${BUILD_CONFIGURATION} -p:AOT=${AOT} -p:Trim=${TRIM}

COPY . .

WORKDIR "/src/src/IdentityApi"

FROM build AS publish

RUN dotnet publish -r $BUILD_ARCHITECTURE "IdentityApi.csproj" --no-restore -c $BUILD_CONFIGURATION -p:AOT=${AOT} -p:Trim=${TRIM} -p:ExtraOptimize=${EXTRA_OPTIMIZE} -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0 AS final

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5050
EXPOSE 5050

COPY --from=publish /app/publish .
ENTRYPOINT ["./IdentityApi"]