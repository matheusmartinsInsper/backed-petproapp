using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.User;

namespace app.Application.UseCase
{
    public class SignupUserClinic
    {
        private IRepositoryUserClinic _repository;
        private IRepositoryNetWork _repositorynet;
        public SignupUserClinic(IRepositoryUserClinic repository,IRepositoryNetWork repositorynet)
        {
            _repository = repository;
            _repositorynet = repositorynet;
        }
        public async Task<User> execute(UserClinic usertutor)
        {
            User user = User.create(usertutor);
            NetWorkCollaborators network = NetWorkCollaborators.create(user.id,user.categoryCode);
            await _repository.save(user);
            await _repositorynet.save(network);
            return user;
        }
    }
}
