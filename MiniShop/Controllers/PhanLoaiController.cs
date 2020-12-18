using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MiniShop.Exten;
using MiniShop.Repository.IRepository;
using Newtonsoft.Json;

namespace MiniShop.Controllers
{
    public class PhanLoaiController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public PhanLoaiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert()
        {
            return View();

        }

        //api-------------------------------------------------------------------------------------------------
        [HttpPost]
        public IActionResult Upsert(string ma_phan_loai,string ten_phan_loai)
        {  // đầu vào là name của các "Đối tượng html" trong view Upserts muốn truyền cho HttpPost để upload dữ liệu lên cho server
            if (ModelState.IsValid) // nếu các trường nhập vào cho view Upsert không bị sai quy tắc
            {
                
                 var parameter = new DynamicParameters(); // tạo 1 dynamic parameters để lưu các tham số truyền vào
                parameter.Add("@MA_PHAN_LOAI", ma_phan_loai);
                 parameter.Add("@TEN_MA_PHAN_LOAI", ten_phan_loai);
                 var result = _unitOfWork.SP_Call.Excute(SD.Phan_Loai.CREATE, parameter);
                // _unitOfWork.Sp_Call.Excute là hàm excute 1 hàm SQL, với đầu vào gồm 2 biến(biến thứ nhất kiểu string là tên stored procedure,
                //  biến thứ 2 kiểu Dynamic Parameters là tham số truyền vào cho stored procedure)
                if (result.success) // nếu hàm thực thi thành công 
                {
                    return RedirectToAction(nameof(Index)); // thì trả về View Index
                }    
                else
                {
                    if (result.message.Contains("duplicate")) // nếu message của result có chứa từ duplicate
                        result.message = "Mã phân loại đã tồn tại"; // gán result.message là mã phân loại đã tồn tại
                    ModelState.AddModelError("", result.message);  // Show model error có (key) bằng chuỗi rỗng
                    // (key) quyết định vị trí show model error ở ngay trên key đó
                    // Ở đây key bằng chuỗi rỗng tức là show lên đầu trang 
                    // Nội dung đưa vào cho model error là result.message
                }
            }    
            return View();
        }
        //http get
        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Phan_Loai.GET_ALL); // gọi procedure lấy danh sách phân loại
            if(result.success)
            {
                return Content(result.message, "application/json");
            }    
            return NotFound();
        }
        public IActionResult get(string id)
        {
            var parameter = new DynamicParameters(); // tạo parameter lưu thông tin 
            parameter.Add("@MA_PHAN_LOAI", id);
            var result =_unitOfWork.SP_Call.Excute(SD.Phan_Loai.GET, parameter); // gọi procedure lấy thông tin của phân loại
            if(result.success)
            {
                return Content(result.message, "application/json");
            }    
            else
            {
                return NotFound();
            }    
        }
        [HttpPost]
        public IActionResult Update(string oldId,string newId,string newValue)
        {
            var parameter = new DynamicParameters(); // tạo parameter lưu thông tin
            parameter.Add("@MA_PHAN_LOAI_OLD", oldId);
            parameter.Add("@MA_PHAN_LOAI_NEW", newId);
            parameter.Add("@TEN_MA_PHAN_LOAI", newValue);
            var result = _unitOfWork.SP_Call.Excute(SD.Phan_Loai.UPDATE,parameter); // gọi procedure update
            if(result.success)
            {
                return Json(new { success = true, message = "Cập nhập thành công" });
            }   
            else
                return Json(new { success = false, message = result.message });
        }
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var parameter = new DynamicParameters(); // parameter 
            parameter.Add("@MA_PL", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Phan_Loai.DELETE, parameter); // gọi stored procedures xóa phân loại
            if (result.success)
            {
                return Json(new { success = true, message = "xóa thành công" });
            }
            else
                return Json(new { success = false, message = result.message });


        }
    }
}
