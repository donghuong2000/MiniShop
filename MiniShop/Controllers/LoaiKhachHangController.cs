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
    public class LoaiKhachHangController : BaseHomeController
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
        [HttpGet]
        public IActionResult Upsert()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upsert(string ma_loai_khach_hang, string ten_loai_khach_hang)
        { // đầu vào là name của các "Đối tượng html" trong view Upserts muốn truyền cho HttpPost để upload dữ liệu lên cho server
            if (ModelState.IsValid) // nếu các trường nhập vào cho view Upsert không bị sai quy tắc
            {

                var parameter = new DynamicParameters();  // tạo 1 dynamic parameters để lưu các tham số truyền vào
                parameter.Add("@MALOAIKH", ma_loai_khach_hang);
                parameter.Add("@TENLOAI", ten_loai_khach_hang);
                var result = _unitOfWork.SP_Call.Excute(SD.Loai_Khach_Hang.CREATE, parameter);
                // _unitOfWork.Sp_Call.Excute là hàm excute 1 hàm SQL, với đầu vào gồm 2 biến(biến thứ nhất kiểu string là tên stored procedure,
                //  biến thứ 2 kiểu Dynamic Parameters là tham số truyền vào cho stored procedure)
                if (result.success) // nếu hàm thực thi thành công 
                {
                    return RedirectToAction(nameof(Index)); // thì trả về View Index
                }
                else
                {
                    if (result.message.Contains("duplicate")) // nếu message của result có chứa từ duplicate
                        result.message = "Mã loại khách hàng đã tồn tại"; // gán result message bằng chuỗi
                    ModelState.AddModelError("", result.message);
                    // Show model error có (key) bằng chuỗi rỗng
                    // (key) quyết định vị trí show model error ở ngay trên key đó
                    // Ở đây key bằng chuỗi rỗng tức là show lên đầu trang 
                    // Nội dung đưa vào cho model error là result.message
                }
            }
            return View();
        }
        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Loai_Khach_Hang.GET_ALL); // gọi procedure lấy danh sách loại khách hàng
            if(result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
        public IActionResult Get(string id)
        {
            var parameter = new DynamicParameters(); // tạo parameter lưu thông tin 
            parameter.Add("@MALOAIKH", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Loai_Khach_Hang.GET,parameter); // gọi procedure lấy thông tin của loại khách hàng
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
        //        var a = $('#ma_loai_khach_hang').val()
        //var b = $('#ma_loai_khach_hang_old').val()
        //var c = $('#ten_loai_khach_hang').val()
        [HttpPost]
        public IActionResult Update(string a, string b, string c)
        {
 
            var parameter = new DynamicParameters(); 
            parameter.Add("@MALOAIKHOLD", b);
            parameter.Add("@MALOAIKH", a);
            parameter.Add("@TENLOAI", c);
            var result = _unitOfWork.SP_Call.Excute(SD.Loai_Khach_Hang.UPDATE, parameter);
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
            var parameter = new DynamicParameters(); // tạo parameter lưu thông tin
            parameter.Add("@MALOAIKH", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Loai_Khach_Hang.DELETE, parameter); // gọi stored procedure xóa loại khách hàng
            if (result.success)
            {
                return Json(new { success = true, message = "xóa thành công" });
            }
            else
                return Json(new { success = false, message = result.message });
        }
    }
}
