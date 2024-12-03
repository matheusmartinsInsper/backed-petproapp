using app.Application.DTO;
using app.Application.IRepository;
using app.Application.UseCase;
using app.Domain.DTO.Attendance;
using app.WebUI.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private IRepositoryAttendance _repoattendance;
        private IRepositoryFile _repofile;
        private IRepositoryUserTutor _repotutor;
        private IRepositoryPet _repopet;
        private IRepositoryOrderService _repoos;
        private IRepositoryForm _repoform;
        private IRepositoryService _reposervice;
        private IRepositoryProntuario _repoprontuario;
        public AttendanceController(IRepositoryProntuario repoprontuario, IRepositoryService reposervice, IRepositoryForm repoform, IRepositoryUserTutor repotutor, IRepositoryPet repopet, IRepositoryOrderService repoos, IRepositoryAttendance repoattendance, IRepositoryFile repofile) 
        { 
            _repoattendance = repoattendance;
            _repofile = repofile;
            _repotutor = repotutor;
            _repoos = repoos;
            _repopet = repopet;
            _repoform = repoform;
            _reposervice = reposervice;
            _repoprontuario = repoprontuario;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Createattendance([FromQuery(Name ="idos")] string idos)
        {
            try
            {
                CreateAttendance usecase = new CreateAttendance(_repoprontuario, _repotutor,_repopet,_repoos,_repoattendance,_repofile);
                string iduserattendance = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                OuputAttendanceCreate output =  await usecase.execute(idos, iduserattendance);
                var data = new { status = "confirmed",data = output};
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
        [HttpPut("Conclude")]
        [Authorize]
        public async Task<IActionResult> ConcludeAttendaceRoute([FromForm] List<IFormFile> files, [FromForm] string inputdata)
        {
            try
            {
                InputConcludeAttendance attendance = JsonConvert.DeserializeObject<InputConcludeAttendance>(inputdata);
                ConcludeAttendance usecase = new ConcludeAttendance(_repoform, _repotutor, _repopet, _repoos, _repoattendance, _repofile);
                string iduserattendance = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                InputAttendanceConclude dtoinputusecase = new InputAttendanceConclude();
                dtoinputusecase.files = files;
                dtoinputusecase.hipotese = attendance.hipotese;
                dtoinputusecase.conclusao = attendance.conclusao;
                dtoinputusecase.iduserattendance = iduserattendance;
                dtoinputusecase.idorderservice = attendance.idorderservice;
                dtoinputusecase.idattendance = attendance.idattendance;
                await usecase.execute(dtoinputusecase);
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
        public async Task<IActionResult> GetAttendance([FromQuery(Name = "idattendance")] string idattendance)
        {
            try
            {
                GetAttendanceById usecase = new GetAttendanceById(_reposervice, _repotutor, _repopet, _repoos, _repoattendance, _repofile);
                string iduserattendance = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                AttendanceOutPut att = await usecase.execute(idattendance,iduserattendance);
                var data = new { status = "confirmed",data = att };
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
