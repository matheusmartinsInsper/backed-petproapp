using app.Application.UseCase;
using app.Infra.Repository.FactoryContext;
using app.Infra.Repository;
using Microsoft.AspNetCore.Mvc;
using app.Domain.DTO.Service;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using app.Application.IAuth;
using app.Application.IRepository;
using app.WebUI.DTO;
using app.Domain.Agregate.Entities;
using app.Application.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryService _reposervice;
        private IRepositoryNetWork _reponet;
        public ServiceController(IRepositoryUserClinic repoclinic, IRepositoryService reposervice, IRepositoryNetWork reponet)
        {
            _repoclinic = repoclinic;
            _reposervice = reposervice;
            _reponet = reponet;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateService([FromBody] ServiceInputController service)
        {
            try
            {
                CreateService usecase = new CreateService(_reposervice,_repoclinic);
                ServiceDTO serviceDTO = new ServiceDTO 
                { 
                    codecategory = service.CodigoDaCategoria.ToString(),
                    idUser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last(),
                    description = service.Descrição,
                    price = service.Preço,
                    title = service.Titulo,
                    vaccines = service.CodigoDeVacinas,
                    subcategories = service.SubCategories,
                    typeofatendimento = service.Atendimento
                };
                await usecase.execute(serviceDTO);
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
        public async Task<ActionResult> GetByID([FromQuery(Name ="idservice")] string idservice)
        {
            try
            {
                GetServiceByID usecase = new GetServiceByID(_reposervice,_reponet);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                ServiceOutputDTO service = await usecase.execute(iduser,idservice);
                var data = new { status = "confirmed", data = service };
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
        public async Task<ActionResult> GetByUser([FromQuery(Name = "posted")] bool posted)
        {
            try
            {
                GetServicesByUser usecase = new GetServicesByUser(_reposervice);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                List<ServiceOutputDTO> services = await usecase.execute(iduser,posted);
                var data = new { status = "confirmed", data = services};
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
        public async Task<ActionResult> GetByNetWork([FromQuery(Name = "posted")] bool posted)
        {
            try
            {
                GetServicesByNetWork usecase = new GetServicesByNetWork(_reposervice,_reponet);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                List<ServiceOutputDTO> services = await usecase.execute(iduser,posted);
                var data = new { status = "confirmed", data = services };
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
        [HttpPost("Post")]
        [Authorize]
        public async Task<ActionResult> Postservice([FromQuery(Name = "idservice")] string idservice)
        {
            try
            {
                PostService usecase = new PostService(_reposervice, _repoclinic);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                await usecase.execute(idservice, iduser);
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

    }
}
