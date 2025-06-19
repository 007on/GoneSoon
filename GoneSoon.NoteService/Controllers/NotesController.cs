using GoneSoon.InteractionProtocol.NoteService.Data;
using GoneSoon.NoteService.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoneSoon.NoteService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly ILogger<NotesController> _logger;
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService, ILogger<NotesController> logger)
        {
            _noteService = noteService;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new note.
        /// </summary>
        /// <param name="note">The note data to create.</param>
        /// <returns>The created note.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Note), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateNote([FromBody] NewNoteDto note)
        {
            if (note == null)
            {
                _logger.LogError("Invalid note data received.");
                return BadRequest("Note data is invalid.");
            }

            try
            {
                var createdNote = await _noteService.CreateNewNote(note);
                return CreatedAtAction(nameof(GetNote), new { id = createdNote.Id }, createdNote);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a note.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Updates an existing note.
        /// </summary>
        /// <param name="id">The ID of the note to update.</param>
        /// <param name="note">The updated note data.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateNote(Guid id, [FromBody] Note note)
        {
            if (note == null || id != note.Id)
            {
                _logger.LogError("Invalid note data received.");
                return BadRequest("Note data is invalid.");
            }

            try
            {
                await _noteService.UpdateNote(note);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating the note.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Retrieves a note by ID.
        /// </summary>
        /// <param name="id">The ID of the note to retrieve.</param>
        /// <returns>The requested note.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Note), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNote(Guid id)
        {
            try
            {
                var note = await _noteService.GetNote(id);
                if (note == null)
                {
                    return NotFound();
                }

                return Ok(note);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving the note.");
                return StatusCode(500, "Internal server error.");
            }
        }

        /// <summary>
        /// Deletes a note by ID.
        /// </summary>
        /// <param name="id">The ID of the note to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            try
            {
                await _noteService.DeleteNote(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting the note.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
