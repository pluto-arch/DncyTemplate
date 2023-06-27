# dotnet 快速开发模板

> 支持API，MVC项目，mvc自带简单admin页面
> 支持多租户应用模式，或者单体应用模式，微服务模式。
> efcore+uow(单dbcontext+多dbcontext支持) 满足常用的应用开发场景
> 支持ef+Dapper混合操作

1. 打包
请确保电脑中有nuget.exe
```
# 打包成nuget
nuget pack DncyTemplate.nuspec 
```
会生成：DncyTemplate.{version}.nupkg  版本号可在nuspec中自定义
2. 安装
在刚刚生成的文件同目录执行下边命令
```
dotnet new -i DncyTemplate.{version}.nupkg  
```

3. 查看模板是否安装成功
```
dotnet new dncy -h
```


