FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Leverage layer caching: copy project files first and restore dependencies
COPY ["CookieAuthenticationExample/CookieAuthenticationExample.csproj", "CookieAuthenticationExample/"]

# Use BuildKit cache mounts to persist NuGet packages across builds
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore "CookieAuthenticationExample/CookieAuthenticationExample.csproj"

# Copy remaining source files
COPY . .
WORKDIR "/src/CookieAuthenticationExample"

# Publish directly without a separate build step to eliminate redundant layers
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish "CookieAuthenticationExample.csproj" \
        -c Release \
        -o /app/publish \
        /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

EXPOSE 8080

# Run as a non-privileged user 
USER $APP_UID

COPY --from=build --chown=$APP_UID:$APP_UID /app/publish .

ENTRYPOINT ["dotnet", "CookieAuthenticationExample.dll"]