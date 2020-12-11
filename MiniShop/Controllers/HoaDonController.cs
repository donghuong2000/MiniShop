﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniShop.Exten;
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

        public IActionResult Upsert()
        {
            ViewBag.MaGiamGia = GetSelectItemsMaGiamGia();
            ViewBag.HoaDonId = Guid.NewGuid();
            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
            ViewBag.MatHang = GetSelectItemsMatHang();
            ViewBag.ListKhachHang = GetSelectItemsKhachHang();
            ViewBag.ListNhanVien = GetSelectItemsNhanVien();
            return View();
        }
        //public IActionResult Get(string id)
        //{
        //    var parameter = new DynamicParameters();
        //    parameter.Add("@MAHD", id);
        //    var result = _unitOfWork.SP_Call.Excute(SD.Hoa_Don.GET, parameter);
        //    if (result.success)
        //    {
        //        return Content(result.message, "application/json");
        //    }
        //    return NotFound();
        //}
        //public IActionResult Detail(string id)
        //{
        //    return View();
        //}
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
                var obj = JArray.Parse(objstring);
                // chuyển đổi thành 
                price = float.Parse(obj[0]["GIA"].ToString());
            }
            return Json(new { data = price });
        }
        [HttpPost]
        public IActionResult Upsert(string ma_hoa_don,string nhan_vien,string khach_hang, string discount , string total_amount,string ngay_lap_hoa_don,string[] product, string[] qty)
        {
            return View();
        }
    }

}
