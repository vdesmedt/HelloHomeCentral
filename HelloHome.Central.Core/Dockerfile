FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HelloHome.Central.Core/HelloHome.Central.Core.csproj", "HelloHome.Central.Core/"]
RUN dotnet restore "HelloHome.Central.Core/HelloHome.Central.Core.csproj"
COPY . .
WORKDIR "/src/HelloHome.Central.Core"
RUN dotnet build "./HelloHome.Central.Core.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HelloHome.Central.Core.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloHome.Central.Core.dll"]
