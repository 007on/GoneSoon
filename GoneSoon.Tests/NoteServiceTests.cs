using GoneSoon.Models;
using GoneSoon.Repositories;
using GoneSoon.Services;
using Moq;

namespace GoneSoon.Tests
{
    public class NoteServiceTests
    {
        private readonly Mock<INoteRepository> _mockRepository;
        private readonly NoteService _service;

        public NoteServiceTests()
        {
            _mockRepository = new Mock<INoteRepository>();
            _service = new NoteService(_mockRepository.Object);
        }

        [Fact]
        public async Task CreateNewNoteAsync_CreatesNote()
        {
            var note = new Note { Id = Guid.NewGuid(), Title = "Test Note" };

            _mockRepository.Setup(r => r.CreateNewNote(It.IsAny<Note>())).ReturnsAsync(note);

            var result = await _service.CreateNewNoteAsync(note);

            Assert.Equal(note, result);
        }

        [Fact]
        public async Task GetNoteById_ReturnsNote()
        {
            var noteId = Guid.NewGuid();
            var note = new Note { Id = noteId, Title = "Test Note" };

            _mockRepository.Setup(r => r.GetNote(noteId)).ReturnsAsync(note);

            var result = await _service.GetNoteById(noteId);

            Assert.Equal(note, result);
        }

        [Fact]
        public async Task UpdateNote_UpdatesNote()
        {
            var note = new Note { Id = Guid.NewGuid(), Title = "Test Note" };

            _mockRepository.Setup(r => r.GetNote(note.Id)).ReturnsAsync(note);
            _mockRepository.Setup(r => r.UpdateNote(note)).Returns(Task.CompletedTask);

            await _service.UpdateNote(note);

            _mockRepository.Verify(r => r.UpdateNote(note), Times.Once);
        }

        [Fact]
        public async Task DeleteNote_DeletesNote()
        {
            var noteId = Guid.NewGuid();

            _mockRepository.Setup(r => r.DeleteNote(noteId)).Returns(Task.CompletedTask);

            await _service.DeleteNote(noteId);

            _mockRepository.Verify(r => r.DeleteNote(noteId), Times.Once);
        }
    }
}
