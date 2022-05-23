#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DncyTemplate.Entry/DncyTemplate.Api/DncyTemplate.Api.csproj", "DncyTemplate.Entry/DncyTemplate.Api/"]
RUN dotnet restore "DncyTemplate.Entry/DncyTemplate.Api/DncyTemplate.Api.csproj"
COPY . .
WORKDIR "/src/DncyTemplate.Entry/DncyTemplate.Api"
RUN dotnet build "DncyTemplate.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DncyTemplate.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DncyTemplate.Api.dll"]