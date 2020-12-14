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


        public static string ConnectionString = "Server=.;Database=NHOM3;Trusted_Connection=True;";

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
                    var obj = sqlConnection.ExecuteReader(proceduceName, param, 
                        commandType: System.Data.CommandType.StoredProcedure);
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

        public bool Login(string username,string password)
        {
            var newConnnect = "Server=.;Database=Nhom3;User Id=" + username + ";Password=" + password + ";";
            using (SqlConnection sqlConnection = new SqlConnection(newConnnect))
            {
                
                try
                {
                    sqlConnection.Open();
                    var obj = sqlConnection.ExecuteReader("SELECT name FROM master.dbo.sysdatabases");
                        
                    var r = "";
                    while (obj.Read())
                    {
                        var a = obj.GetValue(0);
                        r += a.ToString();

                    }

                    // chuẩn hóa đầu ra
                    r = "{\"data\":" + r + "}";
                    return true ;
                }
                catch (Exception e)
                {

                    return false;
                }
            }
        }
    }
}
