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
    public class ChucVuController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public ChucVuController(IUnitOfWork unitOfWork)
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
        public IActionResult Upsert(string ma_chuc_vu,string ten_chuc_vu,int luong)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MA_CHUC_VU", ma_chuc_vu);
            parameter.Add("@TEN_CHUC_VU", ten_chuc_vu);
            parameter.Add("@LUONG", luong);
            var result = _unitOfWork.SP_Call.Excute(SD.Chuc_vu.CREATE, parameter);
            if (result.success)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (result.message.Contains("duplicate"))
                    result.message = "Chức vụ đã tồn tại";
                ModelState.AddModelError("", result.message);
            }
            return View();
        }

        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Chuc_vu.GET_ALL);
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            else
                return NotFound();

        }
        public IActionResult Get(string  id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MA_CHUC_VU", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Chuc_vu.GET,parameter);
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            else
                return NotFound("ko thấy chức vụ này");

        }
        [HttpPost]
        public IActionResult Update(string oldId, string newId, string newValue,int luong)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MA_CHUC_VU_OLD", oldId);
            parameter.Add("@MA_CHUC_VU_NEW", newId);
            parameter.Add("@TEN_CHUC_VU", newValue);
            parameter.Add("@LUONG", luong);
            var result = _unitOfWork.SP_Call.Excute(SD.Chuc_vu.UPDATE, parameter);
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
            parameter.Add("@MA_CHUC_VU", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Chuc_vu.DELETE, parameter);
            if (result.success)
            {
                return Json(new { success = true, message = "xóa thành công" });
            }
            else
                return Json(new { success = false, message = result.message });


        }
    }
}
