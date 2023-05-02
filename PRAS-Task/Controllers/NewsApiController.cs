using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using PRAS_Task.Data;
using PRAS_Task.Data.Models;

namespace PRAS_Task.Controllers
{
    public class NewsApiController : ApiController
    {
        private readonly DataContext _context;
        IWebHostEnvironment _environment;

        public NewsApiController(DataContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<ActionResult<IEnumerable<New>>> GetAll()
        {
            var news = from News in _context.News select News;
            if (news != null) return await news.ToListAsync<New>();
            return BadRequest("DB entries not found");
            
        }
        [HttpGet]
        [Route(nameof(Get))]
        public ActionResult Get(int id)
        {
            var data = _context.News.Where(x => x.Id == id).FirstOrDefault();
            if (data != null) return Ok(data);
            return BadRequest("Not found");
        }

        [HttpGet]
        [Route(nameof(GetImage))]

        public PhysicalFileResult GetImage(string fileName)
        {
            var path = $"{_environment.WebRootPath}/images/{fileName}";
            return PhysicalFile(path, "image/jpeg", fileName);
        }
    }
}
