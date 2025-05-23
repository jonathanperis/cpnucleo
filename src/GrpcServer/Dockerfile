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

COPY ["GrpcServer/GrpcServer.csproj", "GrpcServer/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore "./GrpcServer/GrpcServer.csproj" -p:Configuration=${BUILD_CONFIGURATION} -p:AOT=${AOT} -p:Trim=${TRIM}

COPY . .

WORKDIR "/src/GrpcServer"

RUN dotnet build "GrpcServer.csproj" -c $BUILD_CONFIGURATION -p:AOT=${AOT} -p:Trim=${TRIM} -p:ExtraOptimize=${EXTRA_OPTIMIZE} -o /app/publish

FROM build AS publish

RUN dotnet publish "GrpcServer.csproj" --no-restore -c $BUILD_CONFIGURATION -p:AOT=${AOT} -p:Trim=${TRIM} -p:ExtraOptimize=${EXTRA_OPTIMIZE} -o /app/publish

FROM base AS final

ARG ASPNETCORE_ENVIRONMENT
ARG DB_CONNECTION_STRING

WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5020
ENV ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}
ENV DB_CONNECTION_STRING=${DB_CONNECTION_STRING}
EXPOSE 5020

COPY --from=publish /app/publish .
ENTRYPOINT ["./GrpcServer"]