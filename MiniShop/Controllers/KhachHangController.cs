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
    public class KhachHangController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public KhachHangController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            ViewBag.LKH = GetSelectItemsLoaiKH();
            return View();
        }
        [HttpGet]
        public IActionResult Upsert()
        {
            ViewBag.LKH = GetSelectItemsLoaiKH();
            return View();
        }
        [HttpPost]
        public IActionResult Upsert(string tkh, string ns, string cmnd, string sdt, string dc, string gt, string lkh)
        { // đầu vào là name của các "Đối tượng html" trong view Upserts muốn truyền cho HttpPost để upload dữ liệu lên cho server
            if (ModelState.IsValid)
            {

                var parameter = new DynamicParameters(); // tạo 1 dynamic parameters để lưu các tham số truyền vào
                parameter.Add("@MAKH",Guid.NewGuid().ToString());
                parameter.Add("@TENKH", tkh);
                parameter.Add("@GIOITINH", gt);
                parameter.Add("@NGAYSINH", ns);
                parameter.Add("@CMND", cmnd);
                parameter.Add("@SDT", sdt);
                parameter.Add("@DIACHI", dc);
                parameter.Add("@LOAIKH", lkh);
                var result = _unitOfWork.SP_Call.Excute(SD.Khach_Hang.CREATE, parameter);
                // _unitOfWork.Sp_Call.Excute là hàm excute 1 hàm SQL, với đầu vào gồm 2 biến(biến thứ nhất kiểu string là tên stored procedure,
                //  biến thứ 2 kiểu Dynamic Parameters là tham số truyền vào cho stored procedure)
                if (result.success) // nếu hàm thực thi thành công 
                {
                    return RedirectToAction(nameof(Index)); // thì trả về View Index
                }
                else
                {
                    ModelState.AddModelError("", result.message);  // nếu hàm thực thi lỗi thì trả về model error với nội dung là lỗi đó
                }
            }
            return View();
        }
        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Khach_Hang.GET_ALL); // gọi procedure lấy danh sách Khách Hàng
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
        private IEnumerable<SelectListItem> GetSelectItemsLoaiKH()
        {
            string objstring = _unitOfWork.SP_Call.Excute(SD.Loai_Khach_Hang.GET_ALL).message;
            // chuẩn hóa cho phù hợp
            objstring = objstring.Substring(8, objstring.Length - 9);
            var obj = JArray.Parse(objstring);
            // chuyển đổi thành selectlist item

            var list = obj.Select(x => new SelectListItem(x["TENLOAI"].ToString(), x["MALOAIKH"].ToString()));
            return list;
        }
        public IActionResult Get(string id)
        {
            var parameter = new DynamicParameters(); // tạo parameter lưu thông tin
            parameter.Add("@MAKH", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Khach_Hang.GET, parameter); // gọi procedure lấy thông tin của khách hàng
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
        //        var a = $('#tkh').val()
        //var b = $('#ns').val()
        //var c = $('#cmnd').val()
        //var d = $('#sdt').val()
        //var e = $('#gt').val()
        //var f = $('#dc').val()
        //var g = $('#lkh').val()
        //var h = $('#ma').val()
        [HttpPost]
        public IActionResult Update(string a, string b, string c, string d, string e, string f,string g,string h)
        {
   
            var parameter = new DynamicParameters();
            parameter.Add("@MAKH", h);
            parameter.Add("@TENKH", a);
            parameter.Add("@GIOITINH", e);
            parameter.Add("@NGAYSINH", b);
            parameter.Add("@CMND", c);
            parameter.Add("@SDT", d);
            parameter.Add("@DIACHI", f);
            parameter.Add("@LOAIKH", g);
            var result = _unitOfWork.SP_Call.Excute(SD.Khach_Hang.UPDATE, parameter);
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
            parameter.Add("@MAKH", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Khach_Hang.DELETE, parameter); // gọi stored procedure xóa Khách Hàng
            if (result.success)
            {
                return Json(new { success = true, message = "xóa thành công" });
            }
            else
                return Json(new { success = false, message = result.message });
        }
    }
}
