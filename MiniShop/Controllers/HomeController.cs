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
            ViewBag.numNV = SLNV();
            ViewBag.numKH = SLKH();
            ViewBag.numHD = SLHD();
            ViewBag.tongHD =tongHD();

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
                var values = obj.Select(x => float.Parse(x["TONG"].ToString())).ToArray();
                // 
                return Json(new { labels, values });
            }
            return NotFound();
        }
       
        public IActionResult DT10MONTH()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Thong_ke.DOANH_THU_10_THANG_GAN_NHAT); // gọi procedure thống kê doanh thu 10 tháng gần nhất
            if (result.success && result.message.Length > 20) // loại bỏ trường hợp thực thi thất bại , hoặc thực thi thành công nhưng ra json rỗng
            {
                var objstring = result.message;
                objstring = objstring.Substring(8, objstring.Length - 9); // chuẩn hóa đầu ra
                var obj = JArray.Parse(objstring); // chuyển về dạng Jarray(mảng Json)
                var labels = obj.Select(x =>"Tháng "+x["MONTH"].ToString()).ToArray(); // gán mảng tên tháng của obj cho labels để hiển thị lên giao diện
                var values = obj.Select(x => float.Parse(x["TONG"].ToString())).ToArray(); // gán mảng doanh thu của obj cho values để hiển thị lên giao diện
                // 
                return Json(new { labels, values });
            }
            return NotFound();
        }
        public IActionResult DT10KHMNN()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Thong_ke.THONG_KE_10_KHACH_HANG_MUA_NHIEU_NHAT); // gọi procedure thống kê 10 khách hàng mua nhiều nhất
            if (result.success && result.message.Length > 20) // loại bỏ trường hợp thực thi thất bại , hoặc thực thi thành công nhưng ra json rỗng
            {
                var objstring = result.message; 
                objstring = objstring.Substring(8, objstring.Length - 9); // chuẩn hóa đầu ra
                var obj = JArray.Parse(objstring); // chuyển về dạng Jarray(mảng Json)
                var labels = obj.Select(x => x["TENKH"].ToString()).ToArray(); // gán mảng tên khách hàng của obj cho labels để hiển thị lên giao diện
                var values = obj.Select(x => float.Parse(x["TONG"].ToString())).ToArray(); // gán mảng "tổng tiền của khách hàng" cho values để hiển thị lên giao diện
                return Json(new { labels, values });
            }
            return NotFound();
        }
        public IActionResult TK10NVCDTCN()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Thong_ke.THONG_KE_10_NHAN_VIEN_DOANH_THU_CAO_NHAT);// gọi procedure thống kê 10 nhân viên doanh thu cao nhất
            if (result.success && result.message.Length > 20) // loại bỏ trường hợp thực thi thất bại , hoặc thực thi thành công nhưng ra json rỗng
            {
                var objstring = result.message;
                objstring = objstring.Substring(8, objstring.Length - 9); // chuẩn hóa đầu ra
                var obj = JArray.Parse(objstring);  // chuyển về dạng Jarray(mảng Json)
                var labels = obj.Select(x => x["TENNV"].ToString()).ToArray(); // gán mảng tên nhân viên cho labels để hiển thị lên giao diện
                var values = obj.Select(x => float.Parse(x["SOLUONGDONG"].ToString())).ToArray();  // gán mảng số lượng đơn cho values để hiển thị lên giao diện
                var values1 = obj.Select(x => float.Parse(x["TONGTIEN"].ToString())).ToArray();  // gán mảng "tổng tiền bán được" của nhân viên cho values1 để hiển thị lên giao diện
                return Json(new { labels, values,values1 });
            }
            return NotFound();
        }
        public IActionResult TK10SPMNN()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Thong_ke.THONG_KE_10_SAN_PHAM_MUA_NHIEU_NHAT);// gọi procedure thống kê 10 nhân viên doanh thu cao nhất
            if (result.success && result.message.Length > 20)// loại bỏ trường hợp thực thi thất bại , hoặc thực thi thành công nhưng ra json rỗng
            {
                var objstring = result.message;
                objstring = objstring.Substring(8, objstring.Length - 9);// chuẩn hóa đầu ra
                var obj = JArray.Parse(objstring);// chuyển về dạng Jarray(mảng Json)
                var labels = obj.Select(x => x["TENMH"].ToString()).ToArray();// gán mảng tên mặt hàng cho labels để hiển thị lên giao diện
                var values = obj.Select(x => float.Parse(x["SL"].ToString())).ToArray();// gán mảng "số lượng bán được" của mỗi sản phẩm cho values để hiển thị lên giao diện
                return Json(new { labels, values });
            }
            return NotFound();
        }

        public int SLNV()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Thong_ke.COUNT_NHAN_VIEN); // gọi procedure thống kê số lượng nhân viên
            if (result.success)
            {
                var objstring = result.message;
                objstring = objstring.Substring(8, objstring.Length - 9); // chuẩn hóa đầu ra
                return int.Parse(objstring); // trả kết quả về dưới dạng int    
            }
            return 0;
        }
        public int SLKH()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Thong_ke.COUNT_KHACH_HANG);// gọi procedure thống kê số lượng khách hàng
            if (result.success)
            {
                var objstring = result.message;
                objstring = objstring.Substring(8, objstring.Length - 9);// chuẩn hóa đầu ra
                return int.Parse(objstring); // trả kết quả về dưới dạng int
            }
            return 0;
        }
        public int SLHD()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Thong_ke.COUNT_HOA_DON);// gọi procedure thống kê số lượng hóa đơn
            if (result.success)
            {
                var objstring = result.message;
                objstring = objstring.Substring(8, objstring.Length - 9);// chuẩn hóa đầu ra
                return int.Parse(objstring); // trả kết quả về dưới dạng int
            }
            return 0;
        }
        public double tongHD()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Thong_ke.SUM_HOA_DON); // gọi procedure thống kê tổng doanh thu
            if (result.success)
            {
                var objstring = result.message;
                objstring = objstring.Substring(8, objstring.Length - 9);// chuẩn hóa đầu ra
                return double.Parse(objstring); // trả kết quả về dưới dạng double
            }
            return 0;
        }
    }
}
