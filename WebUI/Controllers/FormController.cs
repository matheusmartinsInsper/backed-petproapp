using app.Application.IRepository;
using app.Application.UseCase;
using app.Domain.DTO.Form;
using app.Domain.DTO.InviteCollaborator;
using app.WebUI.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormController : ControllerBase
    {
        private IRepositoryForm _repoform;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryAttendance _repoattendance;
        public FormController(IRepositoryForm repoform, IRepositoryAttendance repoattendance, IRepositoryUserClinic repoclinic)
        {
            _repoform = repoform;
            _repoclinic = repoclinic;
            _repoattendance = repoattendance;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateFormFromUser([FromBody] FormDTO formdto)
        {
            try
            {
                CreateForm usecase = new CreateForm(_repoform);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                await usecase.execute(formdto, iduser);
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
        public async Task<ActionResult> GetFormByIdUser()
        {
            try
            {
                GetFormByUser usecase = new GetFormByUser(_repoform);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                List<FormDbDTO> forms =  await usecase.execute(iduser);
                var data = new { status = "confirmed",data =  forms};
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
        [HttpPost("Save/Anamnese")]
        [Authorize]
        public async Task<ActionResult> CreateInstanceForm([FromBody] InputSaveAnamnese input)
        {
            try
            {
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                SaveAnamnese usecase = new SaveAnamnese(_repoattendance,_repoform,_repoclinic);
                await usecase.execute(iduser,input.values);
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
        [HttpGet("Get/Anamnese")]
        [Authorize]
        public async Task<ActionResult> GetInstanceForm([FromQuery(Name ="idattendance")] string idattendance, [FromQuery(Name = "idform")] string idform)
        {
            try
            {
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                GetAnamneseByAttendance usecase = new GetAnamneseByAttendance(_repoform);
                FormDbDTO form = await usecase.execute(iduser, idattendance,idform);
                var data = new { status = "confirmed",data = form };
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
