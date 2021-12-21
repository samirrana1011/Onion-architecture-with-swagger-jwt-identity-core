using OA_DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OA_Service
{
   public interface INoteRepo
    {
        IEnumerable<Note> GetAllNotes();
        Note GetNoteByID(int id);
        void AddNote(Note note);
        Task<IEnumerable<Note>> GetAllUserNotes(string Uid);

    }
}
