
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniShop.Exten;
using MiniShop.Models;
using MiniShop.Repository.IRepository;
using Newtonsoft.Json.Linq;


namespace MiniShop.Controllers
{
    public class DonNhapHangController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public DonNhapHangController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Don_Nhap_Hang.GET_ALL);
            if (result.success)
            {
                return Content(result.message, "application/json");
            }

            return NotFound();
        }



        private IEnumerable<SelectListItem> GetSelectItemsNhaCungCap()
        {
            string objstring = _unitOfWork.SP_Call.Excute(SD.Nha_Cung_Cap.GET_ALL).message;
            // chuẩn hóa cho phù hợp
            objstring = objstring.Substring(8, objstring.Length - 9);
            var obj = JArray.Parse(objstring);
            // chuyển đổi thành selectlist item

            var list = obj.Select(x => new SelectListItem(x["TENNCC"].ToString(), x["MANCC"].ToString()));
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
        public IActionResult Upsert()
        {
            AddViewBagForUpsert();
            return View();
        }
        [HttpPost]
        public IActionResult Upsert(string ma_don_nhap_hang, string nha_cung_cap, string ngay_nhap,string[] product, int[] qty)
        {
            var stt = Check_Quantity_Upper_Zero(qty);
            if (product.Length == 1 && product[0] == null)
            {
                ModelState.AddModelError("", "Đơn nhập hàng không có mặt hàng nào cả, vui lòng thêm vào");
                AddViewBagForUpsert();
                return View();
            }
            else if (qty.Length == 0)
            {
                ModelState.AddModelError("", "Đơn Nhập Hàng của mặt hàng thứ 1 thuộc hóa đơn phải lớn hơn 0");
                AddViewBagForUpsert();
                return View();
            }
            else if (stt.Count >= 1)
            {
                foreach (var item in stt)
                {
                    ModelState.AddModelError("", "Số lượng của mặt hàng thứ " + item + " thuộc hóa đơn phải lớn hơn 0");
                }
                AddViewBagForUpsert();
                return View();
            }
            else
            {
                var parameter = new DynamicParameters();
                parameter.Add("@MA_DNH", ma_don_nhap_hang);
                parameter.Add("@MA_NCC", nha_cung_cap);
                parameter.Add("@NGAYNHAP", ngay_nhap);
                var result_add_don_nhap_hang = _unitOfWork.SP_Call.Excute(SD.Don_Nhap_Hang.CREATE, parameter);
                if (result_add_don_nhap_hang.success == false)
                {
                    ModelState.AddModelError("", result_add_don_nhap_hang.message);
                    AddViewBagForUpsert();
                    return View();
                }
                DynamicParameters parameter1;
                var listproduct_update = Get_List_Product_Standardized_Quantity(product, qty);
                for (int i = 0; i < listproduct_update.Count(); i++)
                {
                    parameter1 = new DynamicParameters();
                    parameter1.Add("@MA_MH", listproduct_update[i].productId);
                    parameter1.Add("@MA_DNH", ma_don_nhap_hang);
                    parameter1.Add("@SOLUONG", listproduct_update[i].productQuantity);
                    var result_add_don_nhap_hang_detail = _unitOfWork.SP_Call.Excute(SD.Chi_Tiet_Don_Nhap_Hang.CREATE, parameter1);
                    if (result_add_don_nhap_hang_detail.success == false)
                    {
                        ModelState.AddModelError("", result_add_don_nhap_hang_detail.message);
                        AddViewBagForUpsert();
                        return View();
                    }
                }
            }


            return RedirectToAction("Index");


        }
        public void AddViewBagForUpsert()
        {
            ViewBag.ListNhaCungCap = GetSelectItemsNhaCungCap();
            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.MatHang = GetSelectItemsMatHang();
        }
        public IActionResult GetDetail(string id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MADNH", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Don_Nhap_Hang.GETDETAIL, parameter);
            if (result.success && result.message.Length > 20)   
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
        public IActionResult Detail(string id)
        {
            ViewBag.DonNhapHangId = id;
            return View();
        }
        public List<Product> Get_List_Product_Standardized_Quantity(string[] product, int[] quantity)
        {
            List<Product> listproducts = new List<Product>();
            for (int i = 0; i < product.Length; i++)
            {
                Product p = new Product(product[i], quantity[i]);
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
        public List<string> Check_Quantity_Upper_Zero(int[] quantity)
        {
            List<string> STT = new List<string>();
            for (int i = 0; i < quantity.Length; i++)
            {
                if (quantity[i] == 0)
                {
                    STT.Add((i + 1).ToString());
                }
            }
            return STT;
        }
    }
}
