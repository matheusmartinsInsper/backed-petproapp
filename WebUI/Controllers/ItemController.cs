using app.Application.DTO;
using app.Application.IRepository;
using app.Application.UseCase;
using app.Domain.DTO.Item;
using app.Domain.DTO.Stock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace app.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ItemController : ControllerBase
    {
        private IRepositoryStock _repostock;
        private IRepositoryItem _repoitem;
        public ItemController(IRepositoryItem repoitem, IRepositoryStock repostock)
        {
            _repostock = repostock;
            _repoitem = repoitem;
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> createItem([FromBody] ItemDTO item)
        {
            try
            {
                CreateItem usecase = new CreateItem(_repoitem);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                await usecase.execute(item, iduser);
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
        public async Task<ActionResult> getItems()
        {
            try
            {
                GetItemsByUser usecase = new GetItemsByUser(_repoitem);
                string iduser = User.Claims.FirstOrDefault(c => c.Type == "identifier").ToString().Split(" ").Last();
                List<ItemDTOOutput> items = await usecase.execute(iduser);
                var data = new { status = "confirmed",data = items };
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
