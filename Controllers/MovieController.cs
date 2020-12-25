using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieMvc.Data;
using MovieMvc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

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
                string fileName = $"{DateTime.Now:dd-MM-yyyy}-{fileUpload.FileName}";
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
            var pathPic = $"wwwroot{movie.CoverImg}";
            FileInfo file = new FileInfo(pathPic);
            if (file.Exists)
            {
                file.Delete();
            }
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
                    if (!oldMovie.CoverImg.Contains(fileUpload.FileName))
                    {
                        string pathImgMovie = "/images/movie/";
                        string pathSave = $"wwwroot{pathImgMovie}";
                        if (!Directory.Exists(pathSave))
                        {
                            Directory.CreateDirectory(pathSave);
                        }
                        string fileName = $"{DateTime.Now:dd-MM-yyyy}-{fileUpload.FileName}";
                        var path = Path.Combine(Directory.GetCurrentDirectory(), pathSave, fileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await fileUpload.CopyToAsync(stream);
                        }
                        oldMovie.CoverImg = pathImgMovie + fileName;
                    }

                }

                oldMovie.ModifyDate = DateTime.Now;
                db.Movies.Update(oldMovie);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        public List<Movies> MockData() 
        {
            var mockdata = new List<Movies>() {
                new Movies {Id= 1,Title="มังกรหยก",CoverImg="/images/movie/25-12-2563-10-53-55.jfif",ReleaseDate = DateTime.Now,Genre = "Drama",Duration=251 },
                new Movies {Id= 2,Title="harry potter and the philosopher's stone",CoverImg="/images/movie/25-12-2563-10-52-22.jfif",ReleaseDate = DateTime.Now,Genre = "Drama",Duration=251 }};

            return mockdata;
        }
    }
}
