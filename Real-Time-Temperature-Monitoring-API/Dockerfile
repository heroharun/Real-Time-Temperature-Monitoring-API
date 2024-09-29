FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Real-Time-Temperature-Monitoring-API/Real-Time-Temperature-Monitoring-API.csproj", "Real-Time-Temperature-Monitoring-API/"]
RUN dotnet restore "Real-Time-Temperature-Monitoring-API/Real-Time-Temperature-Monitoring-API.csproj"
COPY . .
WORKDIR "/src/Real-Time-Temperature-Monitoring-API"
RUN dotnet build "Real-Time-Temperature-Monitoring-API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Real-Time-Temperature-Monitoring-API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Real-Time-Temperature-Monitoring-API.dll"]
