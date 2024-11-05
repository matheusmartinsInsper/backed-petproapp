using app.Application.IAuth;
using app.Domain.Agregate.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace app.Infra.Auth
{
    public class TokenService:ITokenService
    {
        // private string keysecret = Environment.GetEnvironmentVariable("KEYSECRETTOKEN");
        private string keysecret = "ksksjffdfd74645834745hfhdhdfhdf8899889";
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] key= Encoding.ASCII.GetBytes(keysecret);
            var tokenDescriptior = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []{
                  new Claim("identifier",user.id.ToString()),
                  new Claim(ClaimTypes.Role,user.categoryCode.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptior);
            return tokenHandler.WriteToken(token);
        }
        public void Refreshtoken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(keysecret);
        }
    }
}
