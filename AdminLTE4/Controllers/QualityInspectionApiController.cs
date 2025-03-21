using FILog.Data;
using Microsoft.AspNetCore.Mvc;
using FILog.Models;
using Microsoft.EntityFrameworkCore;


namespace FILog.Controllers
{
    public class QualityInspectionApiController:Controller
    {
        private readonly OpsProdDbContext _context;

        public QualityInspectionApiController(OpsProdDbContext context)
        {
            _context = context;
        }

        [HttpGet("QualityInspection/ReadData")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.MaterialMaster.ToListAsync();
            return Json(data);
        }

        // API untuk mendapatkan data berdasarkan OrderNumber
        [HttpGet("QualityInspectionApi/GetByOrder/{orderNumber}")]
        public async Task<IActionResult> GetMaterialByOrderNumber(string orderNumber)
        {
            var material = await _context.MaterialMaster
                .Where(m => m.OrderNumber == orderNumber)
                .Select(m => new
                {
                    m.PartNumber,
                    m.Description,
                    m.BatchNumber,
                    m.Quantity
                })
                .FirstOrDefaultAsync();


            if (material == null)
            {
                return Json(new { message = "Data tidak ditemukan." });
            }

            return Json(material);
        }

    }
}
