#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Cpnucleo.GRPC/Cpnucleo.GRPC.csproj", "src/Cpnucleo.GRPC/"]
COPY ["src/Cpnucleo.Infra.CrossCutting.Util/Cpnucleo.Infra.CrossCutting.Util.csproj", "src/Cpnucleo.Infra.CrossCutting.Util/"]
COPY ["src/Cpnucleo.Infra.CrossCutting.IoC/Cpnucleo.Infra.CrossCutting.IoC.csproj", "src/Cpnucleo.Infra.CrossCutting.IoC/"]
COPY ["src/Cpnucleo.Application/Cpnucleo.Application.csproj", "src/Cpnucleo.Application/"]
COPY ["src/Cpnucleo.Domain/Cpnucleo.Domain.csproj", "src/Cpnucleo.Domain/"]
COPY ["src/Cpnucleo.Infra.CrossCutting.Security/Cpnucleo.Infra.CrossCutting.Security.csproj", "src/Cpnucleo.Infra.CrossCutting.Security/"]
COPY ["src/Cpnucleo.Infra.Data/Cpnucleo.Infra.Data.csproj", "src/Cpnucleo.Infra.Data/"]
RUN dotnet restore "src/Cpnucleo.GRPC/Cpnucleo.GRPC.csproj"
COPY . .
WORKDIR "/src/src/Cpnucleo.GRPC"
RUN dotnet build "Cpnucleo.GRPC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cpnucleo.GRPC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cpnucleo.GRPC.dll"]