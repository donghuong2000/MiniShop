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
    public class NhanVienController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public NhanVienController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }
        public IActionResult Index()
        {
            // get json string

            ViewBag.ListChucVu = GetSelectItemsChucVu();
            return View();
        }
        private IEnumerable<SelectListItem> GetSelectItemsChucVu()
        {
            string objstring = _unitOfWork.SP_Call.Excute(SD.Chuc_vu.GET_ALL).message;
            // chuẩn hóa cho phù hợp
            objstring = objstring.Substring(8, objstring.Length - 9);
            var obj = JArray.Parse(objstring);
            // chuyển đổi thành selectlist item
            var list = obj.Select(x => new SelectListItem(x["TENCHUCVU"].ToString(), x["MACHUCVU"].ToString()));
            return list;
        }
        public IActionResult Upsert()
        {
            ViewBag.ListChucVu = GetSelectItemsChucVu();
            return View();
        }
        [HttpPost]
        public IActionResult Upsert(string id)
        {

            return View();
        }
        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Nhan_Vien.GET_ALL);
            if(result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
        public IActionResult Get(string id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MANV", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Nhan_Vien.GET,parameter);
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
    }
}
