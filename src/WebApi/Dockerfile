FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
LABEL maintainer="Jonathan Peris"
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG AOT=false
ARG Trim=true
ARG ExtraOptimize=false
ARG BUILD_CONFIGURATION=Release

# Install necessary packages in one layer
RUN apt-get update && apt-get install -y --no-install-recommends clang zlib1g-dev && rm -rf /var/lib/apt/lists/*

WORKDIR /src

# Copy project files
COPY ["src/WebApi/WebApi.csproj", "src/WebApi/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/WebApi/WebApi.csproj" -p:Configuration=${BUILD_CONFIGURATION} -p:AOT=${AOT} -p:Trim=${Trim}

# Copy remaining source files
COPY . .

WORKDIR "/src/src/WebApi"

# Build the project
RUN dotnet build "WebApi.csproj" -c $BUILD_CONFIGURATION

FROM build AS publish

# Publish the application
RUN dotnet publish "WebApi.csproj" -r linux-arm64 --no-restore -c $BUILD_CONFIGURATION -p:AOT=${AOT} -p:Trim=${Trim} -p:ExtraOptimize=${ExtraOptimize} -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime-deps:8.0 AS final

WORKDIR /app
EXPOSE 8080

# Copy the published application
COPY --from=publish /app/publish .
ENTRYPOINT ["./WebApi"]
