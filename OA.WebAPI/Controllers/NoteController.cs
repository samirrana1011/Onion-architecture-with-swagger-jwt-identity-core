using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public async Task<IEnumerable<Note>> Get()
        {
            var Identity = HttpContext.User.Identity as ClaimsIdentity;
            string SID = "";
            if (Identity != null)
            {
                var userCliams = Identity.Claims;
                SID = userCliams.FirstOrDefault(p => p.Type == ClaimTypes.Sid)?.Value;
              
            }
            var Products =await _repo.GetAllUserNotes(SID);
            return Products.ToArray();
        }
        [HttpPost("addnote")]
        public IActionResult Addnote([FromBody]Note note)
        {
            try
            {
                var Identity = HttpContext.User.Identity as ClaimsIdentity;
                if (Identity !=null)
                {
                    var userCliams = Identity.Claims;
                    string SID = userCliams.FirstOrDefault(p => p.Type == ClaimTypes.Sid)?.Value;
                    note.UserID = SID;
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