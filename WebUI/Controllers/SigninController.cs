using app.Application.DTO;
using app.Application.IAuth;
using app.Application.IRepository;
using app.Application.UseCase;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.User;
using app.Infra.Auth;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SigninController : ControllerBase
    {
        private IRepositoryUserTutor _repotutor;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryUserCollaborator _repocollaborator;
        private ITokenService _tokenService;
        public SigninController(IRepositoryUserTutor repotutor,IRepositoryUserClinic repoclinic, ITokenService tokeservice, IRepositoryUserCollaborator repocolaborator)
        {
            _repotutor = repotutor;
            _repoclinic = repoclinic;
            _repocollaborator = repocolaborator;
            _tokenService = tokeservice;
        }
        [HttpPost("Tutor")]
        public async Task<ActionResult> SigninUserTutor([FromBody] SigninUserDTO user)
        {
            try
            {
                SigninUserTutor sigin = new SigninUserTutor(_repotutor,_tokenService);
                string token = await sigin.execute(user.email,user.password);
                Response.Headers.Add("Token", token);
                var res = new
                {
                    status = "Ok",
                    type = "Tutor"
                };
                return Ok(res);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Platform")]
        public async Task<ActionResult> SigninUserPlatform([FromBody] SigninUserDTO user)
        {
            try
            {
                SigninUserPlatform usecase = new SigninUserPlatform(_repoclinic, _tokenService,_repocollaborator);
                OutPutSignin output = await usecase.execute(user.email, user.password);
                Response.Headers.Add("Token", output.token);
                return Ok(output);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
