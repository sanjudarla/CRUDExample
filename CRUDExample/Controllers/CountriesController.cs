using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        private readonly ICountriesAdderService _countriesAdderService;
        private readonly ICountriesUploaderService _countriesUploaderService;

        public CountriesController(ICountriesAdderService countriesAdderService, ICountriesUploaderService countriesUploaderService)
        {
            _countriesAdderService = countriesAdderService;
            _countriesUploaderService = countriesUploaderService;
        }

        [Route("UploadFromExcel")]
        public IActionResult UploadFromExcel()
        {
            return View();
        }
        [HttpPost]
        [Route("UploadFromExcel")]
        public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ViewBag.ErrorMessage = "Please Select the xlsx file";
                return View();
            }
            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMessage = "Unsupported file,Please Select the xlsx file";
                return View();
            }
            int countofCountriesInserted = await _countriesUploaderService.UploadCountriesFromExcelFile(excelFile);
            ViewBag.Message = $"{countofCountriesInserted} countries upload";
            return View();
        }

    }
}
