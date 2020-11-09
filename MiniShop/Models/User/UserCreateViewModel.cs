using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniShop.Models.User
{
    public class UserCreateViewModel
    {
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ComfirmPassword { get; set; }
        public string NgaySinh { get; set; }

        public string GioiTinh { get; set; }
        public string CMND { get; set; }
        public string SDT { get; set; }

        public string ChucVu { get; set; }
        public string NgayLamViec { get; set; }
        public string DiaChi { get; set; }
        
        
    }
}
