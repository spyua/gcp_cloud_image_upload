# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["*.csproj", "./cbk.cloudUploadImage.service.login/"]
COPY ["../cbk.cloudUploadImage.Infrastructure/*.csproj", "./cbk.cloudUploadImage.Infrastructure/"]
COPY ["../cbk.cloud.gcp.serviceProvider/*.csproj", "./cbk.cloud.gcp.serviceProvider/"]
RUN dotnet restore "./cbk.cloudUploadImage.service.login/cbk.cloudUploadImage.service.login.csproj"

# Copy everything else
COPY .. .

WORKDIR "/src/cbk.cloudUploadImage.service.login"

# Copy pem and json files
COPY ["../cbk.cloudUploadImage.Infrastructure/Files/CertificateFil/*.pem", "./"]
COPY ["../cbk.cloudUploadImage.Infrastructure/Files/CertificateFil/*.json", "./"]

RUN dotnet build "cbk.cloudUploadImage.service.login.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cbk.cloudUploadImage.service.login.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set Environment Variable
ENV GOOGLE_APPLICATION_CREDENTIALS=./affable-cacao-389805-297d12d69696.json

ENTRYPOINT ["dotnet", "cbk.cloudUploadImage.service.login.dll"]
