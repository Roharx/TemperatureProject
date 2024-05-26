# Use the official .NET 8.0 runtime as a parent image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
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

# Copy all the source files and build the application
COPY . .
WORKDIR "/src/api"
RUN dotnet build "api.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "api.csproj" -c Release -o /app/publish

# Build the runtime image using the base stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "api.dll"]