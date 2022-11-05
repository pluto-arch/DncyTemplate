## 单元工作模块

Domain中定义的只是一系列抽象，具体的实现再基础设施层，可以是efcore的单元工作(ef,efcore自身就是)，可以是dapper的实现，可以是mongo的实现,....


## 切换工作范围

示例：配合租户切换

```
 using (_unitOfWorkScopeManager.Begin())
 using (_currentTenant.Change(new TenantInfo(id,name)))
 {
     if (rep.Any())
     {
         continue;
     }
 
     foreach (var item in per)
     {
         await rep.InsertAsync(new PermissionGrant
         {
             Name = item.Name,
             ProviderName = "role",
             ProviderKey = "sa",
         });
     }
     await rep.Uow.SaveChangesAsync();
 }
```