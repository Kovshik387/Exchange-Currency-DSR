#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Systems/Api/Exchange.ExchangeCache/Exchange.ExchangeCache.csproj", "Systems/Api/Exchange.ExchangeCache/"]
COPY ["Services/Exchange.Services.Cache/Exchange.Services.Cache.csproj", "Services/Exchange.Services.Cache/"]
COPY ["Services/Exchange.Services.Settings/Exchange.Services.Settings.csproj", "Services/Exchange.Services.Settings/"]
COPY ["Shared/Exchange.Common/Exchange.Common.csproj", "Shared/Exchange.Common/"]
RUN dotnet restore "./Systems/Api/Exchange.ExchangeCache/Exchange.ExchangeCache.csproj"
COPY . .
WORKDIR "/src/Systems/Api/Exchange.ExchangeCache"
RUN dotnet build "./Exchange.ExchangeCache.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Exchange.ExchangeCache.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exchange.ExchangeCache.dll"]