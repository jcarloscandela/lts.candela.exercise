# Use .NET 9 SDK for build and test
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution and project files
COPY ../LTS.Candela.API.sln ./
COPY ../LTS.Candela.API/LTS.Candela.API.csproj ./LTS.Candela.API/
COPY ./LTS.Candela.API.Tests.csproj ./

# Restore all projects
RUN dotnet restore "./LTS.Candela.API.sln"

# Copy the rest of the source code
COPY ../LTS.Candela.API ./LTS.Candela.API/
COPY . .

# Build the solution
RUN dotnet build "./LTS.Candela.API.sln" -c $BUILD_CONFIGURATION --no-restore

# Test stage with code coverage
FROM build AS test
ARG BUILD_CONFIGURATION=Release
CMD ["dotnet", "test", "./LTS.Candela.API.Tests.csproj", "-c", "Release", "--no-build", "--logger:trx", "--collect:XPlat Code Coverage"]
