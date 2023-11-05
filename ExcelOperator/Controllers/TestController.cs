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
        private FileReader _reader;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _reader = new FileReader();
        }

        [HttpGet(Name = "Test")]
        public async Task Get()
        {
            var path = "C:\\Users\\chris\\Downloads\\Livre.xlsx";
            var reader = _reader.ReadHeader(path, 1);   
            await _reader.ReadData(path,2,50000, Callback);   
        }

        private async Task Callback(List<List<string>> rows )
        {
            Console.WriteLine($"Inside the callback method. Received rows: {rows.Count}");
        }
    }
}