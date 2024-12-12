using app.Application.DTO;
using app.Application.IRepository;
using app.Application.UseCase;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Stock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private IRepositoryStock _repostock;
        private IRepositoryItem _repoitem;
        public StockController(IRepositoryItem repoitem, IRepositoryStock repostock)
        {
            _repostock = repostock;
            _repoitem = repoitem;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> moveStock([FromBody] TransactionDTO transaction)
        {
            try
            {
                MoveStock usecase = new MoveStock(_repostock, _repoitem);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                await usecase.execute(transaction,iduser);
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
        public async Task<ActionResult> getStock()
        {
            try
            {
                GetStockByUser usecase = new GetStockByUser(_repostock, _repoitem);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                List<StockOutputcs> stocks =   await usecase.execute(iduser);
                var data = new { status = "confirmed", data = stocks };
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
        [HttpGet("Transactions")]
        [Authorize]
        public async Task<ActionResult> getMovimentation([FromBody] TransactionDTO transaction)
        {
            try
            {
                MoveStock usecase = new MoveStock(_repostock, _repoitem);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                await usecase.execute(transaction, iduser);
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
