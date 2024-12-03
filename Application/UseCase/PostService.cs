using app.Application.IRepository;
using app.Domain.Agregate.Entities;

namespace app.Application.UseCase
{
    public class PostService
    {
        private IRepositoryService _reposervice;
        private IRepositoryUserClinic _repouser;
        public PostService(IRepositoryService reposervice, IRepositoryUserClinic repouser)
        {
            _reposervice = reposervice;
            _repouser = repouser;
        }
        public async Task execute(string idservice, string iduser)
        {
            Service service = await _reposervice.get(idservice);
            service.post();
            await _reposervice.update(service);
            return;
        }
    }
}
