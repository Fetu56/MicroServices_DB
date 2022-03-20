using System.Data.SqlClient;

namespace MicroServices_DB.LogicClass
{
    public static class TokenCheck
    {
        public static bool Check(string token)
        {
            bool result = false;
            if(token == ProgrammValues.SecretToken)
            {
                result = true;
            }
            else
            {
                try
                {
                    SqlConnection connection = ConnectionSigleton.Instance.connection;
                    SqlCommand command = new SqlCommand($"SELECT COUNT(*) from [USERS] WHERE authtoken like '{token}'", connection);
                    if ((int)command.ExecuteScalar() > 0)
                    {
                        result = true;
                    }
                }
                catch (Exception) { }
            }
            return result;
        }

    }
}
