using app.Application.IRepository;
using app.Domain.Agregate.Entities;
using app.Domain.DTO.User;

namespace app.Application.UseCase
{
    public class SignupUserTutor
    {
        private IRepositoryUserTutor _repository;
        public SignupUserTutor(IRepositoryUserTutor repository) 
        {
            _repository = repository;
        }
        public async Task<User> execute(UserTutor usertutor)
        {
            User user = User.create(usertutor);
            await _repository.save(user);
            return user;
        }
    }
}
