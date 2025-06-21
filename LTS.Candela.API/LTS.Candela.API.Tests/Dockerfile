# Use the official .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copy solution and restore as distinct layers
COPY ../LTS.Candela.API.sln ./
COPY ../LTS.Candela.API/*.csproj ./LTS.Candela.API/
COPY *.csproj ./
COPY . ./

RUN dotnet restore ../LTS.Candela.API.sln

# Build the test project
RUN dotnet build LTS.Candela.API.Tests.csproj --no-restore -c Release

# Run tests with code coverage
CMD ["dotnet", "test", "LTS.Candela.API.Tests.csproj", "--no-build", "-c", "Release", "--collect:XPlat Code Coverage", "--logger:trx"]
