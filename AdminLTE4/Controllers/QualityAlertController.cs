using FILog.Data;
using FILog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FILog.Controllers
{
    public class QualityAlertController : Controller
    {
        private readonly OpsProdDbContext _context;
        public QualityAlertController(OpsProdDbContext context)
        {
            _context = context;
        }
        public IActionResult QualityAlert()
        {
            return View();
        }

        [HttpPost("QualityAlert/CreateData")]
        public async Task<IActionResult> CreateData([FromForm] eLOGQualityAlertModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Data tidak valid" });
            }

            string? fileName = null;
            string? filePath = null;

            var uploadFile = Request.Form.Files.FirstOrDefault(f => f.Name == "Image"); // Ambil file dari request
            if (uploadFile != null)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Simpan file dengan nama unik (misalnya GUID)
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadFile.FileName);
                filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(stream);
                }
            }

            var newData = new eLOGQualityAlertModel
            {
                Part_Number = model.Part_Number,
                Image = fileName // Simpan nama file saja
            };

            _context.ELOGQualityAlert.Add(newData);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data berhasil ditambahkan", fileUpload = fileName, data = newData });


        }

        [HttpPost("QualityAlert/ReadData")]
        public async Task<IActionResult> ReadDataAll()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 5;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            var query = _context.ELOGQualityAlert.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x =>
                    x.Part_Number.Contains(searchValue) ||
                    x.Image.Contains(searchValue));
            }

            var recordsTotal = await query.CountAsync();

            var data = await query
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new {
                    x.id,
                    Part_Number = x.Part_Number,
                    Image = x.Image
                })
                .ToListAsync();

            return Ok(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = data
            });
        }


        [HttpPut("QualityAlert/UpdateData/{id}")]
        public async Task<IActionResult> UpdateData(int id, [FromForm] eLOGQualityAlertModel model, IFormFile? imageQuality)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Data tidak valid" });
            }

            var existingData = await _context.ELOGQualityAlert.FindAsync(id);
            if (existingData == null)
            {
                return NotFound(new { message = "Data tidak ditemukan" });
            }

            existingData.Part_Number = model.Part_Number;
            existingData.Image = model.Image;


            if (imageQuality != null && imageQuality.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string dateTimeString = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var uniqueFileNameUpdate = $"{Path.GetFileNameWithoutExtension(imageQuality.FileName)}.{dateTimeString}{Path.GetExtension(imageQuality.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileNameUpdate);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageQuality.CopyToAsync(stream);
                }

                existingData.Image = uniqueFileNameUpdate;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Data berhasil diperbarui", data = existingData });
        }



    }
}
