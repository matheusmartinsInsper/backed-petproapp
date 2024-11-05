using app.Application.IRepository;
using app.Application.UseCase;
using app.Domain.Agregate.ObjectValues;
using app.Domain.DTO.InviteCollaborator;
using app.Domain.DTO.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetWorkController : ControllerBase
    {
        private IRepositoryUserCollaborator _repocollaborator;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryNetWork _reponetwork;
        private IRepositoryInvitation _repoinvitation;
        private IRepositoryOrderService _repoos;
        public NetWorkController(IRepositoryOrderService repoos, IRepositoryInvitation repoinvite, IRepositoryUserClinic repoclinic, IRepositoryNetWork reponet, IRepositoryUserCollaborator repocollaborator)
        {
            _repoclinic = repoclinic;
            _reponetwork = reponet;
            _repocollaborator = repocollaborator;
            _repoinvitation = repoinvite;
            _repoos = repoos;
        }

        [HttpPut("Remove")]
        [Authorize]
        public async Task<ActionResult> RemoveCollaborator([FromQuery(Name = "Email")] string Email)
        {
            try
            {
                RemoveCollaboratorToNetWork usecase = new RemoveCollaboratorToNetWork(_repocollaborator, _reponetwork, _repoclinic);
                string iduserclinic = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                await usecase.execute(iduserclinic, Email);
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
        [HttpGet("Members")]
        public async Task<ActionResult<List<UserBaseDTO>>> GetMembers([FromQuery(Name = "idos")] string idos)
        {
            try
            {
                GetMemberOfNetwork usecase = new GetMemberOfNetwork(_reponetwork, _repoos, _repoclinic);
                List<UserBaseDTO> members = await usecase.execute(idos);
                return Ok(members);
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
