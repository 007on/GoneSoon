using GoneSoon.Models;
using GoneSoon.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoneSoon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INoteManager _noteManager;
        private readonly ILogger<NotesController> _logger;

        public NotesController(INoteManager noteManager, ILogger<NotesController> logger)
        {
            _noteManager = noteManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] NewNoteDto note)
        {
            _logger.LogInformation("Creating a new note.");
            try
            {
                var createdNote = await _noteManager.CreateNewNote(note);
                _logger.LogInformation("Note created successfully.");
                return Ok(new { noteId = createdNote.Id, userId = createdNote.UserId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating note.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNote([FromBody] Note note)
        {
            _logger.LogInformation("Updating note with ID {NoteId}", note.Id);
            try
            {
                await _noteManager.UpdateNote(note);
                _logger.LogInformation("Note with ID {NoteId} updated successfully.", note.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating note with ID {NoteId}", note.Id);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNote(Guid id)
        {
            _logger.LogInformation("Retrieving note with ID {NoteId}", id);
            try
            {
                var note = await _noteManager.GetNote(id);
                if (note == null)
                {
                    _logger.LogWarning("Note with ID {NoteId} not found.", id);
                    return NotFound();
                }
                _logger.LogInformation("Note with ID {NoteId} retrieved successfully.", id);
                return Ok(note);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving note with ID {NoteId}", id);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            _logger.LogInformation("Deleting note with ID {NoteId}", id);
            try
            {
                await _noteManager.DeleteNote(id);
                _logger.LogInformation("Note with ID {NoteId} deleted successfully.", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting note with ID {NoteId}", id);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
