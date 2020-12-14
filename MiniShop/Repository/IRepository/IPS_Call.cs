using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Repository.IRepository
{
    public interface ISP_Call : IDisposable
    {
        (bool success, string message) Excute(string proceduceName, DynamicParameters param = null);
        bool Login(string username, string password);
        
    }
}
