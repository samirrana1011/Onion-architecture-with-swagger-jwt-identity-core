using OA_DataAccess;
using OA_Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        public void DeleteNote(Note note)
        {
            _repository.Delete(note);
        }
        public void UpdateNote(Note note)
        {
            _repository.Update(note);
        }
        public async Task<IEnumerable<Note>> GetAllUserNotes(string Uid)
        {
            //await unitOfWork.YourRepository.GetAsync(filter: w => w.OtherKeyNavigation.Id == 123456,
            //    includeProperties: "OtherKeyNavigation", first: 20, offset: 0);

            return await _repository.GetFilteredData(filter: w => w.UserID==Uid);
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
