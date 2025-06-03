using GoneSoon.InteractionProtocol;

namespace GoneSoon.NoteService.Infrastructure
{
    public static class RedisNoteRepositoryHelpers
    {
        private const string NotePrefix = "note:";
        private const string MetadataPrefix = "metadata:";
        public static string GetRedisNoteKey(Note note)
        {
            return GetRedisNoteKey(note.Id);
        }

        public static string GetRedisNoteKey(Guid noteId)
        {
            return $"{NotePrefix}{noteId}";
        }

        public static string GetRedisMetadataKey(Guid id)
        {
            return $"{MetadataPrefix}{id}";
        }

        public static Guid GetNoteIdFromRedisKey(string key)
        {
            return Guid.Parse(key.Substring(NotePrefix.Length));
        }
    }

    public interface IRedisStorageService
    {
        Task<bool> DeleteAsync(string key);
        Task<string?> GetAsync(string key);
        Task<IEnumerable<string>> GetKeysAsync(string pattern);
        Task<TimeSpan?> GetTtlAsync(string key);
        Task SetAsync(string key, string value, TimeSpan? expiry = null);
    }
}
