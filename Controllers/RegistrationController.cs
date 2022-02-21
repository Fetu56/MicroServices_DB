using BCrypt;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace MicroServices_DB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        SqlConnection connection;

        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(ILogger<RegistrationController> logger)
        {
            _logger = logger;
            connection = new SqlConnection("Server=tcp:fet.database.windows.net,1433;Initial Catalog=fet;Persist Security Info=False;User ID=fet;Password=EgorPrivet123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            connection.Open();
        }

        [HttpGet(Name = "GetIsExist")]
        public IActionResult Get(string email)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(email);
                SqlCommand cmd = new SqlCommand($"IF EXISTS(SELECT * FROM [USERS] WHERE [email] = '{email}')  ");
                
                return Ok(cmd.ExecuteNonQuery());
            }
            catch (ArgumentNullException) { }
            return Problem();
        }
        [HttpPost(Name ="Registration")]
        public IActionResult CheckPass(string email, string pass, string bdate, string secret = "1")
        {
            try
            {
                ArgumentNullException.ThrowIfNull(email);
                ArgumentNullException.ThrowIfNull(pass);

                //check if email arleady exist

                var hash = BCryptHelper.HashPassword(pass, BCryptHelper.GenerateSalt());
                int role = (int)new SqlCommand($"SELECT [id] FROM [ROLES] WHERE [secret] = '{secret}'", connection).ExecuteScalar();

                var date = DateOnly.Parse(bdate);
                SqlCommand cmd = new SqlCommand($"INSERT INTO [USERS] VALUES('{email}', '{hash}', {role}, {date})");

                return Ok(true);
            }
            catch (Exception) { }
            
            return Problem();
        }
    }
}