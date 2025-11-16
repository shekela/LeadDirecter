# --- BASE RUNTIME IMAGE ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# --- BUILD IMAGE WITH SDK ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files
COPY ["LeadDirecter.WebApi/LeadDirecter.WebApi.csproj", "LeadDirecter.WebApi/"]
COPY ["LeadDirecter.Data/LeadDirecter.Data.csproj", "LeadDirecter.Data/"]
COPY ["LeadDirecter.Service/LeadDirecter.Service.csproj", "LeadDirecter.Service/"]
COPY ["LeadDirecter.Model/LeadDirecter.Model.csproj", "LeadDirecter.Model/"]
COPY ["LeadDirecter.Shared/LeadDirecter.Shared.csproj", "LeadDirecter.Shared/"]

RUN dotnet restore "LeadDirecter.WebApi/LeadDirecter.WebApi.csproj"

# Copy full source
COPY . .

WORKDIR "/src/LeadDirecter.WebApi"
RUN dotnet publish -c Release -o /app/publish

# --- FINAL RUNTIME IMAGE ---
FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "LeadDirecter.WebApi.dll"]
