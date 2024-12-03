using app.Application.UseCase;
using app.Domain.Agregate.Entities;
using app.Infra.Repository.FactoryContext;
using app.Infra.Repository;
using Microsoft.AspNetCore.Mvc;
using app.Domain.DTO.OrderService;
using app.Application.IRepository;
using app.WebUI.DTO;
using Microsoft.AspNetCore.Authorization;
using app.Application.DTO;
using app.Application.IAuth;
using app.Application.GatewayService.WppAPI;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderServiceController : ControllerBase
    {
        private IRepositoryOrderService _repoos;
        private IRepositoryService _reposervice;
        private IRepositoryUserTutor _repotutor;
        private IRepositoryUserCollaborator _repocollaborator;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryPet _repopet;
        private IRepositoryNetWork _reponet;
        private ITokenService _tokenservice;
        private IRepositoryPortfolioClient _repoport;
        private IRepositoryProntuario _repoprontuario;
        private IMessage _message;
        public OrderServiceController(IMessage message, IRepositoryProntuario repoprontuario, IRepositoryPortfolioClient repoport, ITokenService tokenservice, IRepositoryPet repopet, IRepositoryOrderService repoos, IRepositoryService reposervice,IRepositoryUserTutor repotutor,IRepositoryNetWork reponet,IRepositoryUserClinic repoclinic, IRepositoryUserCollaborator repocollaborator)
        {
            _repoos = repoos;   
            _reposervice = reposervice;
            _repotutor = repotutor;
            _repoclinic = repoclinic;
            _repocollaborator = repocollaborator;
            _repopet = repopet;
            _reponet = reponet;
            _tokenservice = tokenservice;
            _repoport = repoport;
            _repoprontuario = repoprontuario;
            _message = message;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] OrderServiceInputController order)
        {
            try
            {
                CreateOrderService usecase = new CreateOrderService(_repoprontuario,_repopet, _repoclinic, _repoos,_reposervice,_repotutor,_repoport);
                string idtutor = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                await usecase.execute(order,idtutor);
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
        [HttpPost("Manually")]
        [Authorize]
        public async Task<ActionResult> CreateOSManually([FromBody] InputOSWithoutUserTutor order)
        {
            try
            {
                CreateOSManually usecase = new CreateOSManually(_repoclinic, _repoprontuario, _repoport,_repopet, _repoos, _reposervice, _repotutor,_tokenservice);
                string idowner = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                await usecase.execute(order, idowner);
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
        [HttpPost("Manually/WithTutor")]
        [Authorize]
        public async Task<ActionResult> CreateOSManuallyWithTutorRegister([FromBody] InputOSWithUserTutor order)
        {
            try
            {
                CreateOsManuallyWithTutor usecase = new CreateOsManuallyWithTutor(_repoprontuario, _reposervice,_repopet, _repocollaborator, _repoos, _repotutor);
                string idowner = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                await usecase.execute(order, idowner);
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
        [HttpGet("User")]
        [Authorize]
        public async Task<ActionResult> GetByUser([FromQuery(Name ="wasaccept")] bool wasaccept)
        {
            try
            {
                GetOSByUser usecase = new GetOSByUser(_repocollaborator, _repoclinic,_repoos, _reposervice,_repopet,_repotutor);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                List<OrderServiceOutputDTO> os = await usecase.execute(iduser, wasaccept);
                var data = new { status = "confirmed",data =  os};
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
        [HttpGet("NetWork")]
        [Authorize]
        public async Task<ActionResult> GetByNet([FromQuery(Name = "wasaccept")] bool wasaccept)
        {
            try
            {
                GetOsByNetWork usecase = new GetOsByNetWork(_repocollaborator,_repoclinic,_repoos, _reposervice, _repopet, _repotutor);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                List<OrderServiceOutputDTO> os = await usecase.execute(iduser, wasaccept);
                var data = new { status = "confirmed", data = os };
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
        [HttpGet("NetWork/All")]
        [Authorize]
        public async Task<ActionResult> GetAllByNet()
        {
            try
            {
                GetAllOSByNetWork usecase = new GetAllOSByNetWork(_repocollaborator, _repoclinic, _repoos, _reposervice, _repopet, _repotutor);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                List<OrderServiceOutputDTO> os = await usecase.execute(iduser);
                var data = new { status = "confirmed", data = os };
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
        [HttpGet("User/All")]
        [Authorize]
        public async Task<ActionResult> GetAllByUser()
        {
            try
            {
                GetAllOsByUser usecase = new GetAllOsByUser(_repocollaborator, _repoclinic, _repoos, _reposervice, _repopet, _repotutor);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                List<OrderServiceOutputDTO> os = await usecase.execute(iduser);
                var data = new { status = "confirmed", data = os };
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
        [HttpPut("Accept")]
        [Authorize]
        public async Task<ActionResult> Accept([FromQuery(Name = "idos")] string idos, [FromQuery(Name = "emailuserattendance")] string emailuserattendance)
        {
            try
            {
                AcceptOrderService usecase = new AcceptOrderService(_message, _repotutor, _reposervice, _repoos, _reponet,_repocollaborator,_repoclinic);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                await usecase.execute(iduser, idos, emailuserattendance);
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
        [HttpPut("Reject")]
        [Authorize]
        public async Task<ActionResult> Reject([FromQuery(Name = "idos")] string idos)
        {
            try
            {
                RejectOrderService usecase = new RejectOrderService(_repoos, _reponet);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                await usecase.execute(iduser, idos);
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
        [HttpPut("Cancel")]
        [Authorize]
        public async Task<ActionResult> Cancel([FromQuery(Name = "idos")] string idos)
        {
            try
            {
                CancelOrderService usecase = new CancelOrderService(_repoos, _reponet);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                await usecase.execute(iduser, idos);
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
