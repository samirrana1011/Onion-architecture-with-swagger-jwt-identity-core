using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;

        public NoteController(INoteRepo repo, UserManager<User> userManager)
        {
            _repo = repo;
            _userManager = userManager; 
        }
       
        [HttpGet]
        public async Task<IEnumerable<Note>> Get()
        {
            try
            {
                var Identity = HttpContext.User.Identity as ClaimsIdentity;
                string SID = "";
                if (Identity != null)
                {
                    var userCliams = Identity.Claims;
                    SID = userCliams.FirstOrDefault(p => p.Type == ClaimTypes.Sid)?.Value;

                }
                var Products = await _repo.GetAllUserNotes(SID);
                return Products.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
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

        [HttpPut("update/{id:int}")]
        public IActionResult Updatenote(int id, [FromBody]Note Newnote)
        {
            try
            {
                string SID = "";
                Note note = _repo.GetNoteByID(id);
                if(note==null)
                    return BadRequest("Not Found");
                var Identity = HttpContext.User.Identity as ClaimsIdentity;
                if (Identity != null)
                {
                    var userCliams = Identity.Claims;
                    SID = userCliams.FirstOrDefault(p => p.Type == ClaimTypes.Sid)?.Value;
                }
              
                if (SID != "" && SID != note.UserID)
                    return BadRequest("Not Allowed");
                note.Title = Newnote.Title;
                note.Tag = Newnote.Tag;
                note.Description = Newnote.Description;
                _repo.UpdateNote(note);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("deletenote/{id:int}")]
        public IActionResult Deletenote(int id)
        {
            try
            {
                string SID = "";
                Note note = _repo.GetNoteByID(id);
                if (note == null)
                    return BadRequest("Not Found");
                var Identity = HttpContext.User.Identity as ClaimsIdentity;
                if (Identity != null)
                {
                    var userCliams = Identity.Claims;
                    SID = userCliams.FirstOrDefault(p => p.Type == ClaimTypes.Sid)?.Value;
                }
                
                if(SID!="" && SID!=note.UserID)
                    return BadRequest("Not Allowed");

                _repo.DeleteNote(note);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}