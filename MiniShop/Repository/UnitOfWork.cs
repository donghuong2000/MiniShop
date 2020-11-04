using MiniShop.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork()
        {
            SP_Call = new SP_Call();
        }
        public ISP_Call SP_Call { get; private set; }

        public void Dispose()
        {
            
        }

        public void Save()
        {
            
        }
    }
}
