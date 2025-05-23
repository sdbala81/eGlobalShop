FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8000

ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_HTTP_PORTS=8000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]

COPY ["Customers/WebApi/WebApi.csproj", "src/Customers/WebApi/"]
COPY ["Customers/Infrastructure/Infrastructure.csproj", "src/Customers/Infrastructure/"]
COPY ["Customers/Domain/Domain.csproj", "src/Customers/Domain/"]
COPY ["Customers/Application/Application.csproj", "src/Customers/Application/"]

COPY ["Shared/SharedKernel/SharedKernel.csproj", "src/Shared/SharedKernel/"]
COPY ["Shared/WebApi.Helpers/WebApi.Helpers.csproj", "src/Shared/WebApi.Helpers/"]
COPY ["Shared/Application.Helpers/Application.Helpers.csproj", "src/Shared/Application.Helpers/"]
COPY ["Shared/Nats.Helpers/Nats.Helpers.csproj", "src/Shared/Nats.Helpers/"]

RUN dotnet restore "./src/Customers/WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/Customers/WebApi/"
RUN dotnet build "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ElementLogiq.eGlobalShop.Customers.WebApi.dll"]