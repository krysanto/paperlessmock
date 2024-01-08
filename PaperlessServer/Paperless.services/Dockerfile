#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-bookworm-slim AS base
ENV LD_LIBRARY_PATH=/app:$LD_LIBRARY_PATH
WORKDIR /app
EXPOSE 8001

RUN apt-get update && apt-get install -y \
    libleptonica-dev \
    libtesseract-dev \
    imagemagick \
    ghostscript \
    tesseract-ocr \
    libtesseract-dev \
    tesseract-ocr-eng \
    tesseract-ocr-deu

RUN mkdir -p /app/runtimes/linux-x64/native/
RUN ln -s /usr/lib/x86_64-linux-gnu/libdl.so.2 /usr/lib/x86_64-linux-gnu/libdl.so
RUN ln -s /usr/lib/x86_64-linux-gnu/libdl.so.2 /app/runtimes/linux-x64/native/libdl.so
RUN ln -s /usr/lib/x86_64-linux-gnu/libdl.so.2 /app/libdl.so
RUN mkdir /app/x64
RUN ln -s /usr/lib/x86_64-linux-gnu/liblept.so.5 /app/x64/libleptonica-1.82.0.so
RUN ln -s /usr/lib/x86_64-linux-gnu/libtesseract.so.5 /app/x64/libtesseract50.so

FROM mcr.microsoft.com/dotnet/sdk:7.0-bookworm-slim AS build
ENV LD_LIBRARY_PATH=/app:$LD_LIBRARY_PATH

WORKDIR /src
COPY ["Paperless.services/Paperless.services.csproj", "Paperless.services/"]
COPY ["Paperless.rabbitmq/Paperless.rabbitmq.csproj", "Paperless.rabbitmq/"]
COPY ["Paperless.OCR/Paperless.OCR.csproj", "Paperless.OCR/"]

RUN apt-get update && apt-get install -y \
    libleptonica-dev \
    libtesseract-dev \
    imagemagick \
    ghostscript \
    tesseract-ocr \
    libtesseract-dev \
    tesseract-ocr-eng \
    tesseract-ocr-deu

RUN mkdir -p /app/runtimes/linux-x64/native/
RUN ln -s /usr/lib/x86_64-linux-gnu/libdl.so.2 /usr/lib/x86_64-linux-gnu/libdl.so
RUN ln -s /usr/lib/x86_64-linux-gnu/libdl.so.2 /app/runtimes/linux-x64/native/libdl.so
RUN ln -s /usr/lib/x86_64-linux-gnu/libdl.so.2 /app/libdl.so
RUN mkdir /app/x64
RUN ln -s /usr/lib/x86_64-linux-gnu/liblept.so.5 /app/x64/libleptonica-1.82.0.so
RUN ln -s /usr/lib/x86_64-linux-gnu/libtesseract.so.5 /app/x64/libtesseract50.so

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