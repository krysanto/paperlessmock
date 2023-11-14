#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Container we use for final publish
FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build container
FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS build

# Copy the code into the container
WORKDIR /src
COPY ["Paperless.sl/Paperless.sl.csproj", "Paperless.sl/"]

# NuGet restore
RUN dotnet restore "Paperless.sl/Paperless.sl.csproj"
COPY ["Paperless.sl/", "Paperless.sl/"]

# Build the API
WORKDIR "Paperless.sl"
RUN dotnet build "Paperless.sl.csproj" -c Release -o /app/build

# Publish it
FROM build AS publish
RUN dotnet publish "Paperless.sl.csproj" -c Release -o /app/publish

# Make the final image for publishing
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Paperless.sl.dll"]
