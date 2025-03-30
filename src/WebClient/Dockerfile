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

COPY ["WebClient/WebClient.csproj", "WebClient/"]

RUN dotnet restore "./WebClient/WebClient.csproj" -p:Configuration=${BUILD_CONFIGURATION} -p:AOT=${AOT} -p:Trim=${TRIM}

COPY . .

WORKDIR "/src/WebClient"

RUN dotnet build "WebClient.csproj" -c $BUILD_CONFIGURATION -p:AOT=${AOT} -p:Trim=${TRIM} -p:ExtraOptimize=${EXTRA_OPTIMIZE} -o /app/build

FROM build AS publish

RUN dotnet publish "WebClient.csproj" --no-restore -c $BUILD_CONFIGURATION -p:AOT=${AOT} -p:Trim=${TRIM} -p:ExtraOptimize=${EXTRA_OPTIMIZE} -o /app/publish

FROM base AS final

WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5030
EXPOSE 5030

COPY --from=publish /app/publish .
ENTRYPOINT ["./WebClient"]