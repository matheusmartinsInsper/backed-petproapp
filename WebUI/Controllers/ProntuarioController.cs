using app.Application.DTO;
using app.Application.IRepository;
using app.Application.UseCase;
using app.Domain.Agregate.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProntuarioController : ControllerBase
    {
        private IRepositoryOrderService _repository;
        private IRepositoryService _reposervice;
        private IRepositoryUserTutor _repouser;
        private IRepositoryPortfolioClient _repoport;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryPet _repopet;
        private IRepositoryProntuario _repoprontuario;
        public ProntuarioController(IRepositoryProntuario repoprontuario, IRepositoryPet repopet, IRepositoryUserClinic repoclinic, IRepositoryOrderService repository, IRepositoryService reposervice, IRepositoryUserTutor repouser, IRepositoryPortfolioClient repoport)
        {
            _repository = repository;
            _reposervice = reposervice;
            _repouser = repouser;
            _repoport = repoport;
            _repoclinic = repoclinic;
            _repopet = repopet;
            _repoprontuario = repoprontuario;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetProntuarios()
        {
            try
            {
                GetProntuarioByUser usecase = new GetProntuarioByUser(_repoprontuario, _repopet,_repoclinic,_repository,_reposervice,_repouser,_repoport);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                List< OutputProntuarioDTO> prontuarios = await usecase.execute(iduser);
                var data = new { status = "confirmed", data = prontuarios };
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
