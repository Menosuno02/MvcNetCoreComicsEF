using Microsoft.AspNetCore.Mvc;
using MvcNetCoreComicsEF.Models;
using MvcNetCoreComicsEF.Repositories;

namespace MvcNetCoreComicsEF.Controllers
{
    public class ComicsController : Controller
    {
        private IRepositoryComics repo;

        public ComicsController(IRepositoryComics repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Comic> comics = await this.repo.GetComicsAsync();
            return View(comics);
        }

        public async Task<IActionResult> Details(int id)
        {
            Comic comic = await this.repo.FindComicAsync(id);
            if (comic == null)
            {
                ViewData["MENSAJE"] = "No existe comic con ID " + id;
            }
            return View(comic);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Comic comic)
        {
            await this.repo.CreateComicAsync(comic);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Comic comic = await this.repo.FindComicAsync(id);
            if (comic == null)
            {
                ViewData["MENSAJE"] = "No existe comic con ID " + id;
            }
            return View(comic);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            await this.repo.DeleteComicAsync(id.Value);
            return RedirectToAction("Index");
        }
    }
}
