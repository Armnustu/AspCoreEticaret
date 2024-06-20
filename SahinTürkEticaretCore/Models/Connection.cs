using JetBrains.Annotations;
using Microsoft.Data.SqlClient;

namespace SahinTürkEticaretCore.Models
{
    public class Connection
    {
        public static SqlConnection ServerConnect
        {
           get{
                SqlConnection sqlcon = new SqlConnection("Server=DESKTOP-7GJPC0B\\SQLEXPRESS;Database=SahinTurkCoreProject;TrustServerCertificate=True;Trusted_Connection=True;");
                return sqlcon;
            }
        }
    }
}
