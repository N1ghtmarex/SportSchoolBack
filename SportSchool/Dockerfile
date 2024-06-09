#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
RUN mkdir /app/wwwroot
RUN chown /app/wwwroot

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 1337

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Abstractions/Abstractions.csproj", "Abstractions/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure.Domain/Infrastructure.Domain.csproj", "Infrastructure.Domain/"]
COPY ["Infrastructure.External/Infrastructure.External.csproj", "Infrastructure.External/"]
COPY ["Keycloak/Keycloak.csproj", "Keycloak/"]
COPY ["SportSchool/SportSchool.csproj", "SportSchool/"]
RUN dotnet restore "SportSchool/SportSchool.csproj"
COPY . .
WORKDIR "/src/SportSchool"
RUN dotnet build "./SportSchool.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SportSchool.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SportSchool.dll"]