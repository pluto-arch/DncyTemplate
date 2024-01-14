# dotnet 快速开发模板

> 支持API，MVC项目，mvc自带简单admin页面
> 支持多租户应用模式，或者单体应用模式，微服务模式。
> efcore+uow(单dbcontext+多dbcontext支持) 满足常用的应用开发场景
> 支持ef+Dapper混合操作

1. 打包
请确保电脑中有nuget.exe
```
# 打包成nuget
nuget pack DotNetBoltTemplate.nuspec 
```
会生成：DotNetBoltTemplate.{version}.nupkg  版本号可在nuspec中自定义
2. 安装
在刚刚生成的文件同目录执行下边命令
```
dotnet new install DotNetBoltTemplate.{version}.nupkg  
```

3. 查看模板是否安装成功
```
dotnet new boltapp -h
```

4. 安装cli工具
将cli下的项目进行打包。然后回到根目录。会看到nupkg文件夹，在此文件夹中执行：
```
dotnet tool install -g DotnetyddTemplateCli --add-source ./
```

cli 工具安装完毕后既可以使用cli初始化项目：
```
dotnetydd --init
``


>模板项目中用到的组件：多租户、规约模式、权限检查器实现代码在以下仓库中：
- [租户、规约、权限组件](https://github.com/pluto-arch/DncyComponent)
- [工具代码](https://github.com/pluto-arch/dncytools)
- [IOC源生成器](https://github.com/pluto-arch/Dncy.Microsoft.DependencyInjection.Generator)
