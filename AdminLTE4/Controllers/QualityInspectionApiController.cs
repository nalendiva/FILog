using FILog.Data;
using Microsoft.AspNetCore.Mvc;
using FILog.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;


namespace FILog.Controllers
{
    public class QualityInspectionApiController : Controller
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
            var materialData = await (from material in _context.MaterialMaster

                                      join engineering in _context.EngineeringPortal
                                      on material.PartNumber equals engineering.PartNumber into materialJoin
                                      from eng in materialJoin.DefaultIfEmpty() // Left Join untuk menangani data kosong

                                      // Join ke ConcessionTable
                                      join concession in _context.DMSConcession
                                      on material.PartNumber equals concession.Part_Number into concessionJoin
                                      from con in concessionJoin.DefaultIfEmpty()

                                      //join production in _context.DMSProductionPermit
                                      //on material.PartNumber equals production.Part_Number into productionJoin
                                      //from prod in productionJoin.DefaultIfEmpty()

                                      join qualityAlert in _context.ELOGQualityAlert
                                      on material.PartNumber equals qualityAlert.Part_Number into qualityAlertJoin
                                      from qa in qualityAlertJoin.DefaultIfEmpty()

                                      join tag in _context.DMSMaterialMasterModels
                                      on material.PartNumber equals tag.Part_Number into tagJoin
                                      from taglabel in tagJoin.DefaultIfEmpty()

                                      join kfr in _context.DMSFIKFCModels
                                      on material.PartNumber equals kfr.Part_Number into kfrJoin
                                      from kfrCheck in kfrJoin.DefaultIfEmpty()

                                      join orderStatus in _context.eLOGMasterialMasterModel
                                      on material.PartNumber equals orderStatus.PartNumber into orderStatusJoin
                                      from orderStatusCheck in orderStatusJoin.DefaultIfEmpty()

                                      where material.OrderNumber == orderNumber
                                      select new
                                      {
                                          material.PartNumber,
                                          material.Description,
                                          material.BatchNumber,
                                          material.Quantity,
                                          //material.Current_OP,
                                          FamilyPart = eng != null ? eng.FamilyPart : "N/A",
                                          Concession = con != null ? con.Concession : "N/A",
                                          //// Gunakan subquery untuk ambil semua ProductionPermit aktif
                                          //Production = _context.DMSProductionPermit
                                          //    .Where(p => p.Part_Number == material.PartNumber && p.Status.ToLower() == "active")
                                          //    .Select(p => p.Production_Permit)
                                          //    .DefaultIfEmpty("N/A")
                                          //    .ToList(),

                                          //Status = _context.DMSProductionPermit
                                          //    .Where(p => p.Part_Number == material.PartNumber && p.Status.ToLower() == "active")
                                          //    .Select(p => p.Status)
                                          //    .FirstOrDefault() ?? "N/A",

                                          ImageUrl = qa != null ? "/uploads/" + qa.Image : null,
                                          TagLabel = taglabel != null ? taglabel.FI_Tag : "N/A",
                                          IsKFRActive = (kfrCheck != null && kfrCheck.Status.ToLower() == "active"),
                                          IsUamr = (orderStatusCheck != null && orderStatusCheck.Order_Status.ToLower()=="uamr")


                                      })
                                          .FirstOrDefaultAsync();

            if (materialData == null)
            {
                return Json(new { message = "Data tidak ditemukan." });
            }

            // STEP 2: Ambil data Production permit-nya terpisah (client-side)
            var productionList = await _context.DMSProductionPermit
                .Where(p => p.Part_Number == materialData.PartNumber && p.Status.ToLower() == "active")
                .Select(p => p.Production_Permit)
                .ToListAsync();

            var status = await _context.DMSProductionPermit
                .Where(p => p.Part_Number == materialData.PartNumber && p.Status.ToLower() == "active")
                .Select(p => p.Status)
                .FirstOrDefaultAsync() ?? "N/A";

            // Gabungkan semua production permit jika lebih dari satu
            var formattedData = new
            {
                materialData.PartNumber,
                materialData.Description,
                materialData.BatchNumber,
                materialData.Quantity,
                materialData.FamilyPart,
                materialData.Concession,
                Production = productionList.Any() ? string.Join(", ", productionList) : "N/A",
                Status = status,
                materialData.ImageUrl,
                materialData.TagLabel,
                materialData.IsKFRActive,
                materialData.IsUamr
            };

            return Json(formattedData);
        }


        [HttpGet("QualityInspectionApi/GetDataByUser")]
        public async Task<IActionResult> GetDataByUser()
        {
            var draw = Request.Query["draw"].FirstOrDefault();
            var start = Request.Query["start"].FirstOrDefault();
            var length = Request.Query["length"].FirstOrDefault();
            var searchValue = Request.Query["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 5;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            var username = Environment.UserName;

            //var username = "ale";

            var query = _context.DMSFILogModel
                .Where(x => x.Name == username);

            // Filtering pencarian
            if (!string.IsNullOrEmpty(searchValue))
            {

                DateTime searchDate;
                bool isDateSearch = DateTime.TryParse(searchValue, out searchDate);

                TimeSpan searchTime;
                bool isTimeSearch = TimeSpan.TryParse(searchValue, out searchTime);

                query = query.Where(x =>
                    x.Doc_Number.HasValue && x.Doc_Number.Value.ToString().Contains(searchValue)||
                    x.Order_Number.Contains(searchValue) ||
                    x.Part_Number.Contains(searchValue) ||
                    x.Description.Contains(searchValue) ||
                    x.Batch_Number.Contains(searchValue) ||
                    x.Qty.HasValue && x.Qty.Value.ToString().Contains(searchValue) ||
                    x.Operation.Contains(searchValue) ||
                    (isDateSearch && x.Start_Date.HasValue && x.Start_Date.Value.Date == searchDate.Date) ||
                    (isTimeSearch && x.Start_Time.HasValue &&
                         x.Start_Time.Value.Hours == searchTime.Hours &&
                         x.Start_Time.Value.Minutes == searchTime.Minutes) ||
                    x.Remarks.Contains(searchValue) ||
                    x.Cell.Contains(searchValue)||
                    x.Concession.Contains(searchValue) ||
                    x.Production_Permit.Contains(searchValue)
                );
            }

            var recordsTotal = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.Doc_Number)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                draw = draw,
                recordsFiltered = recordsTotal,
                recordsTotal = recordsTotal,
                data = data
            });
        }

        [HttpPost("QualityInspectionApi/SaveInspection")]
        public async Task<IActionResult> SaveInspection([FromBody] DMSFILogModel model)
        {
            if (model == null)
            {
                return BadRequest("Data tidak valid");
                
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new {
                        Field = x.Key,
                        Errors = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                    });

                return BadRequest(errors);
            }


            //if (await _context.DMSFILogModel.AnyAsync(x => x.Order_Number == model.Order_Number && x.Status == "Open"))
            //{
            //    return BadRequest("Order number with status OPEN already exists.");
            //}

            var maxDoc = await _context.DMSFILogModel.MaxAsync(x => (int?)x.Doc_Number) ?? 10000000;
            var newDocNumber = maxDoc + 1;

            var entity = new DMSFILogModel
            {
                Doc_Number = newDocNumber,
                Order_Number = model.Order_Number,
                Part_Number = model.Part_Number,
                Description = model.Description,
                Operation = model.Operation,
                Batch_Number = model.Batch_Number,
                Qty = model.Qty,
                Cell = model.Cell,
                Concession = model.Concession,
                Production_Permit = model.Production_Permit,
                Start_Date = model.Start_Date,
                Start_Time = model.Start_Time,
                Name = Environment.UserName?? "N/A",
                Status = "Open",

                // tambahkan default values untuk field lain:
                Drawing_Rev = "",
                End_Date = null,
                End_Time = null,
                Labor = 0,
                Week = "",
                EID = "",
                Std_Price = 0,
                QN = "",
                KFR = "",
                Serial_Number = "",
                Remarks = ""
            };

            _context.DMSFILogModel.Add(entity);
            await _context.SaveChangesAsync();

            return Json(entity);
        }


        [HttpPut("QualityInspectionApi/SaveFinish/{id}")]
        public async Task<IActionResult> SaveFinish(int id, [FromBody] DMSFILogModel model)
        {
            var data = await _context.DMSFILogModel.FindAsync(id);

            if (data == null) return NotFound();

            if (model == null)
            {
                return BadRequest("Model null. Periksa kembali nama properti JSON.");
            }

            data.Doc_Number = model.Doc_Number;
            data.Order_Number = model.Order_Number;
            data.Part_Number = model.Part_Number;
            data.Description = model.Description;
            data.Drawing_Rev = model.Drawing_Rev;

            data.Batch_Number = model.Batch_Number;
            data.Qty = model.Qty;
            data.Acc = model.Acc;
            data.Rejected = model.Rejected;
            data.Job_Type = model.Job_Type;

            data.Operation = model.Operation;
            data.Start_Date = model.Start_Date;
            data.Start_Time = model.Start_Time;
            data.End_Date = model.End_Date;
            data.End_Time = model.End_Time;

            data.Labor = model.Labor;
            data.Week = model.Week;
            data.EID = model.EID;
            data.Name = Environment.UserName;
            data.Std_Price = model.Std_Price;

            data.Cell = model.Cell;
            data.QN = model.QN;
            data.KFR = model.KFR;
            data.Concession = model.Concession;
            data.Production_Permit = model.Production_Permit;

            data.Serial_Number = model.Serial_Number;
            data.Remarks = model.Remarks;
            data.Status = model.Status;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Data berhasil diupdate", data });
        }

        //[HttpGet("QualityInspectionApi/GetValidTillByPart/{permitNumber}")]
        //public async Task<IActionResult> GetValidTillByPart(string permitNumber)
        //{
        //    if (string.IsNullOrWhiteSpace(permitNumber))
        //        return BadRequest("Permit number is required.");

        //    var normalizedPermit = permitNumber.Trim().ToLower();

        //    // JOIN DMSFILOG dan DMSProductionPermit berdasarkan Part_Number
        //    var result = await (
        //        from filog in _context.DMSFILogModel
        //        join permit in _context.DMSProductionPermit
        //            on filog.Part_Number equals permit.Part_Number
        //        where (filog.Production_Permit ?? "").Trim().ToLower() == normalizedPermit
        //        select new
        //        {
        //            permit.End_Date,
        //            permit.Remaining_Qty
        //        }
        //    ).ToListAsync();

        //    if (result == null)
        //        return NotFound($"Data untuk permit '{permitNumber}' tidak ditemukan.");

        //    return Ok(result);
        //}

        [HttpGet("QualityInspectionApi/GetValidTillByPart/{partNumber}")]
        public async Task<IActionResult> GetValidTillByPart(string partNumber)
        {
            if (string.IsNullOrWhiteSpace(partNumber))
                return BadRequest("Part number is required.");

            var normalizedPart = partNumber.Trim().ToLower();

            var result = await _context.DMSProductionPermit
                .Where(x =>
                      (x.Part_Number ?? "").Trim().ToLower() == normalizedPart &&
                      (x.Status ?? "").Trim().ToLower() == "active"
                )
                .Select(x => new
                {
                    x.Production_Permit,
                    x.End_Date,
                    x.Remaining_Qty
                })
                .ToListAsync();

            if (result == null || result.Count == 0)
                return NotFound($"Data untuk part number '{partNumber}' tidak ditemukan.");

            return Ok(result);
        }

        [HttpGet("QualityInspectionApi/GetAlertByPart/{partNumber}")]
        public async Task<IActionResult> GetAlertByPart(string partNumber)
        {
            if (string.IsNullOrWhiteSpace(partNumber))
                return BadRequest("Part number is required.");

            var normalizedPart = partNumber.Trim().ToLower();

            var result = await (
                from log in _context.DMSFILogModel

                join kfc in _context.DMSFIKFCModels
                    on log.Part_Number.Trim().ToLower() equals kfc.Part_Number.Trim().ToLower() into kfcGroup
                from kfc in kfcGroup.DefaultIfEmpty()

                join material in _context.DMSMaterialMasterModels
                    on log.Part_Number.Trim().ToLower() equals material.Part_Number.Trim().ToLower() into materialGroup
                from material in materialGroup.DefaultIfEmpty()

                join routing in _context.DMSSRoutingModels
                    on log.Part_Number.Trim().ToLower() equals routing.Part_Number.Trim().ToLower() into routingGroup
                from routing in routingGroup.DefaultIfEmpty()

                where log.Part_Number.Trim().ToLower() == normalizedPart

                select new
                {
                    // Dari DMSFILogModel
                    isConcession = log.Concession,
                    isProduction = log.Production_Permit,

                    // Dari DMSFIKFCModels
                    IsKFRActive = kfc != null && (kfc.Status ?? "").Trim().ToLower() == "active",

                    // Dari DMSMaterialMasterModels
                    IsTagLabel = material != null ? material.FI_Tag : "N/A",

                    // Dari DMSSRoutingModels
                    Routing_Operation = routing != null ? routing.Operation : null
                }
            ).ToListAsync();

            if (result == null || result.Count == 0)
                return NotFound($"Data untuk part number '{partNumber}' tidak ditemukan.");

            return Ok(result);
        }

        [HttpDelete("QualityInspectionApi/DeleteData/{id}")]
        public async Task<IActionResult> DeleteData(int id)
        {
            var data = await _context.DMSFILogModel.FindAsync(id);
            if (data == null) return NotFound();

            _context.DMSFILogModel.Remove(data);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Data berhasil dihapus" });
        }




    }



}
