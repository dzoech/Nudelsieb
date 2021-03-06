#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Nudelsieb.WebApi/Nudelsieb.WebApi.csproj", "Nudelsieb.WebApi/"]
COPY ["Nudelsieb.Domain/Nudelsieb.Domain.csproj", "Nudelsieb.Domain/"]
COPY ["Nudelsieb.Application/Nudelsieb.Application.csproj", "Nudelsieb.Application/"]
COPY ["Nudelsieb.Notifications/Nudelsieb.Notifications.csproj", "Nudelsieb.Notifications/"]
COPY ["Nudelsieb.Persistence/Nudelsieb.Persistence.csproj", "Nudelsieb.Persistence/"]
COPY ["Nudelsieb.Persistence.Relational/Nudelsieb.Persistence.Relational.csproj", "Nudelsieb.Persistence.Relational/"]
RUN dotnet restore "Nudelsieb.WebApi/Nudelsieb.WebApi.csproj"
COPY . .
WORKDIR "/src/Nudelsieb.WebApi"
RUN dotnet build "Nudelsieb.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nudelsieb.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nudelsieb.WebApi.dll"]