using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MiniShop.Exten;
using MiniShop.Repository.IRepository;
using Newtonsoft.Json;

namespace MiniShop.Controllers
{
    public class PhanLoaiController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PhanLoaiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert()
        {
            return View();

        }

        //api-------------------------------------------------------------------------------------------------
        [HttpPost]
        public IActionResult Upsert(string ma_phan_loai,string ten_phan_loai)  
        {
            if(ModelState.IsValid)
            {
                
                 var parameter = new DynamicParameters();
                 parameter.Add("@MA_PHAN_LOAI", ma_phan_loai);
                 parameter.Add("@TEN_MA_PHAN_LOAI", ten_phan_loai);
                 var result = _unitOfWork.SP_Call.Excute(SD.Phan_Loai.CREATE, parameter);
                if(result.success)
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
        //http get
        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Phan_Loai.GET_ALL);
            if(result.success)
            {
                return Content(result.message, "application/json");
            }    
            return NotFound();
        }
        public IActionResult get(string id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MA_PHAN_LOAI", id);
            var result =_unitOfWork.SP_Call.Excute(SD.Phan_Loai.GET, parameter);
            if(result.success)
            {
                return Content(result.message, "application/json");
            }    
            else
            {
                return NotFound();
            }    
        }
        [HttpPost]
        public IActionResult Update(string oldId,string newId,string newValue)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MA_PHAN_LOAI_OLD", oldId);
            parameter.Add("@MA_PHAN_LOAI_NEW", newId);
            parameter.Add("@TEN_MA_PHAN_LOAI", newValue);
            var result = _unitOfWork.SP_Call.Excute(SD.Phan_Loai.UPDATE,parameter);
            if(result.success)
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
            parameter.Add("@MA_PHAN_LOAI", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Phan_Loai.DELETE, parameter);
            if (result.success)
            {
                return Json(new { success = true, message = "xóa thành công" });
            }
            else
                return Json(new { success = false, message = result.message });


        }
    }
}
