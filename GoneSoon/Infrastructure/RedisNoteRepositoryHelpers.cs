using GoneSoon.Models;

namespace GoneSoon.Infrastructure
{
    internal static class RedisNoteRepositoryHelpers
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
}