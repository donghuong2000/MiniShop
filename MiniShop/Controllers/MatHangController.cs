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
    public class MatHangController : BaseHomeController
    {
        private readonly IUnitOfWork _unitOfWork;
        public MatHangController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            ViewBag.PhanLoaiList = GetSelectItemsPhanLoai();
            return View();
        }
        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Mat_Hang.GET_ALL); // gọi procedure lấy danh sách mặt hàng
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
        public IActionResult Get(string id)
        {
            var parameter = new DynamicParameters(); // tạo parameter lưu thông tin
            parameter.Add("@MAMH", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Mat_Hang.GET,parameter); // gọi procedure lấy thông tin của mặt hàng
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
        
        public IActionResult Create()
        {
            ViewBag.PhanLoaiList = GetSelectItemsPhanLoai();
            return View();
        }
        [HttpPost]
        public IActionResult Create(string a,string b, string c, string d, string e, string f)
        { // đầu vào là name của các "Đối tượng html" trong view Upserts muốn truyền cho HttpPost để upload dữ liệu lên cho server
            ViewBag.PhanLoaiList = GetSelectItemsPhanLoai(); // get danh sách tên các "Phân Loại" và truyền cho viewbag.PhanLoaiList để truyền vào view Create
            var parameter = new DynamicParameters();  // tạo 1 dynamic parameters để lưu các tham số truyền vào
            parameter.Add("@MAMH", a);
            parameter.Add("@TENMH", b);
            parameter.Add("@NGAYSX", c);
            parameter.Add("@HANSD", d);
            parameter.Add("@LOAIMH", e);
            parameter.Add("@GIA", f);
            var result = _unitOfWork.SP_Call.Excute(SD.Mat_Hang.CREATE, parameter);
            // _unitOfWork.Sp_Call.Excute là hàm excute 1 hàm SQL, với đầu vào gồm 2 biến(biến thứ nhất kiểu string là tên stored procedure,
            //  biến thứ 2 kiểu Dynamic Parameters là tham số truyền vào cho stored procedure)
            if (result.success) // nếu hàm thực thi thành công 
            {
                return RedirectToAction("index"); // thì trả về View Index
            }
            ModelState.AddModelError("", result.message); // nếu hàm không thực thi thành công thì gán lỗi cho model error và show lên màn hình
            return View();
        }
        [HttpPost]
        public IActionResult Update(string a, string b, string c, string d, string e, string f, string aa)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MA_MH_OLD", aa);
            parameter.Add("@MA_MH_NEW", a);
            parameter.Add("@TEN_MH", b);
            parameter.Add("@NGAYSX", c);
            parameter.Add("@HANSD", d);
            parameter.Add("@LOAIMH", e);
            parameter.Add("@GIA", f);
            var result = _unitOfWork.SP_Call.Excute(SD.Mat_Hang.UPDATE, parameter);
            if (result.success)
            {
                return Json(new { success = true, message = "Update thành công sản phẩm" });
            }
            return Json(new { success = false, message = result.message });
        }
       
        //public IActionResult Update(string a, string b, string c, string d, string e, string f, string g, string h, string k, string l)
        //{

        //    var parameter = new DynamicParameters(); // tạo parameter lưu thông tin
        //    parameter.Add("@MANV", a);
        //    parameter.Add("@TENNV", c);
        //    parameter.Add("@GIOITINH", e);
        //    parameter.Add("@NGAYSINH", d);
        //    parameter.Add("@NGAYLAMVIEC", b);
        //    parameter.Add("@CMND", f);
        //    parameter.Add("@SDT", g);
        //    parameter.Add("@DIACHI", k);
        //    parameter.Add("@CHUCVU", h);
        //    var result = _unitOfWork.SP_Call.Excute(SD.Nhan_Vien.UPDATE, parameter); // gọi stored procedure update Nhân Viên
        //    if (result.success)
        //    {
        //        return Json(new { success = true, message = "đã sửa thành công" });
        //    }
        //    return Json(new { success = false, message = result.message });
        //}


        [HttpDelete]
        public IActionResult Delete(string id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MA_MH", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Mat_Hang.DELETE,parameter);
            if (result.success)
            {
                return Json(new { success = true, message = "Xóa thành công sản phẩm" });
            }
            return Json(new { success = false, message = result.message });
        }
    }
}
