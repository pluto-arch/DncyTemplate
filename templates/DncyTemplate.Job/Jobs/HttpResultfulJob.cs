using System.Net.Http;
using DncyTemplate.Job.Infra;
using DncyTemplate.Job.Infra.Stores;
using Quartz;

using static Quartz.Logging.OperationName;

namespace DncyTemplate.Job.Jobs
{
    [AutoResolveDependency]
    public partial class HttpResultfulJob : IJob, IBackgroundJob
    {
        [AutoInject]
        private readonly IHttpClientFactory _httpClientFactory;

        [AutoInject] 
        private readonly IJobInfoStore _jobInfoStore;
        
        [AutoInject]
        private readonly ILogger<HttpResultfulJob> _logger;


        /// <inheritdoc />
        public async Task Execute(IJobExecutionContext context)
        {
            var jobInfo = await _jobInfoStore.GetAsync(context.JobDetail.Key);
            if (jobInfo==null)
            {
                _logger.LogError("{jobKey} : not found in store", context.JobDetail.Key);
                return;
            }
            var client = _httpClientFactory.CreateClient(jobInfo.TaskName);
            var request = new HttpRequestMessage(HttpMethod.Get, jobInfo.ApiUrl);
            var response = await client.SendAsync(request);
            var resstring = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("call apiurl [{apiUrl}] result : {result}", jobInfo.ApiUrl, resstring);
        }
    }
}