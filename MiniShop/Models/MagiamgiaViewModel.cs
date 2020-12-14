using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Models
{
    public class MagiamgiaViewModel
    {
        public MagiamgiaViewModel()
        {

            DateStart = DateTime.Now;
            DateEnd = DateTime.Now;
        }
        public string Id { get; set; }
        public string Name { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public int Percent { get; set; }

        public int MaxDisCount { get; set; }

        public int MaxTimeUse { get; set; }


    }
}
