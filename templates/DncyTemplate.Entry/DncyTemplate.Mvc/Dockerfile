#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


ENV LANG C.UTF-8
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo '$TZ' > /etc/timezone


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["DncyTemplate.Entry/DncyTemplate.Mvc/DncyTemplate.Mvc.csproj", "DncyTemplate.Entry/DncyTemplate.Mvc/"]
COPY ["DncyTemplate.Infra/DncyTemplate.Infra.csproj", "DncyTemplate.Infra/"]
COPY ["DncyTemplate.Domain/DncyTemplate.Domain.csproj", "DncyTemplate.Domain/"]
COPY ["DncyTemplate.Application/DncyTemplate.Application.csproj", "DncyTemplate.Application/"]
RUN dotnet restore "DncyTemplate.Entry/DncyTemplate.Mvc/DncyTemplate.Mvc.csproj"
COPY . .
WORKDIR "/src/DncyTemplate.Entry/DncyTemplate.Mvc"
RUN dotnet build "DncyTemplate.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DncyTemplate.Mvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DncyTemplate.Mvc.dll"]