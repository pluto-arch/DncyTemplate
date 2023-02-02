# dotnet 快速开发模板

> 支持API，MVC项目，mvc自带简单admin页面

1. 打包
请确保电脑中有nuget.exe
```
# 打包成nuget
nuget pack Pluto.netcoreTemplate.nuspec 
```
会生成：PlutoNetCoreTemplate.{version}.nupkg  版本号可在nuspec中自定义
2. 安装
在刚刚生成的文件同目录执行下边命令
```
dotnet new -i PlutoNetCoreTemplate.1.3.2.nupkg  
```

3. 查看模板是否安装成功
```
dotnet new plutoapi -h
```


