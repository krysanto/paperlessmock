#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Container we use for final publish
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build container
FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build

# Copy the code into the container
WORKDIR /src
COPY ["src/Paperless.rest/Paperless.rest.csproj", "Paperless.rest/"]

# NuGet restore
RUN dotnet restore "Paperless.rest/Paperless.rest.csproj"
COPY ["src/Paperless.rest/", "Paperless.rest/"]

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