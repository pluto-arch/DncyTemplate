using Quartz;

namespace DncyTemplate.Job.Models;

public class JobInfoModel
{
    public string Id { get; set; }

    /// <summary>
    /// </summary>
    public EnumTaskType TaskType { get; set; }


    /// <summary>
    ///     显示名称
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    ///     作业名称
    /// </summary>
    public string TaskName { get; set; }

    /// <summary>
    ///     组名
    /// </summary>
    public string GroupName { get; set; }

    /// <summary>
    /// </summary>
    public string Interval { get; set; }

    /// <summary>
    ///     触发器名称
    /// </summary>
    public string TriggerName { get; set; }

    /// <summary>
    ///     调用的api地址
    /// </summary>
    public string ApiUrl { get; set; }

    /// <summary>
    ///     接口授权key
    /// </summary>
    public string AuthKey { get; set; }

    /// <summary>
    ///     接口授权value
    /// </summary>
    public string AuthValue { get; set; }

    /// <summary>
    ///     描述
    /// </summary>
    public string Describe { get; set; }

    /// <summary>
    ///     请求方式
    /// </summary>
    public string RequestType { get; set; }

    /// <summary>
    /// 远程调用超时时长
    /// </summary>
    public int RemoteCallTimeOut { get; set; } = 60;

    /// <summary>
    ///     最后运行时间
    /// </summary>
    public string LastRunTime { get; set; }

    /// <summary>
    ///     触发器状态
    /// </summary>
    public TriggerState TriggerStatus { get; set; }

    /// <summary>
    ///     状态
    /// </summary>
    public EnumJobStates Status { get; set; }
}