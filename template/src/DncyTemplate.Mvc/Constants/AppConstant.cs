namespace DncyTemplate.Constants;

public class AppConstant
{

    public const string SERVICE_NAME = "DncyTemplate.Mvc";

#if Tenant
    /// <summary>
    /// 租户解析的key
    /// </summary>
    public const string TENANT_KEY = "tenant_id";

    /// <summary>
    /// 临时cookie存储的用户信息key
    /// </summary>
    public const string TEMP_COOKIE_USER_KEY = "tenant_id";
#endif
    /// <summary>
    /// 默认跨域名称
    /// </summary>
    public const string DEFAULT_CORS_NAME = "default";




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


    public class Culture
    {
        public static (string key, string name) ZN_CH = ("zh-CN", "中文");

        public static (string key, string name) EN_US = ("en-US", "English(US)");
    }
}