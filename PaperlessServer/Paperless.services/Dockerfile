#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 8001

RUN apt-get update && apt-get install -y \
    imagemagick \
    ghostscript

FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS build
WORKDIR /src
COPY ["Paperless.services/Paperless.services.csproj", "Paperless.services/"]
COPY ["Paperless.rabbitmq/Paperless.rabbitmq.csproj", "Paperless.rabbitmq/"]
COPY ["Paperless.OCR/Paperless.OCR.csproj", "Paperless.OCR/"]

RUN apt-get update && apt-get install -y \
    imagemagick \
    ghostscript

RUN dotnet restore "Paperless.services/Paperless.services.csproj"
COPY ["Paperless.services/", "Paperless.services/"]
COPY ["Paperless.rabbitmq/", "Paperless.rabbitmq/"]
COPY ["Paperless.OCR/", "Paperless.OCR/"]
WORKDIR "Paperless.services"
RUN dotnet build "Paperless.services.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Paperless.services.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Paperless.services.dll"]