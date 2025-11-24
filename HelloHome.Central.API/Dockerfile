FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "./"]
COPY ["HelloHome.Central.API/HelloHome.Central.API.csproj", "HelloHome.Central.API/"]
RUN dotnet restore "HelloHome.Central.API/HelloHome.Central.API.csproj"
COPY . .
WORKDIR "/src/HelloHome.Central.API"
RUN dotnet build "HelloHome.Central.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HelloHome.Central.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
RUN echo "==== List of files in app ===" && ls /app -al
RUN echo "==== List of files in app/publish ===" && ls /app/publish -al

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:8080"]
