using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.User;

namespace app.Application.UseCase
{
    public class SignupUserCollaborator
    {
        private IRepositoryUserCollaborator _repository;
        public SignupUserCollaborator(IRepositoryUserCollaborator repository)
        {
            _repository = repository;
        }
        public async Task<User> execute(UserCollaborator usertutor)
        {
            User user = User.create(usertutor);
            await _repository.save(user);
            return user;
        }
    }
}
