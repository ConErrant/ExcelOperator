using ExcelOperator.Logic;
using Microsoft.AspNetCore.Mvc;
using XlsxHelper;

namespace ExcelOperator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private ExcelReader _excelreader;
        private CsvReader _csvreader;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _excelreader = new ExcelReader();
            _csvreader = new CsvReader();
        }

        [HttpGet(Name = "Test")]
        public async Task Get()
        {
            var excelPath = "C:\\Users\\chris\\Downloads\\Livre.xlsx";
            var csvPath = "C:\\Users\\chris\\Downloads\\organizations-2000000.csv";

            var excelHeader = _csvreader.ReadHeader(excelPath, 1);
            await _excelreader.ReadData(excelPath, 2, 50000, Callback);

            var reader = _csvreader.ReadHeader(csvPath, 1);
            await _csvreader.ReadData(csvPath, 2, 50000, Callback);
        }

        private async Task Callback(List<List<string>> rows )
        {
            Console.WriteLine($"Inside the callback method. Received rows: {rows.Count}");
        }
    }
}