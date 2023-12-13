using Microsoft.AspNetCore.Mvc;
using NzWalksWebUI.Models;
using NzWalksWebUI.Models.DTO;
using System.Text;
using System.Text.Json;

namespace NzWalksWebUI.Controllers
{
    public class RegionController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public RegionController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = new List<RegionDto>();
            try
            {
                //Get All The Regions Information
                var client = _clientFactory.CreateClient();
                var httpResponseMessage = await client
                    .GetAsync("https://localhost:7131/api/regions");
                httpResponseMessage.EnsureSuccessStatusCode();
                response = await httpResponseMessage.Content.ReadFromJsonAsync<List<RegionDto>>();
            }
            catch (Exception ex)
            {
                //Log Error Exception
                throw;
            }
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel regionViewModel)
        {
            var client = _clientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7131/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(regionViewModel), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();
            if (response is not null)
            {
                return RedirectToAction("Index", "Region");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7131/api/regions/{id}");
            if (response is not null)
            {
                return View(response);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto request)
        {
            var client = _clientFactory.CreateClient();
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7131/api/regions/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();
            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();
            if (response is not null)
            {
                return RedirectToAction("Edit", "Region");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegionDto request)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7131/api/regions/{request.Id}");
                httpResponseMessage.EnsureSuccessStatusCode();
                return RedirectToAction("index", "Region");
            }
            catch (Exception ex)
            {
                //Do something here...
            }
            return View("Edit");
        }
    }
}
