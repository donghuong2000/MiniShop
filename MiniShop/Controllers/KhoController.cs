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
    public class KhoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public KhoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        //get all
        public IActionResult GetAll()
        {
            var result = _unitOfWork.SP_Call.Excute(SD.Kho.GET_ALL);
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
            parameter.Add("@MA_SAN_PHAM", id);
            var result = _unitOfWork.SP_Call.Excute(SD.Kho.GET, parameter);
            if (result.success)
            {
                return Content(result.message, "application/json");
            }
            else
                return NotFound("Không tìm thấy sản phẩm này");

        }

    }
}
