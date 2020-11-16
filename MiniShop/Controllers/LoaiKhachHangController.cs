using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MiniShop.Exten;
using MiniShop.Repository.IRepository;

namespace MiniShop.Controllers
{
    public class LoaiKhachHangController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public LoaiKhachHangController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Upsert()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upsert(string ma_loai_khach_hang, string ten_loai_khach_hang)
        {
            if (ModelState.IsValid)
            {

                var parameter = new DynamicParameters();
                parameter.Add("@MALOAIKH", ma_loai_khach_hang);
                parameter.Add("@TENLOAI", ten_loai_khach_hang);
                var result = _unitOfWork.SP_Call.Excute(SD.Loai_Khach_Hang.CREATE, parameter);
                if (result.success)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if (result.message.Contains("duplicate"))
                        result.message = "Mã phân loại đã tồn tại";
                    ModelState.AddModelError("", result.message);
                }
            }
            return View();
        }
        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Loai_Khach_Hang.GET_ALL);
            if(result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
        public IActionResult Get(string id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MALOAIKH", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Loai_Khach_Hang.GET,parameter);
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Update(string a, string b, string c)
        {
        //        var a = $('#ma_loai_khach_hang').val()
                    //var b = $('#ma_loai_khach_hang_old').val()
                //var c = $('#ten_loai_khach_hang').val()
            var parameter = new DynamicParameters();
            parameter.Add("@MALOAIKHOLD", b);
            parameter.Add("@MALOAIKH", a);
            parameter.Add("@TENLOAI", c);
            var result = _unitOfWork.SP_Call.Excute(SD.Loai_Khach_Hang.UPDATE, parameter);
            if (result.success)
            {
                return Json(new { success = true, message = "Cập nhập thành công" });
            }
            else
                return Json(new { success = false, message = result.message });
        }
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MALOAIKH", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Loai_Khach_Hang.DELETE, parameter);
            if (result.success)
            {
                return Json(new { success = true, message = "xóa thành công" });
            }
            else
                return Json(new { success = false, message = result.message });
        }
    }
}
