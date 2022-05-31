using DncyTemplate.Job.Infra.Stores;
using DncyTemplate.Job.Models;
using Quartz;

namespace DncyTemplate.Job.Controllers;

[AutoResolveDependency]
public partial class HomeController : Controller
{
    [AutoInject]
    private readonly IJobInfoStore _jobInfoStore;


    public async Task<IActionResult> Index()
    {
        var jobs = await _jobInfoStore.GetListAsync();
        ViewData["JobCount"] = await _jobInfoStore.CountAsync();
        ViewData["RunningJobCount"] = jobs?.Count(x=>x.Status==EnumJobStates.Normal)??0;
        ViewData["PauseJobCount"] = jobs?.Count(x => x.Status == EnumJobStates.Pause)??0;
        return View();
    }
}