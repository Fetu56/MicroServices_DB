using BCrypt;
using MicroServices_DB.LogicClass;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

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
                    var allChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    var random = new Random();
                    var resultToken = new string(Enumerable.Repeat(allChar, 60).Select(token => token[random.Next(token.Length)]).ToArray());
                    string authToken = "BD8" + resultToken.ToString();

                    SqlCommand upd = new SqlCommand($"UPDATE [USERS] SET [authtoken] = '{authToken}' WHERE [email] = '{email}'", connection);
                    upd.ExecuteNonQuery();
                    return Ok(authToken);
                }
            }
            catch(Exception) { }
            return Ok(false);
        }
    }
}
