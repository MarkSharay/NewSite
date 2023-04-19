using Microsoft.AspNetCore.Mvc;
using PRAS_Task.Data;

namespace PRAS_Task.Controllers
{
    public class NewsController : Controller
    {
        IdentityContext _context;
        public NewsController(IdentityContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
