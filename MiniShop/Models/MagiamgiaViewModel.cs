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
            ProductId = new List<string>();
            CategoryId = new List<string>();
        }
        public string Id { get; set; }
        public string Name { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public int Percent { get; set; }

        public int MaxDisCount { get; set; }

        public int MaxTimeUse { get; set; }

        public bool IsProductDisCount { get; set; }

        public List<string> ProductId { get; set; }

        public bool IsCategoryDisCount { get; set; }
        public List<string> CategoryId { get; set; }

        public bool IsAllDisCount { get; set; }
    }
}
