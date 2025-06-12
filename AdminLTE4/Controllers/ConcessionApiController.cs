using FILog.Data;
using FILog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
namespace FILog.Controllers
{
    public class ConcessionApiController : Controller
    {

        private readonly OpsProdDbContext _context;
        
        public ConcessionApiController(OpsProdDbContext context)
        {
            _context = context;
        }

        [HttpPost("Concession/CreateData")]
        public async Task<IActionResult> CreateData([FromBody] DMSConcessionModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Data tidak valid" });
            }

            _context.DMSConcession.Add(model);
            await _context.SaveChangesAsync();
            return Json(new { message = "Data berhasil ditambahkan", data = model });
        }


        [HttpPost("Concession/ReadData")]
        public async Task<IActionResult> ReadData()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 5;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            var query = _context.DMSConcession.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x =>
                    x.Order_Number.Contains(searchValue) ||
                    x.Batch_Number.Contains(searchValue) ||
                    x.Part_Number.Contains(searchValue) ||
                    x.Description.Contains(searchValue) ||
                    x.Concession.Contains(searchValue) ||
                    x.Remarks.Contains(searchValue));
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
        
        [HttpPut("Concession/UpdateData/{id}")]
        public async Task<IActionResult> UpdateData(int id, [FromBody] DMSConcessionModel model)
        {
            var data = await _context.DMSConcession.FindAsync(id);
            if (data == null) return NotFound();

            data.Order_Number = model.Order_Number;
            data.Batch_Number = model.Batch_Number;
            data.Part_Number = model.Part_Number;
            data.Description = model.Description;
            data.Concession = model.Concession;
            data.Remarks = model.Remarks;
            data.LabelForm = model.LabelForm;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Data berhasil diupdate", data });
        }

        // DELETE: Concession/DeleteData
        [HttpDelete("Concession/DeleteData/{id}")]
        public async Task<IActionResult> DeleteData(int id)
        {
            var data = await _context.DMSConcession.FindAsync(id);
            if (data == null) return NotFound();

            _context.DMSConcession.Remove(data);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data berhasil dihapus" });
        }


        [HttpPost("Concession/PreviewExcel")]
        public async Task<IActionResult> PreviewExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File tidak valid.");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; //mengaktifkan lisensi

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.First();
            var rowCount = worksheet.Dimension.Rows;
            var colCount = worksheet.Dimension.Columns;

            var headers = new List<string>();
            for (int col = 1; col <= colCount; col++)
                headers.Add(worksheet.Cells[1, col].Text);

            var result = new List<Dictionary<string, string>>();
            for (int row = 2; row <= rowCount; row++)
            {
                var rowDict = new Dictionary<string, string>();
                for (int col = 1; col <= colCount; col++)
                {
                    rowDict[headers[col - 1]] = worksheet.Cells[row, col].Text;
                }
                result.Add(rowDict);
            }

            return Ok(result); // Untuk ditampilkan di preview
        }


        [HttpPost("Concession/ImportExcel")]
        public async Task<IActionResult> ImportExcel([FromBody] List<Dictionary<string, string>> data)
        {
            var newData = new List<DMSConcessionModel>();

            foreach (var row in data)
            {
                try
                {
                    var model = new DMSConcessionModel
                    {
                        Order_Number = row.GetValueOrDefault("Order Number"),
                        Batch_Number = row.GetValueOrDefault("Batch Number"),
                        Part_Number = row.GetValueOrDefault("Part Number"),
                        Description = row.GetValueOrDefault("Description"),
                        Concession = row.GetValueOrDefault("Concession"),
                        Remarks = row.GetValueOrDefault("Remarks"),
                        LabelForm = row.GetValueOrDefault("Label Form")
                    };

                    newData.Add(model);
                }
                catch
                {
                    // Bisa log atau abaikan jika ada error pada baris tertentu
                    continue;
                }
            }

            _context.DMSConcession.AddRange(newData);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data berhasil diimport", count = newData.Count });
        }

        [HttpGet("Concession/ExportExcel")]
        public async Task<ActionResult> ExportExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var data = await _context.DMSConcession.ToListAsync();
            using (var package =new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DMS COncession");

                // buat headernya
                worksheet.Cells[1, 1].Value = "Order Number";
                worksheet.Cells[1, 2].Value = "Batch Number";
                worksheet.Cells[1, 3].Value = "Part Number";
                worksheet.Cells[1, 4].Value = "Description";
                worksheet.Cells[1, 5].Value = "Concession";
                worksheet.Cells[1, 6].Value = "Remarks";
                worksheet.Cells[1, 7].Value = "Label Form";

                // Data
                for (int i = 0; i < data.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cells[row, 1].Value = data[i].Order_Number;
                    worksheet.Cells[row, 2].Value = data[i].Batch_Number;
                    worksheet.Cells[row, 3].Value = data[i].Part_Number;
                    worksheet.Cells[row, 4].Value = data[i].Description;
                    worksheet.Cells[row, 5].Value = data[i].Concession;
                    worksheet.Cells[row, 6].Value = data[i].Remarks;
                    worksheet.Cells[row, 7].Value = data[i].LabelForm;
                }

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DMS_Concession.xlsx");
            }
        }

    }

}
