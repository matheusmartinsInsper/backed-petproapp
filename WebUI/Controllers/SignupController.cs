using app.Application.IRepository;
using app.Application.UseCase;
using app.Domain.DTO.User;
using app.Infra.Repository;
using app.Infra.Repository.FactoryContext;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignupController : ControllerBase
    {
        private IRepositoryUserTutor _repotutor;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryUserCollaborator _repocollaborator;
        private IRepositoryNetWork _netWork;
        public SignupController(IRepositoryUserTutor repotutor,IRepositoryUserClinic repoclinic,IRepositoryUserCollaborator repocollaborator,IRepositoryNetWork network)
        {
            _repotutor = repotutor;
            _repoclinic = repoclinic;
            _repocollaborator = repocollaborator;
            _netWork = network;
        }
        [HttpPost("Tutor")]
        public async Task<ActionResult> CreateTutor([FromBody] UserTutor user)
        {
            try
            {
                SignupUserTutor usecase = new SignupUserTutor(_repotutor);
                await usecase.execute(user);
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
        [HttpPost("Clinic")]
        public async Task<ActionResult> CreateClinic([FromBody] UserClinic user)
        {
            try
            {
                SignupUserClinic usecase = new SignupUserClinic(_repoclinic,_netWork);
                await usecase.execute(user);
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
        [HttpPost("Collaborator")]
        public async Task<ActionResult> CreateCollaborator([FromBody] UserCollaborator user)
        {
            try
            {
                SignupUserCollaborator usecase = new SignupUserCollaborator(_repocollaborator);
                await usecase.execute(user);
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
    }
}
