using Microsoft.AspNetCore.Mvc;

namespace FILog.Controllers
{
    public class QualityInspectionController : Controller
    {

        // Fitur Halaman Start Inspection
        public IActionResult StartInspection()
        {
            ViewBag.ActiveTab = "FinalInspection";
            return View();
        }

        public IActionResult ReceivingInspection()
        {
            ViewBag.ActiveTab = "ReceivingInspection";
            return View();
        }

        public IActionResult UploadAfter()
        {
            ViewBag.ActiveTab = "UploadAfter";
            return View();
        }

        public IActionResult RackInformation()
        {
            ViewBag.ActiveTab = "RackInformation";
            return View();
        }

        // Fitur Halaman Finish Inspection
        public IActionResult FinishInspection()
        {
            ViewBag.ActiveTab = "FinalInspectionFinish";
            return View();
        }

        public IActionResult ReceivingInspectionFinish()
        {
            ViewBag.ActiveTab = "ReceivingInspectionFinish";
            return View();
        }

        public IActionResult QualityAlertFinish()
        {
            ViewBag.ActiveTab = "QualityAlertFinish";
            return View();
        }


    }
}
