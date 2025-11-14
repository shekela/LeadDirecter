# Use the official .NET 8 runtime as base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything and restore
COPY ["LeadDirecter.WebApi/LeadDirecter.WebApi.csproj", "LeadDirecter.WebApi/"]
COPY ["LeadDirecter.Data/LeadDirecter.Data.csproj", "LeadDirecter.Data/"]
COPY ["LeadDirecter.Model/LeadDirecter.Model.csproj", "LeadDirecter.Model/"]
COPY ["LeadDirecter.Shared/LeadDirecter.Shared.csproj", "LeadDirecter.Shared/"]
RUN dotnet restore "LeadDirecter.WebApi/LeadDirecter.WebApi.csproj"

# Copy all sources and build
COPY . .
WORKDIR "/src/LeadDirecter.WebApi"
RUN dotnet publish "LeadDirecter.WebApi.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LeadDirecter.WebApi.dll"]
