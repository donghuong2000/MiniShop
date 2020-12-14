using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Models
{
    public class Product
    {
        
        public string productId { get; set; }
        public int productQuantity { get; set; }
        public Product()
        {

        }
        public Product(string productid, int productquantity)
        {
            productId = productid;
            productQuantity = productquantity;
        }
    }
}
