# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["sima_Bpjs_api.csproj", "./"]
RUN dotnet restore "sima_Bpjs_api.csproj"

# Copy all project files and build
COPY . .
RUN dotnet publish "sima_Bpjs_api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Install curl for healthcheck if needed
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

# Set environment defaults
ENV ASPNETCORE_URLS=http://+:8080
ENV PORT=8080
ENV ENABLE_SWAGGER=true

EXPOSE 8080

ENTRYPOINT ["dotnet", "sima_Bpjs_api.dll"]

