using OA_DataAccess;
using OA_Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace OA_Service
{
    public class NoteRepo : INoteRepo
    {
        private IRepository<Note> _repository;

        public NoteRepo(IRepository<Note> repository)
        {
            _repository = repository;
        }

        public void AddNote(Note note)
        {
            _repository.Insert(note);
        }

        public IEnumerable<Note> GetAllNotes()
        {
            return _repository.GetAll();
        }

        public Note GetNoteByID(int id)
        {
            return _repository.GetById(id);
        }

    }
}
