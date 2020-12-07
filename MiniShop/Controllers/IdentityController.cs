using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniShop.Repository.IRepository;

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
