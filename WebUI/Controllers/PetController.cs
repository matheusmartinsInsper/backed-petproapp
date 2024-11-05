using app.Application.DTO;
using app.Application.IRepository;
using app.Application.UseCase;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Pet;
using app.WebUI.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        private IRepositoryPet _repopet;
        private IRepositoryUserTutor _repotutor;
        public PetController(IRepositoryPet repopet, IRepositoryUserTutor repotutor)
        {
            _repopet = repopet;
            _repotutor = repotutor;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] PetDTO petdto)
        {
            try
            {
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                CreatePet usecase = new CreatePet(_repopet,_repotutor);
                await usecase.execute(petdto,iduser);
                var data = new { status = "confirmed"};
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
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<PetOutput>>> GetPets()
        {
            try
            {
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                FindPetsOfUser usecase = new FindPetsOfUser(_repopet);
                List<PetOutput> pets = await usecase.execute(iduser);
                return pets;
            }
            catch(Exception ex)
            {

                return BadRequest(new
                {
                    messageError = ex.Message
                });
            }
        }
    }
}
