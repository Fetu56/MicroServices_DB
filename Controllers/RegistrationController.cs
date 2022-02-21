using BCrypt;
using MicroServices_DB.LogicClass;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace MicroServices_DB.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RegistrationController : ControllerBase
    {
        SqlConnection connection;

        private readonly ILogger<RegistrationController> _logger;


        public RegistrationController(ILogger<RegistrationController> logger)
        {
            _logger = logger;
            connection = ConnectionSigleton.Instance.connection;
        }

        [HttpGet(Name = "GetIsExist")]
        public IActionResult Get(string email)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(email);
                SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) [email] FROM [USERS] WHERE [email] = '{email}';", connection);
                
                return Ok((int)cmd.ExecuteScalar() > 0);
            }
            catch (ArgumentNullException) { }
            return Problem(null, null, 404);
        }
        [HttpPost(Name ="Registration")]
        public IActionResult Registration(string email, string pass, int birth_day, int birth_month, int birth_year, string secret = "1")
        {
            try
            {
                ArgumentNullException.ThrowIfNull(email);
                ArgumentNullException.ThrowIfNull(pass);
                if (!IsValidEmail(email) || (Boolean)((ObjectResult)Get(email)).Value)
                {
                    throw new ArgumentException("Invalide email");
                }

                var hash = BCryptHelper.HashPassword(pass, BCryptHelper.GenerateSalt());
                int role = (int)new SqlCommand($"SELECT [id] FROM [ROLES] WHERE [secret] = '{secret}'", connection).ExecuteScalar();

                SqlCommand cmd = new SqlCommand($"INSERT INTO [USERS] VALUES('{email}', '{hash}', {role}, '{String.Format("{0}-{1}-{2}", birth_day, birth_month, birth_year)}', '');", connection);

                if(cmd.ExecuteNonQuery() != 1)
                {
                    return Ok(false);
                }

                return Ok(true);
            }
            catch (Exception) { }
            
            return Problem();
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                return new System.Net.Mail.MailAddress(email).Address == email;
            }
            catch(Exception) {  }
            return false;
        }
    }
}