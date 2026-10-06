# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/aspnet:10.0.12 AS base

LABEL maintainer="Jonathan Peris"

RUN apt-get update && apt-get install -y --no-install-recommends libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*

USER app
WORKDIR /app

# The SDK stage runs on the build machine's architecture and cross-publishes for
# TARGETARCH with an explicit runtime identifier. ReadyToRun (TRIM=true) supports
# this through the host Crossgen2 package, so local multi-arch builds avoid
# emulating the whole SDK. Native AOT (AOT=true) is an experiment that needs a
# native builder for the target architecture; the release runs native runners.
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0.401 AS build

ARG AOT=false
ARG TRIM=false
ARG EXTRA_OPTIMIZE=false
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH

RUN if [ "${AOT}" = "true" ]; then \
      apt-get update \
      && apt-get install -y --no-install-recommends clang zlib1g-dev \
      && rm -rf /var/lib/apt/lists/*; \
    fi

WORKDIR /src

COPY ["GrpcServer/GrpcServer.csproj", "GrpcServer/"]
COPY ["GrpcServer.Contracts/GrpcServer.Contracts.csproj", "GrpcServer.Contracts/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN case "${TARGETARCH}" in \
      amd64) rid=linux-x64 ;; \
      arm64) rid=linux-arm64 ;; \
      *) echo "Unsupported TARGETARCH '${TARGETARCH}'" >&2; exit 1 ;; \
    esac \
    && echo "${rid}" > /tmp/rid \
    && dotnet restore "./GrpcServer/GrpcServer.csproj" -r "${rid}" -p:Configuration=${BUILD_CONFIGURATION} -p:AOT=${AOT} -p:Trim=${TRIM}

COPY . .

# A single publish compiles once; there is no separate build pass.
RUN dotnet publish "./GrpcServer/GrpcServer.csproj" --no-restore -r "$(cat /tmp/rid)" -c ${BUILD_CONFIGURATION} -p:AOT=${AOT} -p:Trim=${TRIM} -p:ExtraOptimize=${EXTRA_OPTIMIZE} -o /app/publish

FROM base AS final

ARG ASPNETCORE_ENVIRONMENT=Production

WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5020;http://+:5021
ENV ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}
EXPOSE 5020
EXPOSE 5021
HEALTHCHECK --interval=1m --timeout=5s --start-period=1m --start-interval=10s --retries=3 CMD bash -ec "exec 3<>/dev/tcp/127.0.0.1/5021; printf 'GET /healthz HTTP/1.1\r\nHost: localhost\r\nConnection: close\r\n\r\n' >&3; head -n 1 <&3 | grep -q ' 200 '" || exit 1

COPY --from=build /app/publish .
ENTRYPOINT ["./GrpcServer"]
