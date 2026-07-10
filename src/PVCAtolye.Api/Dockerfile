FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/PVCAtolye.Api/PVCAtolye.Api.csproj", "src/PVCAtolye.Api/"]
COPY ["src/PVCAtolye.Application/PVCAtolye.Application.csproj", "src/PVCAtolye.Application/"]
COPY ["src/PVCAtolye.Domain/PVCAtolye.Domain.csproj", "src/PVCAtolye.Domain/"]
COPY ["src/PVCAtolye.Infrastructure/PVCAtolye.Infrastructure.csproj", "src/PVCAtolye.Infrastructure/"]
RUN dotnet restore "src/PVCAtolye.Api/PVCAtolye.Api.csproj"
COPY . .
RUN dotnet publish "src/PVCAtolye.Api/PVCAtolye.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PVCAtolye.Api.dll"]
