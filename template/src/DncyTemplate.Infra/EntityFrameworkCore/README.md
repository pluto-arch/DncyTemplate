﻿## 迁移命令

> 由于使用多租户或者领域事件方式，dbcontext初始化需要加载 依赖，所以迁移需要定义 对应的MigrationDbContext。
> 具体参考 DncyTemplateDbContext  -->  DncyTemplateMigrationDbContext 以下命令中的 -Context 需要换成 MigrationDbContext的。
### 使用vs 包管理控制台 切换项目至 DncyTemplate.Infra
```shell
# 添加迁移
Add-Migration InitialCreate -Context DncyTemplateMigrationDbContext -Project DncyTemplate.Infra -StartupProject DncyTemplate.Infra -OutputDir Migrations/DncyTemplateDb

# 移除迁移
Remove-Migration -Context DncyTemplateMigrationDbContext -Project DncyTemplate.Infra -StartupProject DncyTemplate.Infra

# 应用迁移
Update-Database -Context DncyTemplateMigrationDbContext -Project DncyTemplate.Infra -StartupProject DncyTemplate.Infra


# 使用链接字符串应用迁移
Update-Database -Context DncyTemplateMigrationDbContext -Project DncyTemplate.Infra -StartupProject DncyTemplate.Infra -Connection "数据库连接字符串"
```

### dotnet-ef 迁移命令
切换目录值src下。运行命令行
```shell
# 添加迁移
dotnet-ef migration add <migration_name> -c DncyTemplateMigrationDbContext -p DncyTemplate.Infra -s DncyTemplate.Infra -o Migrations/DncyTemplateDb

# 迁移应用到数据库
dotnet-ef database update -c DncyTemplateMigrationDbContext -p DncyTemplate.Infra -s DncyTemplate.Infra

# 指定连接字符串迁移
dotnet-ef database update -c DncyTemplateMigrationDbContext -p DncyTemplate.Infra -s DncyTemplate.Infra --connection "数据库连接字符串"
```
