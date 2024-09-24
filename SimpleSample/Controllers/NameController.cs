using Microsoft.AspNetCore.Mvc;
using SimpleSample.Services;
using System.Threading.Tasks;

namespace SimpleSample.Controllers
{
    public class NameController : Controller
    {
        private readonly TableStorageService _tableStorageService;

        public NameController()
        {
            _tableStorageService = new TableStorageService();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Submit(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                await _tableStorageService.AddNameAsync(name);
                ViewBag.Message = "Name added successfully!";
            }
            return View("Index");
        }
    }
}
