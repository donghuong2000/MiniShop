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
    public class HomeController : BaseHomeController
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
            var result = _unitOfWork.SP_Call.Excute(SD.Thong_ke.DOANH_THU_10_NGAY_GAN_NHAT); // gọi procedure thống kê doanh thu 10 ngày gần nhất
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

        public IActionResult Update(string a, string b, string c, string d, string e, string f, string aa)
        {
            var parameter = new DynamicParameters(); // tạo parameter lưu thông tin
            parameter.Add("@MA_MH_OLD", aa);
            parameter.Add("@MA_MH_NEW", a);
            parameter.Add("@TEN_MH", b);
            parameter.Add("@NGAYSX", c);
            parameter.Add("@HANSD", d);
            parameter.Add("@LOAIMH", e);
            parameter.Add("@GIA", f);
            var result = _unitOfWork.SP_Call.Excute(SD.Mat_Hang.UPDATE, parameter); // gọi procedure cập nhật thông tin của mặt hàng
            if (result.success)
            {
                return Json(new { success = true, message = "Update thành công sản phẩm" });
            }
            return Json(new { success = false, message = result.message });
        }




    }
}
