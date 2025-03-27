namespace GoneSoon.Infrastructure
{
    public interface IRedisStorageService
    {
        Task<bool> DeleteAsync(string key);
        Task<string?> GetAsync(string key);
        Task<IEnumerable<string>> GetKeysAsync(string pattern);
        Task<TimeSpan?> GetTtlAsync(string key);
        Task SetAsync(string key, string value, TimeSpan? expiry = null);
    }


}
