using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyCashIdentityProject.PresentationLayer.Controllers
{
    public class ExchangeController : Controller
    {
        private readonly HttpClient _client = new HttpClient();
        private const string ApiKey = "630ce9cc86msh271c60cffe62d5ep1b514djsn0fe292593744";
        private const string ApiHost = "currency-exchange.p.rapidapi.com";

        public async Task<IActionResult> Index()
        {
            ViewBag.UsdToTry = await GetExchangeRate("USD", "TRY");
            ViewBag.EurToTry = await GetExchangeRate("EUR", "TRY");
            ViewBag.GbpToTry = await GetExchangeRate("GBP", "TRY");
            ViewBag.UsdToEur = await GetExchangeRate("USD", "EUR");

            return View();
        }

        private async Task<string> GetExchangeRate(string fromCurrency, string toCurrency)
        {
            var requestUri = $"https://currency-exchange.p.rapidapi.com/exchange?from={fromCurrency}&to={toCurrency}&q=1.0";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri),
                Headers =
        {
            { "X-RapidAPI-Key", ApiKey },
            { "X-RapidAPI-Host", ApiHost },
        },
            };

            using (var response = await _client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                if (decimal.TryParse(body, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal rate))
                {
                    return rate.ToString("0.0000", CultureInfo.InvariantCulture);
                }
                else
                {
                    // Sonucun geçerli bir sayı olmadığı durumda hata işleyişiniz ne olursa olsun, burada bir şeyler yapmalısınız.
                    return "0.0000";
                }
            }
        }

    }
}
