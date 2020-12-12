using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Models
{
    public class HoaDonViewModel
    {
        public HoaDonViewModel()
        {
            listProduct = new List<Product>();
        }
        public string Id { get; set; } // mã hóa đơn
        public string CustomerId { get; set; } // mã khách hàng
        public string CustomerName { get; set; } // tên khách hàng
        public string StaffId { get; set; } // mã nhân viên
        public string StaffName { get; set; } // tên nhân viên
        public DateTime DateCreate { get; set; } // ngày lập hóa đơn
        public float TotalAmount  { get; set; } // tổng tiền
        public string Discount { get; set; }  // mã giảm giá của hóa đơn
        public float SubAmount { get; set; } // tổng tiền tạm tính
        public List<Product> listProduct { get; set; }
    }
}
