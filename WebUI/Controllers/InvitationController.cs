using app.Application.DTO;
using app.Application.IRepository;
using app.Application.UseCase;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.InviteCollaborator;
using app.Domain.DTO.Service;
using app.WebUI.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        private IRepositoryUserCollaborator _repocollaborator;
        private IRepositoryUserClinic _repoclinic;
        private IRepositoryNetWork _reponetwork;
        private IRepositoryInvitation _repoinvitation;
        public InvitationController(IRepositoryInvitation repoinvite,IRepositoryUserClinic repoclinic, IRepositoryNetWork reponet, IRepositoryUserCollaborator repocollaborator)
        {
            _repoclinic = repoclinic;
            _reponetwork = reponet;
            _repocollaborator = repocollaborator;
            _repoinvitation = repoinvite;
        }
        [HttpPost("Send")]
        [Authorize]
        public async Task<ActionResult> CreateInvite([FromQuery(Name = "Email")] string email)
        {
            try
            {
                SendInviteToCollaborator usecase = new SendInviteToCollaborator(_repoinvitation, _repoclinic, _repocollaborator);
                InviteDTO inviteDTO = new InviteDTO()
                {
                    EmailUserCollaborator = email,
                    idUserSender = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last()
                };
                await usecase.execute(inviteDTO);
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
        public async Task<ActionResult> GetAllInvitations()
        {
            try
            {
                GetInvitationsOfUser usecase = new GetInvitationsOfUser(_repoinvitation, _repoclinic,_repocollaborator);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                List<InvitationOutput> invitations = await usecase.execute(iduser);
                var data = new { status = "confirmed",data = invitations };
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
        [HttpPut("Accepted")]
        [Authorize]
        public async Task<ActionResult> AcceptedInvitation([FromQuery(Name ="idinvitation")] string idinvitation)
        {
            try
            {
                AcceptInvitation usecase = new AcceptInvitation(_repoinvitation,_repocollaborator,_reponetwork,_repoclinic);
                string iduserrecipient = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                await usecase.execute(idinvitation,iduserrecipient);
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
        public async Task<ActionResult> RejectInvitation([FromQuery(Name = "idinvitation")] string idinvitation)
        {
            try
            {
                RejectInvitation usecase = new RejectInvitation(_repoinvitation, _repocollaborator);
                string iduserrecipient = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                await usecase.execute(idinvitation, iduserrecipient);
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
        [HttpPut("Cancelled")]
        [Authorize]
        public async Task<ActionResult> CancelledInvitation([FromQuery(Name = "idinvitation")] string idinvitation)
        {
            try
            {
                CancelInvitation usecase = new CancelInvitation(_repoclinic, _repoinvitation);
                string iduserrecipient = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                await usecase.execute(idinvitation, iduserrecipient);
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
