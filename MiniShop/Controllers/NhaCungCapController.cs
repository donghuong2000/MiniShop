using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using MiniShop.Exten;
using MiniShop.Repository.IRepository;

namespace MiniShop.Controllers
{
    public class NhaCungCapController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public NhaCungCapController(IUnitOfWork unitOfWork)
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
        //get all
        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Nha_Cung_Cap.GET_ALL);
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
            parameter.Add("@MA_NHA_CUNG_CAP", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Nha_Cung_Cap.GET, parameter);
            if (result.success)
            {
                return Content(result.message, "application/json");
            } 
            else
                return NotFound("Không tìm thấy nhà cung cấp này");
        }
        [HttpPost]
        public IActionResult Upsert(string ma_nha_cung_cap, string ten_nha_cung_cap, string dia_chi, string sdt, string stk)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MA_NHA_CUNG_CAP", ma_nha_cung_cap);
            parameter.Add("@TEN_NHA_CUNG_CAP", ten_nha_cung_cap);
            parameter.Add("@DIA_CHI", dia_chi);
            parameter.Add("@SDT", sdt);
            parameter.Add("@STK", stk);
            var result = _unitOfWork.SP_Call.Excute(SD.Nha_Cung_Cap.CREATE, parameter);
            if (result.success)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (result.message.Contains("duplicate"))
                    result.message = "Nhà cung cấp đã tồn tại";
                ModelState.AddModelError("", result.message);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Update(string oldId, string newId, string ten_nha_cung_cap, string dia_chi, string sdt, string stk)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@MA_NHA_CUNG_CAP_OLD", oldId);
            parameter.Add("@MA_NHA_CUNG_CAP_NEW", newId);
            parameter.Add("@TEN_NHA_CUNG_CAP", ten_nha_cung_cap);
            parameter.Add("@DIA_CHI", dia_chi);
            parameter.Add("@SDT", sdt);
            parameter.Add("@STK", stk);
            var result = _unitOfWork.SP_Call.Excute(SD.Nha_Cung_Cap.UPDATE, parameter);
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
            parameter.Add("@MA_NHA_CUNG_CAP", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Nha_Cung_Cap.DELETE, parameter);
            if (result.success)
            {
                return Json(new { success = true, message = "Xóa thành công" });
            }
            else
                return Json(new { success = false, message = result.message });


        }
    }
}
