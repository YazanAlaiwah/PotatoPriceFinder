using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.RegularExpressions;


namespace PotatoPriceFinder.Server.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PotatoPriceFinderController : ControllerBase
    {

        string urlAddress = "http://eportal.aa-engineers.com/assessment/potatoQ12024.csv";
        static List<PotatoPriceFinder> dataList = [];

        private readonly ILogger<PotatoPriceFinderController> _logger;

        public PotatoPriceFinderController(ILogger<PotatoPriceFinderController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetPotatoPriceFinder")]
        public IEnumerable<PotatoPriceFinder> Get()
        {
            var poundsAvailableNeeded = Request.Query["pounds"];


            if (dataList.Count == 0)
            {
               
                WebClient wc = new WebClient();
                string data = wc.DownloadString(urlAddress);
                dataList = Regex.Split(data, "\r\n|\r|\n").Skip(1).ToArray().Where(s => !string.IsNullOrWhiteSpace(s)).Select(v =>
                {
                    string[] values = v.Split(',');
                    return new PotatoPriceFinder
                    {
                        Name = values[0],
                        Price = float.Parse(values[2]),
                        BagWeight = Int32.Parse(values[1]),
                        BagAvailable = Int32.Parse(values[3]),
                        PricePerPound = float.Parse(values[2]) / Int32.Parse(values[3])
                    };
                }).ToList();
            }

            List<PotatoPriceFinder> tempDataList = dataList;

            if (!string.IsNullOrEmpty(poundsAvailableNeeded)) {
          
                tempDataList = dataList.Where(v => (v.BagAvailable * v.BagWeight) >= Int32.Parse(poundsAvailableNeeded)).ToList();
            }

            return tempDataList.OrderBy(v => v.PricePerPound).Take(3);
        }
    }
}
