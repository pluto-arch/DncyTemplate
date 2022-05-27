using DncyTemplate.Job.Infra.Stores;
using DncyTemplate.Job.Models;
using Quartz;

namespace DncyTemplate.Job.Controllers;

[AutoResolveDependency]
public partial class HomeController : Controller
{
    [AutoInject]
    private readonly IJobInfoStore _jobInfoStore;
    [AutoInject]
    private readonly ISchedulerFactory _jobSchedularFactory;


    public async Task<IActionResult> Index()
    {
        IScheduler scheduler = await _jobSchedularFactory.GetScheduler();
        IReadOnlyCollection<IJobExecutionContext> jobs = await scheduler.GetCurrentlyExecutingJobs();
        ViewData["JobCount"] = await _jobInfoStore.CountAsync();
        ViewData["RunningJobCount"] = jobs.Count;
        ViewData["PauseJobCount"] =
            (await _jobInfoStore.GetListAsync()).Count(x => x.Status == EnumJobStates.Pause);
        return View();
    }
}