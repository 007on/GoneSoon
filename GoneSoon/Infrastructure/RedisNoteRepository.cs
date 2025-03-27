using GoneSoon.Models;
using GoneSoon.Repositories;
using System.Text.Json;

namespace GoneSoon.Infrastructure
{
    public class RedisNoteRepository : INoteRepository
    {
        private readonly IRedisStorageService _storageService;
        private const string NotePrefix = "note:";

        public RedisNoteRepository(IRedisStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<Note> CreateNewNote(Note note)
        {
            string key = GetKey(note);
            var expiry = note.ExpireDate - DateTime.UtcNow;
            var json = JsonSerializer.Serialize(note);

            await _storageService.SetAsync(key, json, expiry);

            return note;
        }

        public async Task<Note> GetNote(Guid id)
        {
            string key = GetKey(id);
            var json = await _storageService.GetAsync(key);
            return json != null ? JsonSerializer.Deserialize<Note>(json) : default;
        }

        public async Task DeleteNoteAsync(Guid id)
        {
            await _storageService.DeleteAsync(id.ToString());
        }

        public async Task UpdateNote(Note note)
        {
            var key = GetKey(note);
            var value = JsonSerializer.Serialize(note);
            var expiry = note.ExpireDate - DateTime.UtcNow;

            await _storageService.SetAsync(key, value, expiry);
        }

        public async Task DeleteNote(Guid id)
        {
            var key = GetKey(id);
            await _storageService.DeleteAsync(key);
        }

        private static string GetKey(Note note)
        {
            return GetKey(note.Id);
        }

        private static string GetKey(Guid noteId)
        {
            return $"{NotePrefix}{noteId}";
        }
    }
}
