# Use the official .NET SDK image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base

WORKDIR /src

# Copy solution file
COPY ../LTS.Candela.API.sln ./

# Copy API and test project folders
COPY ../LTS.Candela.API/LTS.Candela.API ./LTS.Candela.API
COPY . ./LTS.Candela.API.Tests

# Restore dependencies
RUN dotnet restore LTS.Candela.API.sln

# Build the test project
RUN dotnet build LTS.Candela.API.Tests/LTS.Candela.API.Tests.csproj --no-restore -c Release

# Run tests with code coverage
CMD ["dotnet", "test", "LTS.Candela.API.Tests/LTS.Candela.API.Tests.csproj", "--no-build", "-c", "Release", "--collect:XPlat Code Coverage", "--logger:trx"]
