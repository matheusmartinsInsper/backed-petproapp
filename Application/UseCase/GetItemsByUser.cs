using app.Application.DTO;
using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.Item;

namespace app.Application.UseCase
{
    public class GetItemsByUser
    {
        private IRepositoryItem _repoitem;
        public GetItemsByUser(IRepositoryItem repoitem)
        {
            _repoitem = repoitem;
        }
        public async Task<List<ItemDTOOutput>> execute(string iduser)
        {
            List<Item> items = await _repoitem.getbyiduser(iduser);  
            List<ItemDTOOutput> output = new List<ItemDTOOutput>();
            foreach (Item item in items)
            {
                ItemDTOOutput itemdto = new ItemDTOOutput();
                itemdto.sizes = new List<ItemSizeDTODb>();
                itemdto.itemid = item.itemid;
                itemdto.description = item.description;
                itemdto.name = item.name;
                itemdto.datecreate = item.datecreate;
                itemdto.category = item.category;
                itemdto.unity = item.unity;
                foreach(ItemSize size in item.sizes)
                {
                    ItemSizeDTODb sizedto = new ItemSizeDTODb();
                    sizedto.price = size.price;
                    sizedto.size = size.size;
                    sizedto.avalaible = size.avalaible;
                    sizedto.iditemsize = size.iditemsize;
                    sizedto.iditem = size.iditem;
                    itemdto.sizes.Add(sizedto);
                }
              output.Add(itemdto);
            }
            return output;
        }
    }
}
