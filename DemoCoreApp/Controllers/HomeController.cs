using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DemoCoreApp.Data;
using DemoCoreApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;


namespace CoreDemoCRUD.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private DataContext _context;
        public HomeController(DataContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }


        public IActionResult Index() => View();
        public IActionResult Respond() => View();

        [HttpPost]
        public IActionResult Respond(GuestResponse response)
        {
            try
            {
                _context.Responses.Add(response);
                _context.SaveChanges();
                return RedirectToAction(nameof(Thanks), new { Name = response.Name, WillAttend = response.WillAttend });
            }

            catch
            {
                return View("Error");
            }

        }
        public IActionResult Thanks(GuestResponse response) => View(response);
        public IActionResult List() => View(_context.Responses.OrderByDescending(r => r.WillAttend));
        
        /// <summary>
        /// Actio ExportExcel is called for exporting tabe to Excel format
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ExportExcel()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string format = "dd-M-yyyy-hh-mm-ss";
            string eFileName = String.Format("Excel-Export-{0}.xlsx", DateTime.Now.ToString(format));
            string sFileName = eFileName;
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                int RowCount = 1;
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("List");
                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Name");
                row.CreateCell(1).SetCellValue("Email");
                row.CreateCell(2).SetCellValue("Phone");
                row.CreateCell(3).SetCellValue("Attend");
                var tabledata = _context.Responses.OrderByDescending(r => r.WillAttend);
                foreach (var Value in tabledata)
                {
                    row = excelSheet.CreateRow(RowCount);
                    row.CreateCell(0).SetCellValue(Value.Name);
                    row.CreateCell(1).SetCellValue(Value.Email);
                    row.CreateCell(2).SetCellValue(Value.Phone);
                    row.CreateCell(3).SetCellValue(Value.WillAttend.ToString());
                    RowCount++;
                }
                workbook.Write(fs);
                using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                try
                {
                    return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
                }

                finally
                {
                    var path = Path.Combine(sWebRootFolder, sFileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                }
                }
            }
        }
      
        public IActionResult Error()
        {
         return View("Error");  
        }
    }
}
