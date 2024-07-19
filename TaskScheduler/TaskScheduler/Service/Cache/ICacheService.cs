namespace TaskScheduler.Cache
{
    public interface ICacheService
    {
        Task SetAsync(string key, string value, TimeSpan expiration);
        Task<string> GetAsync(string key);
    }
}