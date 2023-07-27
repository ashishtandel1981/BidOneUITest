using BidOneUI.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BidOneUI.Controllers
{
 
    public class PersonController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration _configuration;

        public PersonController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {            
            HttpClientHandler httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _configuration = configuration;

            httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new System.Uri(_configuration["ApiSettings:ApiBaseAddress"]);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]        
        public async Task<IActionResult> Index(Person person)
        {
            if (!ModelState.IsValid)
            {
                return View(person);
            }
           
            var content = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(_configuration["ApiSettings:AddPersonEndpoint"], content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Person infomation created successfully!";
                return RedirectToAction("Index");
            }

            // Handle error 
            return View("Error");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {            
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var error = new ProblemDetails
            {
                Title = "An error occurred while processing your request.",
                Detail = exceptionHandlerPathFeature?.Error?.Message,
                Status = 500 
            };

            return View(error);
        }
    }
}
