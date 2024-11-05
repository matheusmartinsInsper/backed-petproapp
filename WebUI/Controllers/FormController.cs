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
        public FormController(IRepositoryForm repoform)
        {
            _repoform = repoform;
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
    }
}
