#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Cpnucleo.RazorPages/Cpnucleo.RazorPages.csproj", "src/Cpnucleo.RazorPages/"]
COPY ["src/Cpnucleo.Infra.CrossCutting.Util/Cpnucleo.Infra.CrossCutting.Util.csproj", "src/Cpnucleo.Infra.CrossCutting.Util/"]
RUN dotnet restore "src/Cpnucleo.RazorPages/Cpnucleo.RazorPages.csproj"
COPY . .
WORKDIR "/src/src/Cpnucleo.RazorPages"
RUN dotnet build "Cpnucleo.RazorPages.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cpnucleo.RazorPages.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cpnucleo.RazorPages.dll"]