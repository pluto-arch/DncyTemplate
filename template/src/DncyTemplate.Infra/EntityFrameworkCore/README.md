﻿## 迁移命令

> 由于使用多租户或者领域事件方式，dbcontext初始化需要加载 依赖，所以迁移需要定义 对应的MigrationDbContext。
> 具体参考 DeviceCenterDbContext  -->  DeviceCenterMigrationDbContext 以下命令中的 -Context 需要换成 MigrationDbContext的。

```
-- DeviceCenterDbContext
Add-Migration InitialCreate -Context DeviceCenterMigrationDbContext -Project DncyTemplate.Infra -StartupProject DncyTemplate.Infra -OutputDir Migrations/DeviceCenter


Remove-Migration -Context DeviceCenterMigrationDbContext -Project DncyTemplate.Infra -StartupProject DncyTemplate.Infra


Update-Database -Context DeviceCenterMigrationDbContext -Project DncyTemplate.Infra -StartupProject DncyTemplate.Infra


-- 使用链接字符串应用迁移
Update-Database -Context DeviceCenterMigrationDbContext -Project DncyTemplate.Infra -StartupProject DncyTemplate.Infra -Connection "Server=192.168.0.126,1433;Database=Pnct_T20210602000002;User Id=sa;Password=970307lBx;Trusted_Connection = False;"


```
