using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth
{
    public class JwtTokenManager
    {
        private readonly string _key;
        private readonly IEnumerable<User> _users;

        public JwtTokenManager(string key)
        {
            _key = key;
            _users = UserList.AllUsers;
        }

        public string Authenticate(UserRequest user)
        {
            var authUser = _users.SingleOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
            if (authUser is null) return string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new(ClaimTypes.Name, authUser.UserName),
                    new(ClaimTypes.Role, authUser.Role),
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
