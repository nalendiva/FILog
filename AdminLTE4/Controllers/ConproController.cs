using Microsoft.AspNetCore.Mvc;

namespace FILog.Controllers
{
    public class ConproController : Controller
    {
        public IActionResult Concession()
        {
            ViewBag.ActiveTab = "Concession";
            return View();
        }
        public IActionResult ProductionPermit()
        {
            ViewBag.ActiveTab = "ProductionPermit";
            return View();
        }

    }
}
