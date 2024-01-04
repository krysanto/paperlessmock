#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8001

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Paperless.services/Paperless.services.csproj", "Paperless.services/"]
COPY ["Paperless.rabbitmq/Paperless.rabbitmq.csproj", "Paperless.rabbitmq/"]
RUN dotnet restore "Paperless.services/Paperless.services.csproj"
COPY . .
WORKDIR "/src/Paperless.services"
RUN dotnet build "Paperless.services.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Paperless.services.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Paperless.services.dll"]