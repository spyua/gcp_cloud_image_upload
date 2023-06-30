# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["cbk.image.service.member/*.csproj", "./cbk.image.service.member/"]
COPY ["cbk.image.Infrastructure/*.csproj", "./cbk.image.Infrastructure/"]
COPY ["cbk.cloud.serviceProvider/*.csproj", "./cbk.cloud.serviceProvider/"]
RUN dotnet restore "./cbk.image.service.member/cbk.image.service.member.csproj"

# Copy everything else
COPY .. .

WORKDIR "/src/cbk.image.service.member"

# Copy pem and json files
COPY ["cbk.image.Infrastructure/Files/CertificateFil/*.pem", "./"]
COPY ["cbk.image.Infrastructure/Files/CertificateFil/*.json", "./"]

RUN dotnet build "cbk.image.service.member.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cbk.image.service.member.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set Environment Variable
ENV GOOGLE_APPLICATION_CREDENTIALS=./affable-cacao-389805-297d12d69696.json

ENTRYPOINT ["dotnet", "cbk.image.service.member.dll"]
