#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM vesteacr.azurecr.io/veste/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TemplateMinimalApi.API/TemplateMinimalApi.API.csproj", "TemplateMinimalApi.API/"]
COPY ["TemplateMinimalApi.Tests/TemplateMinimalApi.Tests.csproj", "TemplateMinimalApi.Tests/"]
COPY ["TemplateMinimalApi.Extensions/TemplateMinimalApi.Extensions.csproj", "TemplateMinimalApi.Extensions/"]
RUN dotnet restore "TemplateMinimalApi.API/TemplateMinimalApi.API.csproj"
COPY . .
WORKDIR "/src/TemplateMinimalApi.API"
RUN dotnet build "TemplateMinimalApi.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TemplateMinimalApi.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TemplateMinimalApi.API.dll"]