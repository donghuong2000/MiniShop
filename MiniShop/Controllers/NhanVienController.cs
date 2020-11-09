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
    public class NhanVienController : Controller
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
            string objstring = _unitOfWork.SP_Call.Excute(SD.Chuc_vu.GET_ALL).message;
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
        {
            if(ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@MANV", ma_nhan_vien);
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
                if (result.success)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    if (result.message.Contains("duplicate"))
                        result.message = "Mã phân loại đã tồn tại";
                    ModelState.AddModelError("", result.message);
                }
            }
            ViewBag.ListChucVu = GetSelectItemsChucVu();
            return View();
        }
        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Nhan_Vien.GET_ALL);
            if(result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
        public IActionResult Get(string id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MANV", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Nhan_Vien.GET,parameter);
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            return NotFound();
        }
    }
}
