#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ConsumerWebSite-ConsoleApp/B3Digitas.ConsumerWebSite.csproj", "ConsumerWebSite-ConsoleApp/"]
COPY ["Library/B3Digitas.Library.csproj", "Library/"]
RUN dotnet restore "./ConsumerWebSite-ConsoleApp/./B3Digitas.ConsumerWebSite.csproj"
COPY . .
WORKDIR "/src/ConsumerWebSite-ConsoleApp"
RUN dotnet build "./B3Digitas.ConsumerWebSite.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./B3Digitas.ConsumerWebSite.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "B3Digitas.ConsumerWebSite.dll"]