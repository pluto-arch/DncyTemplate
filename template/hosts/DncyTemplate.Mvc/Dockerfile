#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# build & publish
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
COPY ["nuget.config", "."]
COPY ["hosts/DncyTemplate.Mvc/DncyTemplate.Mvc.csproj", "hosts/DncyTemplate.Mvc/"]
COPY ["src/DncyTemplate.Infra/DncyTemplate.Infra.csproj", "src/DncyTemplate.Infra/"]
COPY ["src/DncyTemplate.Domain/DncyTemplate.Domain.csproj", "src/DncyTemplate.Domain/"]
COPY ["src/DncyTemplate.Application/DncyTemplate.Application.csproj", "src/DncyTemplate.Application/"]
RUN dotnet restore -r linux-x64 /p:PublishReadyToRun=true "hosts/DncyTemplate.Mvc/DncyTemplate.Mvc.csproj"
COPY . .
WORKDIR "/source/hosts/DncyTemplate.Mvc"
RUN dotnet build "DncyTemplate.Mvc.csproj" -c Release -o /source/build

FROM build AS publish
RUN dotnet publish "DncyTemplate.Mvc.csproj" -c Release -o /source/publish -r linux-x64 --self-contained true --no-restore /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishSingleFile=true


# runtiming
FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-jammy-amd64
WORKDIR /app
EXPOSE 80
EXPOSE 443

# setup timezone
ENV LANG C.UTF-8
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo '$TZ' > /etc/timezone

COPY --from=publish /source/publish ./
ENTRYPOINT ["./DncyTemplate.Mvc"]