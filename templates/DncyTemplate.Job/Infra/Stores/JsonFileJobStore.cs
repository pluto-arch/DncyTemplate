using DncyTemplate.Job.Models;
using Quartz;

namespace DncyTemplate.Job.Infra.Stores;

public class JsonFileJobStore : IJobInfoStore
{

    private readonly string _defaultDir;
    private readonly string _defaultJobFile;
    public JsonFileJobStore(IHostEnvironment env)
    {
        _defaultDir = Path.Combine(env.ContentRootPath, "BackgroundJob");
        _defaultJobFile = Path.Combine(_defaultDir, "BackgroundJobs.json");
        CreateDirectoryIfNotExists();
        CreateJsonFileIfNotExists();

    }

    private void CreateJsonFileIfNotExists()
    {
        if (!File.Exists(_defaultJobFile))
        {
            File.Create(_defaultJobFile);
        }
    }

    private void CreateDirectoryIfNotExists()
    {
        if (!Directory.Exists(_defaultDir))
        {
            Directory.CreateDirectory(_defaultDir);
        }
    }


    private async Task<List<JobInfoModel>> ReadFromFileAsync()
    {
        var text = await File.ReadAllTextAsync(_defaultJobFile);
        if (string.IsNullOrEmpty(text))
        {
            return default;
        }

        var models = JsonConvert.DeserializeObject<List<JobInfoModel>>(text);
        return models;
    }


    /// <inheritdoc />
    public async Task<int> CountAsync()
    {
        return (await ReadFromFileAsync())?.Count ?? 0;
    }

    /// <inheritdoc />
    public async Task<List<JobInfoModel>> GetListAsync()
    {
        return await ReadFromFileAsync();
    }

    /// <inheritdoc />
    public async Task<JobInfoModel> GetAsync(string id)
    {
        var list = await ReadFromFileAsync();
        if (list != null && list.Any())
        {
            return list.FirstOrDefault(x => x.Id == id);
        }
        return null;
    }

    /// <inheritdoc />
    public async Task<JobInfoModel> GetAsync(JobKey job)
    {
        var list = await ReadFromFileAsync();
        if (list != null && list.Any())
        {
            return list.FirstOrDefault(x => x.TaskName == job.Name && x.GroupName == job.Group);
        }
        return null;
    }

    /// <inheritdoc />
    public async Task AddAsync(JobInfoModel job)
    {
        var list = await ReadFromFileAsync() ?? new List<JobInfoModel>();

        var exist = list.FirstOrDefault(x => x.TaskName == job.TaskName && x.GroupName == job.GroupName);
        if (exist != null)
        {
            throw new InvalidOperationException($"group:[{job.GroupName}]. name:[{job.TaskName}] 已经存在");
        }
        list.Add(job);
        await WriteToFileAsync(list);
    }


    /// <inheritdoc />
    public async Task UpdateAsync(JobInfoModel job)
    {
        var list = await ReadFromFileAsync() ?? new List<JobInfoModel>();
        if (!list.Any())
        {
            throw new InvalidOperationException($"元素:[{job.Id}]不存在，更新失败");
        }
        var el = list.FirstOrDefault(x => x.Id == job.Id);
        list.Remove(el);
        list.Add(job);
        await WriteToFileAsync(list);
    }

    /// <inheritdoc />
    public async Task RemoveAsync(string groupName, string jobName)
    {
        var list = await ReadFromFileAsync() ?? new List<JobInfoModel>();
        if (!list.Any())
        {
            return;
        }
        list.RemoveAll(x => x.GroupName == groupName && x.TaskName == jobName);
        await WriteToFileAsync(list);
    }

    /// <inheritdoc />
    public async Task PauseAsync(string groupName, string jobName)
    {
        var list = await ReadFromFileAsync() ?? new List<JobInfoModel>();
        if (!list.Any())
        {
            return;
        }
        foreach (var x in list)
        {
            if (x.GroupName == groupName && x.TaskName == jobName)
            {
                x.Status = EnumJobStates.Pause;
            }
        }
        await WriteToFileAsync(list);
    }



    private async Task WriteToFileAsync(List<JobInfoModel> list)
    {
        var text = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(list));
        await File.WriteAllBytesAsync(_defaultJobFile, text);
    }
}