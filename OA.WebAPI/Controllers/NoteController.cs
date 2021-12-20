using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OA_DataAccess;
using OA_Service;

namespace OA.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class NoteController : Controller
    {
        private readonly INoteRepo _repo;

        public NoteController(INoteRepo repo)
        {
            _repo = repo;
        }
       
        [HttpGet]
        public IEnumerable<Note> Get()
        {
            var Products = _repo.GetAllNotes();
            return Products.ToArray();
        }
        [HttpPost("addnote")]
        public IActionResult Addnote([FromBody]Note note)
        {
            try
            {
                var currentUser = HttpContext.User;
                if (currentUser.HasClaim(c => c.Type == "Email"))
                {

                }
                //    note.User = currentUser;
                _repo.AddNote(note);
              return  Ok();
            }
            catch (Exception ex)
            {
                return Unauthorized(ex);
            }
        }
    }
}