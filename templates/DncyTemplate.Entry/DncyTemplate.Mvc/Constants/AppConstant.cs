﻿namespace DncyTemplate.Mvc.Constants;

public class AppConstant
{
    /// <summary>
    /// 租户解析的key
    /// </summary>
    public const string TENANT_KEY = "tenant_id";

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