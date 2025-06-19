using GoneSoon.InteractionProtocol.NoteService.Data;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GoneSoon.InteractionProtocol.Services
{
    public class NoteServiceClient : INoteServiceClient
    {
        private readonly HttpClient _httpClient;

        public NoteServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Note> CreateNoteAsync(NewNoteDto note)
        {
            var response = await _httpClient.PostAsJsonAsync("api/notes", note);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Note>();
        }

        public async Task UpdateNoteAsync(Note note)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/notes/{note.Id}", note);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Note> GetNoteAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/notes/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Note>();
        }

        public async Task DeleteNoteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/notes/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
