# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# Copy the solution file and all project files
COPY ["tempProjNew.sln", "."]
COPY ["api/api.csproj", "api/"]
COPY ["service/service.csproj", "service/"]
COPY ["infrastructure/infrastructure.csproj", "infrastructure/"]
COPY ["tests/tests.csproj", "tests/"]
COPY ["exceptions/exceptions.csproj", "exceptions/"]

# Restore dependencies for all projects
RUN dotnet restore

# Copy the rest of the source files
COPY . .

# Build the entire solution
RUN dotnet build "tempProjNew.sln" -c Release -o /app/build

# Publish the API project
RUN dotnet publish "api/api.csproj" -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build-env /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "api.dll"]
