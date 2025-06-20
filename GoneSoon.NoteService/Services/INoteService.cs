﻿using GoneSoon.InteractionProtocol.NoteService.Data;

namespace GoneSoon.NoteService.Services
{
    public interface INoteService
    {
        Task<Note> CreateNewNote(NewNoteDto newNote);
        Task DeleteNote(Guid noteId);
        Task UpdateNote(Note note);
        Task<Note> GetNote(Guid noteId);
    }
}