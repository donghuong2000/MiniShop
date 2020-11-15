using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniShop.Exten;
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
            objstring = objstring.Substring(8, objstring.Length - 9);
            var obj = JArray.Parse(objstring);
            // chuyển đổi thành selectlist item
            var list = obj.Select(x => new SelectListItem(x["TENLOAIMH"].ToString(), x["MALOAIMH"].ToString()));
            return list;
        }
        public IActionResult Upsert()
        {
            ViewBag.PhanLoaiList = GetSelectItemsPhanLoai();
            return View();
        }
    }
}
