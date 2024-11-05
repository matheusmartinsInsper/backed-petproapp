using app.Domain.Agregate.Entities;

namespace app.Application.IAuth
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
