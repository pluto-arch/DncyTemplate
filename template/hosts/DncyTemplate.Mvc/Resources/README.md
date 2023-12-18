
## DataAnnotation 为默认的模型验证资源文件

如果需要使用默认的多文件方式，则需要去掉

```
.AddDataAnnotationsLocalization(options =>
            {
                options.SetUpDataAnnotationLocalizerProvider(); // 去掉这行即可
            })
```