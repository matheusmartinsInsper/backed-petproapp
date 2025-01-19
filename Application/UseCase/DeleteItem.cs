using app.Application.IRepository;
using app.Domain.Agregate.Entities;

namespace app.Application.UseCase
{
    public class DeleteItem
    {
        private IRepositoryItem _repoitem;
        public DeleteItem(IRepositoryItem repoitem)
        {
            _repoitem = repoitem;
        }
        public async Task execute(string iditem,string iduser)
        {
            Item item = await _repoitem.getbyid(iditem);
            if(item.iduser == iduser)
            {
                item.exclude();
                await _repoitem.udpate(item);
                return;
            }
            throw new Exception("Usuario sem permissão para deletar item");
        }
    }
}
