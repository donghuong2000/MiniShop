using System;
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
    public class GiamGiaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public GiamGiaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Giam_Gia.GET_ALL);
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
            if(objstring.Length>0)
            {
                objstring = objstring.Substring(8, objstring.Length - 9);
                var obj = JArray.Parse(objstring);
                // chuyển đổi thành selectlist item
                var list = obj.Select(x => new SelectListItem(x["TENLOAIMH"].ToString(), x["MALOAIMH"].ToString()));
                return list;
            }
            return null;
            
            
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
        public IActionResult Upsert(string id)
        {
            ViewBag.PhanLoaiList = GetSelectItemsPhanLoai();
            ViewBag.SanPhamList = GetSelectItemsMatHang();
            var vm = new MagiamgiaViewModel();
            ViewBag.Id = "";
            if (id!=null)
            {
                ViewBag.Id = "has";
                var parameter = new DynamicParameters();
                parameter.Add("@MAGG", id);
                var result = _unitOfWork.SP_Call.Excute(SD.Giam_Gia.GET, parameter);
                if(result.success)
                {
                    var objString = result.message.Substring(8, result.message.Length - 9);
                    var obj = JArray.Parse(objString);
                    vm.Id = id;
                    vm.Name = obj[0]["TEN"].ToString();
                    vm.DateStart = DateTime.Parse(obj[0]["NGAYBD"].ToString());
                    vm.DateEnd = DateTime.Parse(obj[0]["NGAYKT"].ToString());
                    vm.Percent = int.Parse(obj[0]["PT_GIAM"].ToString());
                    vm.MaxDisCount = int.Parse(obj[0]["TIENGIAM"].ToString());
                    vm.MaxTimeUse = int.Parse(obj[0]["LANSUDUNGMAX"].ToString());
                   
                }    
            }
            return View(vm);
        }
        [HttpPost]
        public IActionResult Upsert(MagiamgiaViewModel vm)
        {
            
            if(ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@MAGG", vm.Id);
                var result = _unitOfWork.SP_Call.Excute(SD.Giam_Gia.IS_EXIST, parameter).message;
                result = result.Substring(8, result.Length - 9);
                var obj = JArray.Parse(result);
                var isExist = obj[0]["IS_EXIST"].ToString();
                parameter.Add("@TEN", vm.Name);
                parameter.Add("@NGAYBD", vm.DateStart);
                parameter.Add("@NGAYKT", vm.DateEnd);
                parameter.Add("@PT_GIAM", vm.Percent);
                parameter.Add("@TIENGIAM", vm.MaxDisCount);
                parameter.Add("@LANSUDUNGMAX", vm.MaxTimeUse);
                if (isExist != "1")
                {
                    //add 
                    
                    var res = _unitOfWork.SP_Call.Excute(SD.Giam_Gia.CREATE, parameter);
                    if(res.success)
                    {
                       
                        return RedirectToAction("index");
                    }
                    ModelState.AddModelError("", res.message);

                }
                else
                {
                    //add update
                    var res = _unitOfWork.SP_Call.Excute(SD.Giam_Gia.UPDATE, parameter);
                    if (res.success)
                    {
                        return RedirectToAction("index");
                    }
                    ModelState.AddModelError("", res.message);

                }

            }
            
            return View(vm);

        }
        public IActionResult Get(string id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MAGG", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Giam_Gia.GET, parameter);
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MAGG", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Giam_Gia.DELETE, parameter);
            if(result.success)
            {
                return Json(new { success = true, message = "Xóa thành công Mã Giảm Giá" });
            }
            return Json(new { success = false, message = result.message });
        }
        
    }
}
