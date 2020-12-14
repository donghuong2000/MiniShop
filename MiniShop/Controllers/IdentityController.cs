using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniShop.Exten;
using MiniShop.Repository.IRepository;
using Newtonsoft.Json.Linq;

namespace MiniShop.Controllers
{
    public class IdentityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public IdentityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username,string password)
        {
            var result = _unitOfWork.SP_Call.Login(username, password);
            if(result==true)
            {
                Request.HttpContext.Session.SetString("U", username);
                var parameter = new DynamicParameters();
                parameter.Add("@MANV", username);
                var x = _unitOfWork.SP_Call.Excute(SD.Nhan_Vien.GET,parameter);
                if(x.success)
                {
                    string objstring = x.message;
                    // chuẩn hóa cho phù hợp
                    objstring = objstring.Substring(8, objstring.Length - 9);
                    var obj = JArray.Parse(objstring);
                    // chuyển đổi thành selectlist item
                    var role = obj[0]["CHUCVU"].ToString();
                    Request.HttpContext.Session.SetString("R", role);
                }
                return RedirectToAction("Index", "home");
            }
            ModelState.AddModelError("", "đăng nhập thất bại, vui lòng kiểm tra lại tài khoản, mật khẩu");
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Logout()
        {

            Request.HttpContext.Session.Remove("U");
            return RedirectToAction("login");
        }
    }
}
