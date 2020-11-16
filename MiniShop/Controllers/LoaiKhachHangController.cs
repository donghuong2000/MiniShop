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
    }
}
