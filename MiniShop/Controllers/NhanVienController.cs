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
    public class NhanVienController : BaseController
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
            string objstring = _unitOfWork.SP_Call.Excute(SD.Chuc_vu.GET_ALL).message; // gọi hàm
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
        public IActionResult Upsert(string ma_nhan_vien,string ten_nhan_vien,
            string ngay_sinh,string gioi_tinh,
            string cmnd,string sdt,
            string ngay_lam_viec,
            string chuc_vu,
            string dia_chi,
            string ten_tai_khoan, 
            string mat_khau,
            string Com_mat_khau) 
            // đầu vào là name của các "Đối tượng html" trong view Upserts muốn truyền cho HttpPost để upload dữ liệu lên cho server
        {
            if(ModelState.IsValid)  // nếu các trường nhập vào cho view Upsert không bị sai quy tắc
            {
                var parameter = new DynamicParameters(); // tạo 1 dynamic parameters để lưu các tham số truyền vào
              
                parameter.Add("@TENNV", ten_nhan_vien);
                parameter.Add("@GIOITINH", gioi_tinh);
                parameter.Add("@NGAYSINH", ngay_sinh);
                parameter.Add("@CMND", cmnd);
                parameter.Add("@SDT", sdt);
                parameter.Add("@DIACHI",dia_chi );
                parameter.Add("@USERNAME", ten_tai_khoan);
                parameter.Add("@NVPASSWORD", mat_khau);
                parameter.Add("@NGAYLAMVIEC", ngay_lam_viec);
                parameter.Add("@CHUCVU", chuc_vu);
                var result = _unitOfWork.SP_Call.Excute(SD.Nhan_Vien.CREATE, parameter); 
                // _unitOfWork.Sp_Call.Excute là hàm excute 1 hàm SQL, với đầu vào gồm 2 biến(biến thứ nhất kiểu string là tên stored procedure,
                //  biến thứ 2 kiểu Dynamic Parameters là tham số truyền vào cho stored procedure)
                if (result.success) // nếu hàm thực thi thành công 
                {
                    return RedirectToAction(nameof(Index)); // thì trả về View Index
                }
                else
                {
                    if (result.message.Contains("duplicate")) // nếu message của result có chứa từ duplicate
                        result.message = "Tên tài khoản bị trùng, vui lòng chọn tài khoản khác"; // gán result.message là tên tài khoản bị trùng
                    ModelState.AddModelError("", result.message); // Show model error có (key) bằng chuỗi rỗng
                    // (key) quyết định vị trí show model error ở ngay trên key đó
                    // Ở đây key bằng chuỗi rỗng tức là show lên đầu trang 
                    // Nội dung đưa vào cho model error là result.message
                }
            }
            ViewBag.ListChucVu = GetSelectItemsChucVu(); // nếu mã phân loại đã tồn tại thì gán lại List Chức 
            return View();
        }
        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Nhan_Vien.GET_ALL); // gọi procedure lấy danh sách Nhân Viên
            if(result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
        public IActionResult Get(string id)
        {
            var parameter = new DynamicParameters(); // tạo parameter lưu thông tin
            parameter.Add("@MANV", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Nhan_Vien.GET,parameter); // gọi hàm lấy thông tin của nhân viên
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
        //        var a = $('#ma_nhan_vien').val()
        //var b = $('#ma_nhan_vien_old').val()
        //var c = $('#ten_nhan_vien').val()
        //var d = $('#ngay_sinh').val()
        //var e = $('#gioi_tinh').val()
        //var f = $('#cmnd').val()
        //var g = $('#sdt').val()
        //var h = $('#chuc_vu').val()
        //var k = $('#dia_chi').val()
        //var l = $('#user_name').val()
        [HttpPost]
        public IActionResult Update(string a,string b,string c,string d,string e,string f,string g,string h,string k,string l)
        {
   
            var parameter = new DynamicParameters(); // tạo parameter lưu thông tin
            parameter.Add("@MANV", a);
            parameter.Add("@TENNV", c);
            parameter.Add("@GIOITINH", e);
            parameter.Add("@NGAYSINH",d);
            parameter.Add("@NGAYLAMVIEC", b);
            parameter.Add("@CMND", f);
            parameter.Add("@SDT", g);
            parameter.Add("@DIACHI", k);
            parameter.Add("@CHUCVU", h);
            var result = _unitOfWork.SP_Call.Excute(SD.Nhan_Vien.UPDATE, parameter); // gọi stored procedure update Nhân Viên
            if (result.success)
            {
                return Json(new { success = true, message = "đã sửa thành công" });
            }
            return Json(new { success = false, message = result.message });
        }
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var parameter = new DynamicParameters(); // tạo parameter lưu thông tin
            parameter.Add("@MANV", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Nhan_Vien.DELETE, parameter); // gọi hàm xóa nhân viên
            if (result.success)
            {
                return Json(new { success = true, message = "xóa thành công" });
            }
            else
                return Json(new { success = false, message = result.message });


        }
    }
}
