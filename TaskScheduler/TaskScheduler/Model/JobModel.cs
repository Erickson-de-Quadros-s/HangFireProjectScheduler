using System.ComponentModel.DataAnnotations;

namespace TaskScheduler.Model
{
    public class JobModel
    {
        [Required]
        public string Message { get; set; }

        [Required]
        public int DelayInSeconds { get; set; }

        public string ParentJobId { get; set; }

        public string JobId { get; set; }

        public string CronExpression { get; set; }

        public JobModel(string message, int delayInSeconds, string parentJobId = null, string jobId = null, string cronExpression = null)
        {
            Message = message;
            DelayInSeconds = delayInSeconds;
            ParentJobId = parentJobId;
            JobId = jobId;
            CronExpression = cronExpression;
        }
    }
}
