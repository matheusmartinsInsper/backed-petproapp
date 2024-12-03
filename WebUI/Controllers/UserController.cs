using app.Application.DTO;
using app.Application.IRepository;
using app.Application.UseCase;
using app.Domain.DTO.Fone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryUserCollaborator _repocollaborator;
        private IRepositoryUserTutor _repotutor;
        public UserController(IRepositoryUserClinic repoclinic, IRepositoryUserCollaborator repocollaborator, IRepositoryUserTutor repotutor)
        {
            _repoclinic = repoclinic;
            _repotutor = repotutor;
            _repocollaborator = repocollaborator;
        }

        [HttpPost("Phone")]
        [Authorize]
        public async Task<ActionResult> addPhone([FromBody] PhoneDTOInput phone)
        {
            try{
                AddPhone usecase = new AddPhone(_repoclinic,_repocollaborator,_repotutor);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
               
                await usecase.execute(iduser, phone);
                return Ok(new { status = "Confirmado"});
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    messageError = ex.Message
                });
            }
        }

    }
}
