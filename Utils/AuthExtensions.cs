using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Utils
{
    public interface IAuthUser
    {
        Guid Id { get; set; }
        string Email { get; set; }
        byte RoleId { get; set; }
    }

    public static class AuthExtensions
    {
        public static string BuildJWT<UserType>(string issuer, string securityKey, UserType user) where UserType : class, IAuthUser
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //hash


            var permClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Email)
                };

            foreach (var item in user.GetType().GetProperties())
            {
                if (item.GetValue(user) == null)
                {
                    permClaims.Add(new Claim(item.Name, string.Empty)); 
                    continue;
                }
                permClaims.Add(new Claim(item.Name, item.GetValue(user)!.ToString()!));
            }

            var token = new JwtSecurityToken(issuer,
                           issuer,
                           permClaims,
                           expires: DateTime.Now.AddMinutes(60),
                           signingCredentials: credentials);
            var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt_token;
        }
    }
}
