using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth
{
    public class JwtTokenManager
    {
        private readonly string _key;

        private readonly List<User> _users = new()
        {
            new User {UserName = "srinivas", Password = "myTestPassword", Role = "Admin"},
            new User {UserName = "prasad", Password = "myTestPassword", Role = "user"}
        };

        public JwtTokenManager(string key)
        {
            _key = key;
        }

        public string Authenticate(UserRequest user)
        {

            if (!_users.Any(x => x.UserName == user.UserName && x.Password == user.Password)) return string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new(ClaimTypes.Name, user.UserName),
                    new(ClaimTypes.Role, _users.First(x => x.UserName == user.UserName).Role),
                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
