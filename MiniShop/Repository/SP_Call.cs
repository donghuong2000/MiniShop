using Dapper;
using MiniShop.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace MiniShop.Repository
{
    public class SP_Call : ISP_Call
    {

        private static string ConnectionString = "Server=dbms.czhgxs9hoqoj.ap-southeast-1.rds.amazonaws.com,1433;Database=NHOM3;User Id=admin;Password=Hoilamj0123!";
        public SP_Call()
        {

        }

        public void Dispose()
        {

        }
        public (bool success,string message) Excute(string proceduceName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                try
                {
                    var obj = sqlConnection.ExecuteReader(proceduceName, param, commandType: System.Data.CommandType.StoredProcedure);
                    var r = "";
                    while (obj.Read())
                    {
                        var a = obj.GetValue(0);
                        r += a.ToString();

                    }

                    // chuẩn hóa đầu ra
                    r = "{\"data\":" + r + "}";
                    return (true, r);
                }
                catch (Exception e)
                {

                    return (false, e.Message);
                }
                
                
                
            }
        }

    }
}
