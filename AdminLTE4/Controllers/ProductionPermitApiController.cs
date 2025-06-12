using FILog.Data;
using FILog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace FILog.Controllers
{
    public class ProductionPermitApiController : Controller
    {
        private OpsProdDbContext _context;
        public ProductionPermitApiController(OpsProdDbContext context)
        {
            _context = context;
        }

        [HttpPost("Production/CreateData")]
        public async Task<IActionResult> CreateData([FromBody] DMSProductionPermitModel model) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Data tidak valid" });
            }

            _context.DMSProductionPermit.Add(model);
            await _context.SaveChangesAsync();
            return Json(new { message = "Data berhasil ditambahkan", data = model });
        }

    
        [HttpPost("Production/ReadDataAll")]
        public async Task<IActionResult> ReadData()
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            // Mengecek apakah start dan length kosong dan memberikan default value
            int pageSize = length != null ? Convert.ToInt32(length) : 5;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            var query = _context.DMSProductionPermit.AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                DateTime searchDate;
                bool isDateSearch = DateTime.TryParse(searchValue, out searchDate);

                query = query.Where(x =>
                    x.Part_Number.Contains(searchValue) ||
                    x.Description.Contains(searchValue) ||
                    x.Production_Permit.Contains(searchValue) ||
                    (isDateSearch && x.Start_Date.HasValue && x.Start_Date.Value.Date == searchDate.Date) ||
                    (isDateSearch && x.End_Date.HasValue && x.End_Date.Value.Date == searchDate.Date) ||
                    x.Remarks.Contains(searchValue) ||
                    (x.Qty.HasValue && x.Qty.Value.ToString().Contains(searchValue)) ||
                    (x.Remaining_Qty.HasValue && x.Remaining_Qty.Value.ToString().Contains(searchValue)) ||
                    x.Status.Contains(searchValue) ||
                    (x.Alert_Qty.HasValue && x.Alert_Qty.Value.ToString().Contains(searchValue)) ||
                    x.Alert_Email.Contains(searchValue));

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


        [HttpPut("Production/UpdateData/{id}")]
        public async Task<IActionResult> UpdateData(int id, [FromBody] DMSProductionPermitModel model)
        {
            var data = await _context.DMSProductionPermit.FindAsync(id);
            if (data == null) return NotFound();

            if (model == null)
            {
                return BadRequest("Model null. Periksa kembali nama properti JSON.");
            }

            data.Part_Number = model.Part_Number;
            data.Description = model.Description;
            data.Production_Permit = model.Production_Permit;
            data.Start_Date = model.Start_Date;
            data.End_Date = model.End_Date;
            data.Remarks = model.Remarks;
            data.Qty = model.Qty;
            data.Remaining_Qty = model.Remaining_Qty;
            data.Status = model.Status;
            data.Alert_Qty = model.Alert_Qty;
            data.Alert_Email = model.Alert_Email;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Data berhasil diupdate", data });
        }


        [HttpDelete("Production/DeleteData/{id}")]
        public async Task<IActionResult> DeleteData(int id)
        {
            var data = await _context.DMSProductionPermit.FindAsync(id);
            if (data == null) return NotFound();

            _context.DMSProductionPermit.Remove(data);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data berhasil dihapus" });
        }


        [HttpPost("Production/PreviewExcel")]
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

        [HttpPost("Production/ImportExcel")]
        public async Task<IActionResult> ImportExcel([FromBody] List<Dictionary<string, string>> data)
        {
            var newData = new List<DMSProductionPermitModel>();

            foreach (var row in data)
            {
                try
                {
                    var model = new DMSProductionPermitModel
                    {
                        Part_Number = row.GetValueOrDefault("Part Number"),
                        Description = row.GetValueOrDefault("Description"),
                        Production_Permit = row.GetValueOrDefault("Production Permit"),
                        Start_Date = DateTime.TryParse(row.GetValueOrDefault("Start Date"), out DateTime startDate) ? startDate : (DateTime?)null,
                        End_Date = DateTime.TryParse(row.GetValueOrDefault("End Date"), out DateTime endDate) ? endDate : (DateTime?)null,
                        Remarks = row.GetValueOrDefault("Remarks"),
                        Qty = decimal.TryParse(row.GetValueOrDefault("Qty"), out decimal qty) ? qty : 0m,  // Ubah ke decimal
                        Remaining_Qty = decimal.TryParse(row.GetValueOrDefault("Remaining Qty"), out decimal remainingQty) ? remainingQty : 0m,  // Ubah ke decimal
                        Status = row.GetValueOrDefault("Status"),
                        Alert_Qty = decimal.TryParse(row.GetValueOrDefault("Alert Qty"), out decimal alertQty) ? alertQty : 0m,  // Ubah ke decimal
                        Alert_Email = row.GetValueOrDefault("Alert Email")
                    };

                    newData.Add(model);
                }
                catch
                {
                    // Bisa log atau abaikan jika ada error pada baris tertentu
                    continue;
                }
            }

            _context.DMSProductionPermit.AddRange(newData);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data berhasil diimport", count = newData.Count });
        }

        [HttpGet("Production/ExportExcel")]
        public async Task<ActionResult> ExportExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var data = await _context.DMSProductionPermit.ToListAsync();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DMS Production Permit");

                // buat headernya
                worksheet.Cells[1, 1].Value = "Part Number";
                worksheet.Cells[1, 2].Value = "Description";
                worksheet.Cells[1, 3].Value = "Production Permit";
                worksheet.Cells[1, 4].Value = "Start Date";
                worksheet.Cells[1, 5].Value = "End Date";
                worksheet.Cells[1, 6].Value = "Remarks";
                worksheet.Cells[1, 7].Value = "Qty";
                worksheet.Cells[1, 8].Value = "Remaining Qty";
                worksheet.Cells[1, 9].Value = "Status";
                worksheet.Cells[1, 10].Value = "Alert Qty";
                worksheet.Cells[1, 11].Value = "Alert Email";

                // Data
                for (int i = 0; i < data.Count; i++)
                {
                    var row = i + 2;
                    worksheet.Cells[row, 1].Value = data[i].Part_Number;
                    worksheet.Cells[row, 2].Value = data[i].Description;
                    worksheet.Cells[row, 3].Value = data[i].Production_Permit;
                    worksheet.Cells[row, 4].Value = data[i].Start_Date;
                    worksheet.Cells[row, 5].Value = data[i].End_Date;
                    worksheet.Cells[row, 6].Value = data[i].Remarks;
                    worksheet.Cells[row, 7].Value = data[i].Qty;
                    worksheet.Cells[row, 8].Value = data[i].Remaining_Qty;
                    worksheet.Cells[row, 9].Value = data[i].Status;
                    worksheet.Cells[row, 10].Value = data[i].Alert_Qty;
                    worksheet.Cells[row, 11].Value = data[i].Alert_Email;
                }

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DMS_Production.xlsx");
            }
        }




    }
}
