
# 集成事件

详情参见：https://microservices.io/patterns/data/transactional-outbox.html

## 事务性发件箱：

![事务性发件箱](https://microservices.io/i/patterns/data/ReliablePublication.png)


1. 本地发件箱可以使用任何存储模式，保证和业务域操作属于同一个事务内保存即可
2. 单独发信服务从信箱里面读取然后丢到事件总线