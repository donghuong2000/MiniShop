using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MiniShop.Exten;
using MiniShop.Repository.IRepository;

namespace MiniShop.Controllers
{
    public class DonNhapHangController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DonNhapHangController(IUnitOfWork unitOfWork)
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
        [HttpGet]
        public IActionResult Detail()
        {
            return View();
        }
        //get all
        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Don_Nhap_Hang.GET_ALL);
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            else
                return NotFound();
        }
        //get
        public IActionResult Get(string id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MA_DNH", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Don_Nhap_Hang.GET, parameter);
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            else
                return NotFound("Không tìm thấy đơn nhập hàng này");
        }
        [HttpPost]
        public IActionResult Upsert(string ma_don_nhap_hang, string ma_nha_cung_cap, string ngay_nhap)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MA_DNH", ma_don_nhap_hang);
            parameter.Add("@MA_NCC", ma_nha_cung_cap);
            parameter.Add("@NGAYNHAP", ngay_nhap);
            var result = _unitOfWork.SP_Call.Excute(SD.Don_Nhap_Hang.CREATE, parameter);
            if (result.success)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (result.message.Contains("duplicate"))
                    result.message = "Đơn nhập hàng đã tồn tại";
                ModelState.AddModelError("", result.message);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Update(string oldId, string newId, string ma_nha_cung_cap, string ngay_nhap)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MA_DNH_OLD", oldId);
            parameter.Add("@MA_DNH_NEW", newId);
            parameter.Add("@MA_NCC", ma_nha_cung_cap);
            parameter.Add("@NGAYNHAP", ngay_nhap);
            var result = _unitOfWork.SP_Call.Excute(SD.Don_Nhap_Hang.UPDATE, parameter);
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
            var parameter = new DynamicParameters();
            parameter.Add("@MA_DNH", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Don_Nhap_Hang.DELETE, parameter);
            if (result.success)
            {
                return Json(new { success = true, message = "Xóa thành công" });
            }
            else
                return Json(new { success = false, message = result.message });
        }
    }
}
