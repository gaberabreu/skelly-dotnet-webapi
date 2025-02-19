FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Skelly.WebApi.Presentation/Skelly.WebApi.Presentation.csproj", "src/Skelly.WebApi.Presentation/"]
RUN dotnet restore "./src/Skelly.WebApi.Presentation/Skelly.WebApi.Presentation.csproj"
COPY ["src/Skelly.WebApi.Presentation/", "src/Skelly.WebApi.Presentation/"]
WORKDIR "/src/src/Skelly.WebApi.Presentation"
RUN dotnet build "./Skelly.WebApi.Presentation.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Skelly.WebApi.Presentation.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
RUN useradd -m appuser
USER appuser
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Skelly.WebApi.Presentation.dll"]