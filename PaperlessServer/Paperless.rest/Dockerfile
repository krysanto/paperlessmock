#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Container we use for final publish
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 8000

# Build container
FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS build

# Copy the code into the container
WORKDIR /src
COPY ["Paperless.rest/Paperless.rest.csproj", "Paperless.rest/"]
COPY ["Paperless.rabbitmq/Paperless.rabbitmq.csproj", "Paperless.rabbitmq/"]
COPY ["Paperless.FileIO/Paperless.FileIO.csproj", "Paperless.FileIO/"]

# NuGet restore
RUN dotnet restore "Paperless.rest/Paperless.rest.csproj"
COPY ["Paperless.rest/", "Paperless.rest/"]
COPY ["Paperless.rabbitmq/", "Paperless.rabbitmq/"]
COPY ["Paperless.FileIO/", "Paperless.FileIO/"]

# Build the API
WORKDIR "Paperless.rest"
RUN dotnet build "Paperless.rest.csproj" -c Release -o /app/build

# Publish it
FROM build AS publish
RUN dotnet publish "Paperless.rest.csproj" -c Release -o /app/publish

# Make the final image for publishing
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Paperless.rest.dll"]
