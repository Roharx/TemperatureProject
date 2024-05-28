# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# Copy the solution file and project files
COPY ["tempProjNew.sln", "."]
COPY ["api/api.csproj", "api/"]
COPY ["service/service.csproj", "service/"]
COPY ["infrastructure/infrastructure.csproj", "infrastructure/"]
COPY ["tests/tests.csproj", "tests/"]
COPY ["exceptions/exceptions.csproj", "exceptions/"]

# Restore dependencies for all projects
RUN dotnet restore

# Copy the rest of the source files and build the application
COPY . .
WORKDIR "/src/api"
RUN dotnet build "api.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "api.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "api.dll"]
