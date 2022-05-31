namespace DncyTemplate.Job.Models;

public class JobLogModel
{
    public string Time { get; set; }

    public int RunSeconds { get; set; }

    public EnumJobStates State { get; set; }

    public string Message { get; set; }
}