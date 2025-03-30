using GoneSoon.Models;
using GoneSoon.Repositories;
using System.Text.Json;

namespace GoneSoon.Infrastructure
{
    public class RedisNoteRepository : INoteRepository
    {
        private readonly IRedisStorageService _storageService;

        public RedisNoteRepository(IRedisStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<Note> CreateNewNote(Note note)
        {
            var noteKey = RedisNoteRepositoryHelpers.GetRedisNoteKey(note.Id);
            var metadataKey = RedisNoteRepositoryHelpers.GetRedisMetadataKey(note.Id);
            var expiry = note.ExpireDate - DateTime.UtcNow;

            var json = JsonSerializer.Serialize(note);
            var metadata = JsonSerializer.Serialize(new { note.Title, note.UserId });

            await _storageService.SetAsync(noteKey, json, expiry);
            await _storageService.SetAsync(metadataKey, metadata, expiry);

            return note;
        }

        public async Task<Note> GetNote(Guid id)
        {
            var key = RedisNoteRepositoryHelpers.GetRedisNoteKey(id);
            var json = await _storageService.GetAsync(key);
            return json != null ? JsonSerializer.Deserialize<Note>(json) : null;
        }

        public async Task<NoteMetadata> GetNoteMetadata(Guid id)
        {
            var key = RedisNoteRepositoryHelpers.GetRedisMetadataKey(id);
            var json = await _storageService.GetAsync(key);
            return json != null ? JsonSerializer.Deserialize<NoteMetadata>(json) : null;
        }

        public async Task UpdateNote(Note note)
        {
            var noteKey = RedisNoteRepositoryHelpers.GetRedisNoteKey(note.Id);
            var metadataKey = RedisNoteRepositoryHelpers.GetRedisMetadataKey(note.Id);
            var value = JsonSerializer.Serialize(note);
            var metadata = JsonSerializer.Serialize(new NoteMetadata { Title = note.Title, UserId = note.UserId });
            var expiry = note.ExpireDate - DateTime.UtcNow;

            await _storageService.SetAsync(noteKey, value, expiry);
            await _storageService.SetAsync(metadataKey, metadata, expiry);
        }

        public async Task DeleteNote(Guid id)
        {
            var noteKey = RedisNoteRepositoryHelpers.GetRedisNoteKey(id);
            var metadataKey = RedisNoteRepositoryHelpers.GetRedisMetadataKey(id);

            await _storageService.DeleteAsync(noteKey);
            await _storageService.DeleteAsync(metadataKey);
        }
    }
}
