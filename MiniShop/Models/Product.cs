using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Models
{
    public class Product
    {
        public Product(string productid,string productname,float productprice)
        {
            productId = productid;
            productName = productname;
            productPrice = productprice;
        }
        public string productId { get; set; }
        public string productName { get; set; }
        public float productPrice { get; set; }
    }
}
