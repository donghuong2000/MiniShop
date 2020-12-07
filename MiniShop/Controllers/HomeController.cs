using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniShop.Exten;
using MiniShop.Repository.IRepository;

namespace MiniShop.Controllers
{
    public class HomeController : BaseController
    {

        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult getAll()
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MA_PHAN_LOAI","DGD");
            parameter.Add("@TEN_MA_PHAN_LOAI", "Đồ Gia Dụng");
             
            var a =  _unitOfWork.SP_Call.Excute(SD.Phan_Loai.CREATE,parameter);
            return Json(a);
        }


        
        
    }
}
