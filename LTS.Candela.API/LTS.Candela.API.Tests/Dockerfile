# Use .NET 9 SDK for build and test
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LTS.Candela.API.Tests.csproj", "."]
RUN dotnet restore "./LTS.Candela.API.Tests.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./LTS.Candela.API.Tests.csproj" -c $BUILD_CONFIGURATION

# Test stage
FROM build AS test
ARG BUILD_CONFIGURATION=Release
CMD ["dotnet", "test", "./LTS.Candela.API.Tests.csproj", "-c", "Release", "--no-build", "--logger:trx"]
