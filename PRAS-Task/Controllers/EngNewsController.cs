using Microsoft.AspNetCore.Mvc;
using PRAS_Task.Data.Models;
using PRAS_Task.Data;
using Microsoft.AspNetCore.Authorization;

namespace PRAS_Task.Controllers
{
    public class EngNewsController : Controller
    {
        DataContext _context;
        IWebHostEnvironment _environment;
        public EngNewsController(DataContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            var news = _context.EngNews.ToList<EngNew>();
            return View("Index", news);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create(EngNew _new)
        {

            var uploadPath = $"{_environment.WebRootPath}/images";
            var files = Request.Form.Files;
            string filePath;
            if (files[0] != null)
            {
                filePath = @$"{uploadPath}/{files[0].FileName}";
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await files[0].CopyToAsync(fileStream);

                }
                _new.picture = files[0].FileName;
                _new.date = DateTime.Now;
                _context.EngNews.Add(_new);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return BadRequest("Error");

        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> CreateView()
        {
            return View("Create");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Delete(int Newid)
        {

            var data = _context.EngNews.Where(x => x.Id == Newid).FirstOrDefault();
            if (data != null)
            {
                _context.EngNews.Remove(data);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return BadRequest("Cant delete");
        }
        [Authorize]
        public ActionResult UpdateView(int id)
        {
            var _new = _context.EngNews.Where(x => x.Id == id).FirstOrDefault();
            if (_new != null)
            {
                return View("Update", _new);
            }
            return BadRequest("Not exist");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Update(EngNew model)
        {
            var data = _context.EngNews.Where(x => x.Id == model.Id).FirstOrDefault();

            if (data != null)
            {
                var files = Request.Form.Files;
                string filePath = "";
                string oldPath;
                if (files.Count > 0)
                {
                    oldPath = model.picture;

                    var uploadPath = Path.Combine($"{_environment.WebRootPath}", "images");
                    filePath = $"{uploadPath}/{files[0].FileName}";
                    if (oldPath != filePath)
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await files[0].CopyToAsync(fileStream);
                        }
                        model.picture = files[0].FileName;
                        data.picture = model.picture;
                    }

                }

                data.text = model.text;
                data.name = model.name;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return BadRequest("Error during processing data");
        }
    }
}

