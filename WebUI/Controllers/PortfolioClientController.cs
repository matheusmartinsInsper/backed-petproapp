using app.Application.DTO;
using app.Application.IRepository;
using app.Application.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioClientController : ControllerBase
    {
        private IRepositoryUserTutor _repotutor;
        private IRepositoryPortfolioClient _repoport;
        private IRepositoryUserCollaborator _repocollaborator;
        private IRepositoryPet _repopet;
        private IRepositoryUserClinic _repoclinic;
        public PortfolioClientController(IRepositoryUserClinic repoclinic, IRepositoryPet repopet, IRepositoryUserTutor repotutor, IRepositoryPortfolioClient repoport, IRepositoryUserCollaborator repocollaborator)
        {
            _repotutor = repotutor;
            _repoport = repoport;
            _repocollaborator = repocollaborator;
            _repopet = repopet;
            _repoclinic = repoclinic;
        }

        // POST api/<ValuesController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddClient([FromQuery(Name ="emailclient")] string emailclient)
        {
            try
            {
                AddClientAtPortfolio usecase = new AddClientAtPortfolio(_repoclinic, _repotutor,_repoport,_repocollaborator);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                await usecase.execute(emailclient, iduser);
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
        [HttpPut]
        [Authorize]
        public async Task<ActionResult> RemoveClient([FromQuery(Name = "emailclient")] string emailclient)
        {
            try
            {
                RemoveClienteToPortifolio usecase = new RemoveClienteToPortifolio(_repoclinic, _repotutor, _repoport);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                await usecase.execute(emailclient, iduser);
                var data = new { status = "confirmed" };
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
        public async Task<ActionResult> GetClients()
        {
            try
            {
                GetClients usecase = new GetClients(_repotutor, _repopet, _repoport);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                List<OutPutClientDTO> clients = await usecase.execute(iduser);
                var data = new { status = "confirmed",data = clients };
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
