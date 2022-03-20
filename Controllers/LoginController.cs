using BCrypt;
using MicroServices_DB.LogicClass;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MicroServices_DB.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : Controller
    {
        SqlConnection connection;
        public LoginController(ILogger<RegistrationController> logger)
        {
            connection = ConnectionSigleton.Instance.connection;
        }

        [HttpPost(Name = "Login")]
        public IActionResult Login(string email, string pass)
        {
            try
            {
                SqlCommand cmd = new SqlCommand($"SELECT [passhash] FROM [USERS] WHERE [email] = '{email}'", connection);
                if (BCryptHelper.CheckPassword(pass, (string)cmd.ExecuteScalar()))
                {
                    var authToken = GetToken(email, pass);
                    SqlCommand upd = new SqlCommand($"UPDATE [USERS] SET [authtoken] = '{authToken}' WHERE [email] = '{email}'", connection);
                    upd.ExecuteNonQuery();
                    EmailSender.Send(authToken, email);
                    return Ok($"Token sended to email - {email}");
                }
            }
            catch(Exception) { }
            return Ok(false);
        }

        private string GetToken(string email, string pass)
        {
            var claim = getClaim(email, pass);
            if(claim == null)
            {
                throw new ArgumentException();
            }

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: DateTime.UtcNow,
                claims: claim.Claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
        

        private ClaimsIdentity getClaim(string email, string pass)
        {
            var cl = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email),
                new Claim(ClaimsIdentity.DefaultNameClaimType, pass)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(cl, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
