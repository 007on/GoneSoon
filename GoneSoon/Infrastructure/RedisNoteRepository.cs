using GoneSoon.Models;
using GoneSoon.Repositories;
using StackExchange.Redis;
using System.Text.Json;

namespace GoneSoon.Infrastructure
{
    public class RedisNoteRepository : INoteRepository
    {
        private readonly IDatabase _database;
        private const string NotePrefix = "note:";

        public RedisNoteRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<Note> CreateNewNote(Note note)
        {
            var key = $"{NotePrefix}{note.Id}";
            var value = JsonSerializer.Serialize(note);
            var expiry = note.ExpireDate - DateTime.UtcNow;

            await _database.StringSetAsync(key, value, expiry);
            return note;
        }

        public async Task<Note> GetNote(Guid noteId)
        {
            var key = $"{NotePrefix}{noteId}";
            var value = await _database.StringGetAsync(key);

                return value.HasValue ? JsonSerializer.Deserialize<Note>(value!) : null;
        }

        public async Task UpdateNote(Note note)
        {
            var key = $"{NotePrefix}{note.Id}";
            if (!await _database.KeyExistsAsync(key))
            {
                throw new ArgumentException("Note does not exist or was deleted.");
            }

            var value = JsonSerializer.Serialize(note);
            var expiry = note.ExpireDate - DateTime.UtcNow;
            await _database.StringSetAsync(key, value, expiry);
        }

        public async Task DeleteNote(Guid noteId)
        {
            var key = $"{NotePrefix}{noteId}";
            await _database.KeyDeleteAsync(key);
        }
    }
}
