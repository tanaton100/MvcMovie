using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieMvc.Data;
using MovieMvc.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MovieMvc.Controllers
{
    public class MovieController : Controller
    {
        private readonly AppDb db;

        public MovieController(AppDb db) => this.db = db;
        public async Task<IActionResult> Index()
        {
            var movies = await db.Movies.ToListAsync();
            return View(movies);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Movies model, IFormFile fileUpload)
        {
            if (model.Duration < 1)
            {
                ModelState.AddModelError("errDuration", "The duration field is required.");
                return View();
            }

            if (fileUpload == null)
            {
                ModelState.AddModelError("errFileUpload", "The file upload field is required.");
                return View();
            }

            if (ModelState.IsValid)
            {
                string pathImgMovie = "/images/movie/";
                string pathSave = $"wwwroot{pathImgMovie}";
                if (!Directory.Exists(pathSave))
                {
                    Directory.CreateDirectory(pathSave);
                }
                string extFile = Path.GetExtension(fileUpload.FileName);
                string fileName = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + extFile;
                var path = Path.Combine(Directory.GetCurrentDirectory(), pathSave, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await fileUpload.CopyToAsync(stream);
                }

                DateTime dateNow = DateTime.Now;
                model.CoverImg = pathImgMovie + fileName;
                model.CreateDate = dateNow;
                model.ModifyDate = dateNow;
                db.Movies.Add(model);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await db.Movies.FindAsync(id);
            db.Movies.Remove(movie);
           await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var movie = await db.Movies.FindAsync(id);
            return View(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Movies model, IFormFile fileUpload)
        {
            var oldMovie = await db.Movies.FindAsync(model.Id);
            oldMovie.Duration = model.Duration;
            oldMovie.Genre = model.Genre;
            oldMovie.ReleaseDate = model.ReleaseDate;
            if (ModelState.IsValid)
            {
                if (fileUpload != null)
                {
                    string pathImgMovie = "/images/movie/";
                    string pathSave = $"wwwroot{pathImgMovie}";
                    if (!Directory.Exists(pathSave))
                    {
                        Directory.CreateDirectory(pathSave);
                    }
                    string extFile = Path.GetExtension(fileUpload.FileName);
                    string fileName = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + extFile;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), pathSave, fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await fileUpload.CopyToAsync(stream);
                    }
                    oldMovie.CoverImg = pathImgMovie + fileName;
                }

                oldMovie.ModifyDate = DateTime.Now;
                db.Movies.Update(oldMovie);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
