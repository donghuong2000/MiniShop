using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniShop.Exten;
using MiniShop.Repository.IRepository;
using Newtonsoft.Json.Linq;

namespace MiniShop.Controllers
{
    public class MatHangController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public MatHangController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            ViewBag.PhanLoaiList = GetSelectItemsPhanLoai();
            return View();
        }
        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Mat_Hang.GET_ALL);
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
        private IEnumerable<SelectListItem> GetSelectItemsPhanLoai()
        {
            string objstring = _unitOfWork.SP_Call.Excute(SD.Phan_Loai.GET_ALL).message;
            // chuẩn hóa cho phù hợp
            objstring = objstring.Substring(8, objstring.Length - 9);
            var obj = JArray.Parse(objstring);
            // chuyển đổi thành selectlist item
            var list = obj.Select(x => new SelectListItem(x["TENLOAIMH"].ToString(), x["MALOAIMH"].ToString()));
            return list;
        }
        public IActionResult Get(string id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MAMH", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Mat_Hang.GET,parameter);
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
        
        public IActionResult Create()
        {
            ViewBag.PhanLoaiList = GetSelectItemsPhanLoai();
            return View();
        }
        [HttpPost]
        public IActionResult Create(string a,string b, string c, string d, string e, string f)
        {
            ViewBag.PhanLoaiList = GetSelectItemsPhanLoai();
            var parameter = new DynamicParameters();
            parameter.Add("@MAMH", a);
            parameter.Add("@TENMH", b);
            parameter.Add("@NGAYSX", c);
            parameter.Add("@HANSD", d);
            parameter.Add("@LOAIMH", e);
            parameter.Add("@GIA", f);
            var result = _unitOfWork.SP_Call.Excute(SD.Mat_Hang.CREATE, parameter);
            if (result.success)
            {
                return RedirectToAction("index");
            }
            ModelState.AddModelError("", result.message);
            return View();
        }
        public IActionResult Update()
        {
            return View();
        }


        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MA_MH", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Mat_Hang.DELETE,parameter);
            if (result.success)
            {
                return Json(new { success = true, message = "Xóa thành công sản phẩm" });
            }
            return Json(new { success = false, message = result.message });
        }
    }
}
