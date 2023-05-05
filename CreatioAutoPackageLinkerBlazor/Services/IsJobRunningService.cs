using Hangfire;

namespace CreatioAutoPackageLinkerBlazor.Services {
    public static class IsJobRunningService {
        public static bool IsJobRunning(string methodName) {
            var runningJobs = JobStorage.Current.GetMonitoringApi().ProcessingJobs(0, int.MaxValue);

            foreach (var job in runningJobs) {
                var jobType = job.Value.Job.Type;
                var jobMethod = job.Value.Job.Method;

                if (jobMethod.Name == methodName && jobMethod.DeclaringType == jobType) {
                    return true;
                }
            }

            return false;
        }
    }
}
