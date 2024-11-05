using app.Application.DTO;
using app.Application.IAuth;
using app.Application.IRepository;
using app.Application.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorsController : ControllerBase
    {
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryUserCollaborator _repocollaborator;
        private ITokenService _tokenService;
        public CollaboratorsController(IRepositoryUserClinic repoclinic, ITokenService tokeservice, IRepositoryUserCollaborator repocolaborator)
        {
            _repoclinic = repoclinic;
            _repocollaborator = repocolaborator;
            _tokenService = tokeservice;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetMyCollaboratos()
        {
            try
            {
                GetCollaborators usecase = new GetCollaborators(_repoclinic, _repocollaborator);
                string iduserclinic = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                List<CollaboratorDTO> collaborators = await usecase.execute(iduserclinic);
                var data = new { status = "confirmed", data = collaborators };
                return Ok(data);
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
