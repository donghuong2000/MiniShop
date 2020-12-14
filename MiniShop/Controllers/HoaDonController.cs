using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniShop.Exten;
using MiniShop.Models;
using MiniShop.Repository;
using MiniShop.Repository.IRepository;
using Newtonsoft.Json.Linq;

namespace MiniShop.Controllers
{
    public class HoaDonController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;
        
        public HoaDonController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Hoa_Don.GET_ALL);
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }



        private IEnumerable<SelectListItem> GetSelectItemsNhanVien()
        {
            string objstring = _unitOfWork.SP_Call.Excute(SD.Nhan_Vien.GET_ALL).message;
            // chuẩn hóa cho phù hợp
            objstring = objstring.Substring(8, objstring.Length - 9);
            var obj = JArray.Parse(objstring);
            // chuyển đổi thành selectlist item

            var list = obj.Select(x => new SelectListItem(x["MANV"].ToString() + "-" + x["TENNV"].ToString(), x["MANV"].ToString()));
            return list;
        }



        private IEnumerable<SelectListItem> GetSelectItemsKhachHang()
        {
            string objstring = _unitOfWork.SP_Call.Excute(SD.Khach_Hang.GET_ALL).message;
            // chuẩn hóa cho phù hợp
            objstring = objstring.Substring(8, objstring.Length - 9);
            var obj = JArray.Parse(objstring);
            // chuyển đổi thành selectlist item

            var list = obj.Select(x => new SelectListItem(x["MAKH"].ToString() + "-" + x["TENKH"].ToString(), x["MAKH"].ToString()));
            return list;
        }
        private IEnumerable<SelectListItem> GetSelectItemsMaGiamGia()
        {
            string objstring = _unitOfWork.SP_Call.Excute(SD.Giam_Gia.GET_ALL).message;
            // chuẩn hóa cho phù hợp
            objstring = objstring.Substring(8, objstring.Length - 9);
            var obj = JArray.Parse(objstring);
            // chuyển đổi thành selectlist item
            var list = obj.Select(x => new SelectListItem(x["MAGG"].ToString(), x["MAGG"].ToString()));
            return list;
        }

        private IEnumerable<SelectListItem> GetSelectItemsMatHang()
        {
            string objstring = _unitOfWork.SP_Call.Excute(SD.Mat_Hang.GET_ALL).message;
            // chuẩn hóa cho phù hợp
            objstring = objstring.Substring(8, objstring.Length - 9);
            var obj = JArray.Parse(objstring);
            // chuyển đổi thành selectlist item

            var list = obj.Select(x => new SelectListItem(x["TENMH"].ToString(), x["MAMH"].ToString()));
            return list;
        }


        public IActionResult Get(string id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MAHD", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Hoa_Don.GET, parameter);
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
        
        public IActionResult GetPrice(string id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MAMH", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Mat_Hang.GET, parameter);
            float price = 0; 
            if (result.success && result.message.Length >20) 
            {
                // chuẩn hóa cho phù hợp
                var objstring = result.message;
                objstring = objstring.Substring(8, objstring.Length - 9);
                // chuyển lại thành Jarray
                var obj = JArray.Parse(objstring);
                // chuyển đổi thành string 
                price = float.Parse(obj[0]["GIA"].ToString());
            }
            return Json(new { data = price });
        }
        public IActionResult Upsert()
        {
            AddViewBagForUpsert();
            return View();
        }
        [HttpPost]
        public IActionResult Upsert(string ma_hoa_don, string khach_hang, string nhan_vien,  string ngay_lap_hoa_don,string total_amount,string discount ,string[] product, int[] qty)
        {
            var stt = Check_Quantity_Upper_Zero(qty);
            var list_check_quantity = CHECK_ENOUGH_QUANTITY_PRODUCT(product, qty);
            if (product.Length == 1 && product[0] == null)
            {
                ModelState.AddModelError("", "Hóa đơn không có mặt hàng nào cả, vui lòng thêm vào");
                AddViewBagForUpsert();
                return View();
            }
            else if (qty.Length==0)
            {
                ModelState.AddModelError("", "Số lượng của mặt hàng thứ 1 thuộc hóa đơn phải lớn hơn 0");
                AddViewBagForUpsert();
                return View();
            }
            else if (stt.Count>=1)
            {
                foreach(var item in stt)
                {
                    ModelState.AddModelError("", "Số lượng của mặt hàng thứ " + item + " thuộc hóa đơn phải lớn hơn 0");
                    ModelState.AddModelError("", "--------------------------------------------------------------------");
                }
                AddViewBagForUpsert();
                return View();
            }
            else if (list_check_quantity.Count >= 1)
            {
                foreach(var item in list_check_quantity)
                {
                    ModelState.AddModelError("", "Số lượng tồn kho của mặt hàng có mã " + item.productId + " chỉ còn " + item.productQuantity );
                    ModelState.AddModelError("", "Mặt hàng có mã " + item.productId + " chỉ được bán ra tối đa " + (item.productQuantity - 20) + " sản phẩm để không vi phạm quy định tồn kho");
                    ModelState.AddModelError("", "--------------------------------------------------------------------------------------------------");
                }
                ModelState.AddModelError("", " ! Quy định tồn kho : Số lượng tồn kho của mặt hàng sau khi thực hiện tạo hóa đơn phải từ 20 sản phẩm trở lên ");
                AddViewBagForUpsert();
                return View();
            }
            else
            { //{"data":[{"KETQUA":"0"}]}
                
                var parameter = new DynamicParameters();
                parameter.Add("@MAHD", ma_hoa_don);
                parameter.Add("@MAKH", khach_hang);
                parameter.Add("@MANV", nhan_vien);
                parameter.Add("@NGAYLHD", ngay_lap_hoa_don);
                parameter.Add("@TONGTIEN", total_amount);
                parameter.Add("@MAGIAMGIA", discount);
                var result_add_hoa_don = _unitOfWork.SP_Call.Excute(SD.Hoa_Don.CREATE, parameter);
                if (result_add_hoa_don.success == false)
                {
                    ModelState.AddModelError("", result_add_hoa_don.message);
                    AddViewBagForUpsert();
                    return View();
                }
                DynamicParameters parameter1;
                var listproduct_update = Get_List_Product_Standardized_Quantity(product, qty);
                for (int i = 0; i < listproduct_update.Count(); i++)
                {
                    parameter1 = new DynamicParameters();
                    parameter1.Add("@MAMH", listproduct_update[i].productId);
                    parameter1.Add("@MAHD", ma_hoa_don);
                    parameter1.Add("@SOLUONG", listproduct_update[i].productQuantity);
                    var result_add_hoa_don_detail = _unitOfWork.SP_Call.Excute(SD.Chi_Tiet_Hoa_Don.CREATE, parameter1);
                    if (result_add_hoa_don_detail.success == false)
                    {
                        ModelState.AddModelError("", result_add_hoa_don_detail.message);
                        AddViewBagForUpsert();
                        return View();
                    }
                }
            }
           
            
                return RedirectToAction("Index");

            
        }
        public void AddViewBagForUpsert()
        {
            ViewBag.MaGiamGia = GetSelectItemsMaGiamGia();
            ViewBag.HoaDonId = Guid.NewGuid();
            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.MatHang = GetSelectItemsMatHang();
            ViewBag.ListKhachHang = GetSelectItemsKhachHang();
            ViewBag.ListNhanVien = GetSelectItemsNhanVien();
        }
        public IActionResult GetDetail(string id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MAHD", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Hoa_Don.GETDETAIL,parameter);
            if(result.success && result.message.Length >20)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
        public IActionResult Detail(string id)
        {
            ViewBag.HoaDonId = id;
            return View();
        }
        public List<Product> Get_List_Product_Standardized_Quantity (string[] product , int[] quantity)
        {
            List<Product> listproducts = new List<Product>() ;
            for(int i=0; i < product.Length; i++)
            {
                Product p = new Product(product[i],quantity[i]);
                listproducts.Add(p);
            } // tao ra 1 list product tu 2 mang product va quantity
            var newProductList = listproducts.GroupBy(x => x.productId)
                .Select(x => new
                {
                    productid = x.Key,
                    productQuantity = x.Sum(y => y.productQuantity)
                }).ToList();
            
            var listproduct_update = newProductList.Select(x => new Product { productId = x.productid, productQuantity = x.productQuantity }).ToList();
            return listproduct_update;
        }
        public List<string> Check_Quantity_Upper_Zero(int [] quantity)
        {
            List<string> STT= new List<string>();
            for(int i=0;i<quantity.Length;i++)
            {
                if(quantity[i] == 0)
                {
                    STT.Add((i+1).ToString());
                }
            }
            return STT;
        }
        public List<Product> CHECK_ENOUGH_QUANTITY_PRODUCT(string[] product ,int[] quantity)
        {
            List<Product> listproducts = new List<Product>();
            for (int i = 0; i < product.Length; i++)
            {
                var parameter_check = new DynamicParameters();
                parameter_check.Add("@MAMH", product[i]);
                parameter_check.Add("@SOLUONG", quantity[i]);
                var result_check = _unitOfWork.SP_Call.Excute(SD.Hoa_Don.CHECK_ENOUGH_QUANTITY_PRODUCT, parameter_check);
                if (result_check.success)
                {
                    var objstring = result_check.message;
                    objstring = objstring.Substring(8, objstring.Length - 9);
                    var obj = JArray.Parse(objstring);
                    var result = int.Parse(obj[0]["KETQUA"].ToString());
                    var quantity_remain = int.Parse(obj[0]["SOLUONGTON"].ToString());
                    if (result == 0)
                    {
                        Product product_temp = new Product() { productId = product[i], productQuantity = quantity_remain };
                        listproducts.Add(product_temp); // khong the tao duoc hoa don , vi co 1 mat hang vi pham so luong trong kho
                        // list này hiển thị mã mặt hàng nào có số lượng tồn không đủ trong kho , kèm quantity là số lượng tồn trong kho
                    }    
                        
                            
                }
            }
            return listproducts;
        }
    }

}
