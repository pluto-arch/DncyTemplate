#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# build & publish
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY ["nuget.config", "."]
COPY ["DncyTemplate.Job/DncyTemplate.Job.csproj", "DncyTemplate.Job/"]
COPY ["DncyTemplate.Infra/DncyTemplate.Infra.csproj", "DncyTemplate.Infra/"]
COPY ["DncyTemplate.Domain/DncyTemplate.Domain.csproj", "DncyTemplate.Domain/"]
COPY ["DncyTemplate.Application/DncyTemplate.Application.csproj", "DncyTemplate.Application/"]
RUN dotnet restore -r linux-x64 /p:PublishReadyToRun=true "DncyTemplate.Job/DncyTemplate.Job.csproj"
COPY . .
WORKDIR "/source/DncyTemplate.Job"
RUN dotnet build "DncyTemplate.Job.csproj" -c Release -o /source/build

FROM build AS publish
RUN dotnet publish "DncyTemplate.Job.csproj" -c Release -o /source/publish -r linux-x64 --self-contained true --no-restore /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishSingleFile=true

# runtiming -jammy-amd64 is Ubuntu X64 environment
FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-jammy-amd64
WORKDIR /app
EXPOSE 80
EXPOSE 443

# setup timezone
ENV LANG C.UTF-8
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo '$TZ' > /etc/timezone


COPY --from=publish /source/publish ./
ENTRYPOINT ["./DncyTemplate.Job"]