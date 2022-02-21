using System.Data.SqlClient;

namespace MicroServices_DB.LogicClass
{
    public class ConnectionSigleton
    {
        private static readonly object lck = new object();
        private static ConnectionSigleton instance = null;
        public static ConnectionSigleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lck)
                    {
                        if (instance == null)
                        {
                            instance = new ConnectionSigleton();
                        }
                    }
                }
                return instance;
            }
        }
        public SqlConnection connection;
        ConnectionSigleton()
        {
            connection = new SqlConnection("Server=tcp:fet.database.windows.net,1433;Initial Catalog=fet;Persist Security Info=False;User ID=fet;Password=EgorPrivet123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            connection.Open();
        }
        
    }
}
