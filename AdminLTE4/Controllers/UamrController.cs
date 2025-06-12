using FILog.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FILog.Controllers
{
    public class UamrController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly OpsProdDbContext _context;
        public UamrController(OpsProdDbContext context)
        {
            _context = context;
        }

        [HttpGet("Uamr/ReadDataAll")]
        public async Task<IActionResult> ReadData()
        {
            var draw = Request.Query["draw"].FirstOrDefault();
            var start = Request.Query["start"].FirstOrDefault();
            var length = Request.Query["length"].FirstOrDefault();
            var searchValue = Request.Query["search[value]"].FirstOrDefault();
            
            // Ambil parameter status dari request form
            var status = Request.Query["status"].FirstOrDefault();
            var exclude = Request.Query["exclude"].FirstOrDefault()?.ToLower() == "true";

            int pageSize = length != null ? Convert.ToInt32(length) : 5;
            int skip = start != null ? Convert.ToInt32(start) : 0;


            var query = _context.eLOGMasterialMasterModel.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                if (exclude)
                    query = query.Where(x => x.Order_Status != status);
                else
                    query = query.Where(x => x.Order_Status == status);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x =>
                    x.OrderNumber.Contains(searchValue) ||
                    x.PartNumber.Contains(searchValue) ||
                    x.Description.Contains(searchValue) ||
                    x.Quantity.HasValue && x.Quantity.Value.ToString().Contains(searchValue) ||
                    x.BatchNumber.Contains(searchValue) ||
                    x.Order_Status.Contains(searchValue) ||
                    x.Current_OP.Contains(searchValue) ||
                    x.Current_WC.Contains(searchValue));
            }

            var recordsTotal = await query.CountAsync();
            var data = await query.Skip(skip).Take(pageSize).ToListAsync();

            return Ok(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = data
            });
        }

    }
}