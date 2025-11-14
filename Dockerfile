FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Venice.Orders.Api/Venice.Orders.Api.csproj", "src/Venice.Orders.Api/"]
COPY ["src/Venice.Orders.Application/Venice.Orders.Application.csproj", "src/Venice.Orders.Application/"]
COPY ["src/Venice.Orders.Domain/Venice.Orders.Domain.csproj", "src/Venice.Orders.Domain/"]
COPY ["src/Venice.Orders.Infrastructure/Venice.Orders.Infrastructure.csproj", "src/Venice.Orders.Infrastructure/"]
RUN dotnet restore "src/Venice.Orders.Api/Venice.Orders.Api.csproj"
COPY . .
WORKDIR "/src/src/Venice.Orders.Api"
RUN dotnet build "Venice.Orders.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Venice.Orders.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN apt-get update && apt-get install -y wget && rm -rf /var/lib/apt/lists/*
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Venice.Orders.Api.dll"]

