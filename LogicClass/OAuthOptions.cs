using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MicroServices_DB.LogicClass
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer";
        public const string AUDIENCE = "MyAtuthClient";
        private const string KEY = "my_secret_key123";
        public const int MAX_LIFE = 1;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
