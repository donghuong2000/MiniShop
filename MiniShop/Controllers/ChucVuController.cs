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
    public class ChucVuController : BaseController
    {

        private readonly IUnitOfWork _unitOfWork;
        public ChucVuController(IUnitOfWork unitOfWork)
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
        public IActionResult Upsert(string ma_chuc_vu,string ten_chuc_vu,int luong)
        { // đầu vào là name của các "Đối tượng html" trong view Upserts muốn truyền cho HttpPost để upload dữ liệu lên cho server
            var parameter = new DynamicParameters(); // tạo 1 dynamic parameters để lưu các tham số truyền vào
            parameter.Add("@MA_CHUC_VU", ma_chuc_vu);
            parameter.Add("@TEN_CHUC_VU", ten_chuc_vu);
            parameter.Add("@LUONG", luong);
            var result = _unitOfWork.SP_Call.Excute(SD.Chuc_vu.CREATE, parameter);
            // _unitOfWork.Sp_Call.Excute là hàm excute 1 hàm SQL, với đầu vào gồm 2 biến(biến thứ nhất kiểu string là tên stored procedure,
            //  biến thứ 2 kiểu Dynamic Parameters là tham số truyền vào cho stored procedure)
            if (result.success) // nếu hàm thực thi thành công 
            {
                return RedirectToAction(nameof(Index)); // thì trả về View Index
            }
            else
            {
                if (result.message.Contains("duplicate")) // nếu message của result có chứa từ duplicate
                    result.message = "Chức vụ đã tồn tại";  // gán result.message là tên tài khoản bị trùng
                ModelState.AddModelError("", result.message);  // Show model error có (key) bằng chuỗi rỗng
                                                               // (key) quyết định vị trí show model error ở ngay trên key đó
                                                               // Ở đây key bằng chuỗi rỗng tức là show lên đầu trang 
                                                               // Nội dung đưa vào cho model error là result.message
            }
            return View();
        }

        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Chuc_vu.GET_ALL); // gọi procedure lấy danh sách chức vụ
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            else
                return NotFound();

        }
        public IActionResult Get(string  id)
        {
            var parameter = new DynamicParameters(); // tạo parameter lưu thông tin
            parameter.Add("@MA_CHUC_VU", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Chuc_vu.GET,parameter); // gọi procedure lấy thông tin của chức vụ
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            else
                return NotFound("ko thấy chức vụ này");

        }
        [HttpPost]
        public IActionResult Update(string oldId, string newId, string newValue,int luong)
        {
            var parameter = new DynamicParameters(); // tạo parameter lưu thông tin 
            parameter.Add("@MA_CHUC_VU_OLD", oldId);
            parameter.Add("@MA_CHUC_VU_NEW", newId);
            parameter.Add("@TEN_CHUC_VU", newValue);
            parameter.Add("@LUONG", luong);
            var result = _unitOfWork.SP_Call.Excute(SD.Chuc_vu.UPDATE, parameter); // gọi procedure update chức vụ
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
            parameter.Add("@MA_CHUC_VU", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Chuc_vu.DELETE, parameter); // gọi stored procedure xóa chức vụ
            if (result.success)
            {
                return Json(new { success = true, message = "xóa thành công" });
            }
            else
                return Json(new { success = false, message = result.message });


        }
    }
}
