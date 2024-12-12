using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Item;

namespace app.Application.UseCase
{
    public class CreateItem
    {
        private IRepositoryItem _repoitem;
        public CreateItem(IRepositoryItem repoitem)
        {
            _repoitem = repoitem;
        }
        public async Task execute(ItemDTO itemdto, string iduser)
        {
            Item item = Item.create(itemdto,iduser);
            await _repoitem.save(item);
            return;
        }
    }
}
