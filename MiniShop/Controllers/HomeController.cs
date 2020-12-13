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
using Newtonsoft.Json.Linq;

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
        public IActionResult thongkedoanhthu(int id)
        {

            var parameter = new DynamicParameters();
            parameter.Add("@THANG", id);
            var result =  _unitOfWork.SP_Call.Excute(SD.Thong_ke.DOANH_THU_THANG, parameter);
            if(result.success && result.message.Length>20)
            {
                var objstring = result.message;
                objstring = objstring.Substring(8, objstring.Length - 9);
                var obj = JArray.Parse(objstring);
                var labels = obj.Select(x => x["NGAYLHD"].ToString()).ToArray();
                var values= obj.Select(x => int.Parse(x["TONG"].ToString())).ToArray();
                // 
                return Json(new { labels, values });
            }
            return NotFound();
        }
        public IActionResult DT10DAY()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Thong_ke.DOANH_THU_10_NGAY_GAN_NHAT);
            if (result.success && result.message.Length > 20)
            {
                var objstring = result.message;
                objstring = objstring.Substring(8, objstring.Length - 9);
                var obj = JArray.Parse(objstring);
                var labels = obj.Select(x => x["IndividualDate"].ToString().Replace(" 12:00:00 AM","")).ToArray();
                var values = obj.Select(x => int.Parse(x["TONG"].ToString())).ToArray();
                // 
                return Json(new { labels, values });
            }
            return NotFound();
        }




    }
}
