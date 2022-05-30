namespace DncyTemplate.Job.Models;

public enum EnumJobStates
{
    None,

    /// <summary>
    /// 正常
    /// </summary>
    Normal,

    /// <summary>
    /// 暂停
    /// </summary>
    Pause,

    /// <summary>
    /// 完成
    /// </summary>
    Completed,

    /// <summary>
    /// 异常
    /// </summary>
    Exception,

    /// <summary>
    /// 冻结
    /// </summary>
    Blocked,

    /// <summary>
    /// 停止
    /// </summary>
    Stopped
}