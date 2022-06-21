namespace DncyTemplate.Api.Constants;
public class AppConstant
{
    /// <summary>
    /// 租户解析的key
    /// </summary>
    public const string TENANT_KEY = "tenant_id";


    /// <summary>
    /// 默认跨域名称
    /// </summary>
    public const string DEFAULT_CORS_NAME = "default";

    /// <summary>
    /// 默认的http： context type 格式
    /// </summary>

    public const string DEFAULT_CONTENT_TYPE = MediaTypeNames.Application.Json;

    /// <summary>
    /// 环境名称
    /// </summary>
    public class EnvironmentName
    {
        /// <summary>
        /// 开发环境
        /// </summary>
        public const string DEV = "dev";
        /// <summary>
        /// 测试环境
        /// </summary>
        public const string TEST = "test";
        /// <summary>
        /// 发布环境
        /// </summary>
        public const string RELEASE = "release";
    }
}