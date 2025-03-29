FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

LABEL maintainer="Jonathan Peris"

USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    clang zlib1g-dev \
    && rm -rf /var/lib/apt/lists/* \
    && apt-get clean

ARG AOT
ARG TRIM
ARG EXTRA_OPTIMIZE
ARG BUILD_CONFIGURATION

WORKDIR /src

COPY ["IdentityApi/IdentityApi.csproj", "IdentityApi/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore "./IdentityApi/IdentityApi.csproj" -p:Configuration=${BUILD_CONFIGURATION} -p:AOT=${AOT} -p:Trim=${TRIM}

COPY . .

WORKDIR "/src/IdentityApi"

RUN dotnet build "IdentityApi.csproj" -c $BUILD_CONFIGURATION -p:AOT=${AOT} -p:Trim=${TRIM} -p:ExtraOptimize=${EXTRA_OPTIMIZE} -o /app/publish

FROM build AS publish

RUN dotnet publish "IdentityApi.csproj" --no-restore -c $BUILD_CONFIGURATION -p:AOT=${AOT} -p:Trim=${TRIM} -p:ExtraOptimize=${EXTRA_OPTIMIZE} -o /app/publish

FROM base AS final

WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5010
EXPOSE 5010

COPY --from=publish /app/publish .
ENTRYPOINT ["./IdentityApi"]