#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Cpnucleo.MVC/Cpnucleo.MVC.csproj", "src/Cpnucleo.MVC/"]
COPY ["src/Cpnucleo.Infra.CrossCutting.Util/Cpnucleo.Infra.CrossCutting.Util.csproj", "src/Cpnucleo.Infra.CrossCutting.Util/"]
RUN dotnet restore "src/Cpnucleo.MVC/Cpnucleo.MVC.csproj"
COPY . .
WORKDIR "/src/src/Cpnucleo.MVC"
RUN dotnet build "Cpnucleo.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cpnucleo.MVC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cpnucleo.MVC.dll"]