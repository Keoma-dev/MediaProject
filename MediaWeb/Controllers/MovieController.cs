using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediaWeb.Database;
using MediaWeb.Domain.Movie;
using MediaWeb.Models.Movie;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MediaWeb.Controllers
{
    public class MovieController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly MediaWebDbContext _mediaWebDbContext;

        public MovieController(IWebHostEnvironment hostingEnvironment, MediaWebDbContext mediaWebDbContext)
        {
            _hostingEnvironment = hostingEnvironment;
            _mediaWebDbContext = mediaWebDbContext;
        }
        public  async Task<IActionResult> Index()
        {  
            return View();
        }
        public async Task<IActionResult> Create()
        {
            MovieCreateViewModel createModel = new MovieCreateViewModel();

            var genres = await _mediaWebDbContext.Genres.ToListAsync();

            if (genres != null)
            {
                foreach (var genre in genres)
                {
                    createModel.Genres.Add(new SelectListItem()
                    {
                        Value = genre.Id.ToString(),
                        Text = genre.Name
                    });
                }
            }

            return View(createModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(MovieCreateViewModel createModel)
        {
            Movie newMovie = new Movie()
            {
                Title = createModel.Title,
                ReleaseDate = createModel.ReleaseDate,
                Summary = createModel.Summary
            };

            if (createModel.Photo != null)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(createModel.Photo.FileName);
                var pathName = Path.Combine(_hostingEnvironment.WebRootPath, "pics");
                var fileNameWithPath = Path.Combine(pathName, uniqueFileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    createModel.Photo.CopyTo(stream);
                }

                newMovie.Photo = "/pics/" + uniqueFileName;
            }

            _mediaWebDbContext.Update(newMovie);

            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> List()
        {
            IEnumerable<Movie> moviesFromDb = await _mediaWebDbContext.Movies.ToListAsync();

            List<MovieListViewModel> movies = new List<MovieListViewModel>();

            foreach (var movie in moviesFromDb)
            {
                movies.Add(new MovieListViewModel() { Id = movie.Id, Title = movie.Title });
            }

            return View(movies);
        }

        public async Task<IActionResult> Detail(int id) 
        {
            Movie movieFromDb = await _mediaWebDbContext.Movies.FirstOrDefaultAsync(m => m.Id == id);

            MovieDetailViewModel detailModel = new MovieDetailViewModel()
            {
                Id = movieFromDb.Id,
                Title = movieFromDb.Title,
                ReleaseDate = movieFromDb.ReleaseDate,
                Summary = movieFromDb.Summary,
                Photo = movieFromDb.Photo
            };

            return View(detailModel);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Movie movieFromDb = await _mediaWebDbContext.Movies.FirstOrDefaultAsync(m => m.Id == id);

            MovieEditViewModel editModel = new MovieEditViewModel()
            {
                Title = movieFromDb.Title,
                ReleaseDate = movieFromDb.ReleaseDate,
                Summary = movieFromDb.Summary
            };

            return View(editModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, MovieEditViewModel editModel)
        {
            Movie movieFromDb = await _mediaWebDbContext.Movies.FirstOrDefaultAsync(m => m.Id == id);

            movieFromDb.Title = editModel.Title;
            movieFromDb.ReleaseDate = editModel.ReleaseDate;
            movieFromDb.Summary = editModel.Summary;

            if (editModel.Photo != null)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(editModel.Photo.FileName);
                var pathName = Path.Combine(_hostingEnvironment.WebRootPath, "pics");
                var fileNameWithPath = Path.Combine(pathName, uniqueFileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    editModel.Photo.CopyTo(stream);
                }

                movieFromDb.Photo = "/pics/" + uniqueFileName;
            }

            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int id)
        {
            Movie movieFromDb = await _mediaWebDbContext.Movies.FirstOrDefaultAsync(m => m.Id == id);

            MovieDeleteViewModel deleteModel = new MovieDeleteViewModel()
            {
                Id = movieFromDb.Id,
                Title = movieFromDb.Title
            };

            return View(deleteModel);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Movie movieFromDb = await _mediaWebDbContext.Movies.FirstOrDefaultAsync(m => m.Id == id);
            _mediaWebDbContext.Movies.Remove(movieFromDb);
            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
