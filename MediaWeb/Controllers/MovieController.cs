using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediaWeb.Database;
using MediaWeb.Domain.Movie;
using MediaWeb.Models.Movie;
using MediaWeb.Utility;
using Microsoft.AspNetCore.Authorization;
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
        private readonly UploadUtility _uploadUtility;

        public MovieController(IWebHostEnvironment hostingEnvironment, MediaWebDbContext mediaWebDbContext)
        {
            _hostingEnvironment = hostingEnvironment;
            _mediaWebDbContext = mediaWebDbContext;
            _uploadUtility = new UploadUtility();
        }
        public  IActionResult Index()
        {  
            return View();
        }
        [Authorize(Roles ="User")]
        public async Task<IActionResult> Create()
        {
            MovieCreateViewModel createModel = new MovieCreateViewModel();

            //create selectlist of existing gneres
            var genres = await _mediaWebDbContext.Genres.Select(genre => new SelectListItem()
                {
                     Value = genre.Id.ToString(),
                     Text = genre.Name
                }).ToListAsync();

            createModel.Genres = genres;              

            return View(createModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(MovieCreateViewModel createModel)
        {
            //check for duplicate
            List<string> movieTitlesFromDb = await _mediaWebDbContext.Movies.Select(m => m.Title).ToListAsync();
           
            if (movieTitlesFromDb.Contains(StringEdits.FirstLettterToUpper(createModel.Title)))
            {
                return RedirectToAction("Index");
            }

            //create if no duplicate
            Movie newMovie = new Movie()
            {
                Title = StringEdits.FirstLettterToUpper(createModel.Title),
                ReleaseDate = createModel.ReleaseDate,
                Summary = createModel.Summary,
                Photo = UploadUtility.UploadFile(createModel.Photo, "pics", _hostingEnvironment),
                IsHidden = false
            };

            //Add genres to movie
            var movieGenres = new List<MovieGenre>();

            //Select from existing genres
            if (createModel.SelectedGenres != null)
            {
                foreach (var selectedGenre in createModel.SelectedGenres)
                {
                    movieGenres.Add(new MovieGenre() { GenreId = selectedGenre });
                }
            }

            //create only if new genres
            if (createModel.createdGenres != null)
            {
                var createdGenres = StringEdits.FirstLettterToUpper(createModel.createdGenres);
                var createdGenresArray = createdGenres.Split(", ");

                var newGenres = new List<Genre>();
                var genresFromDb = await _mediaWebDbContext.Genres.ToListAsync();

                foreach (var createdGenre in createdGenresArray)
                {
                    if (!genresFromDb.Select(g => g.Name).Contains(createdGenre))
                    {
                        newGenres.Add(new Genre() { Name = createdGenre });
                    }
                    else
                    {
                        movieGenres.Add(new MovieGenre() { Genre = genresFromDb.Find(g => g.Name == createdGenre) });
                    }
                }

                foreach (var newGenre in newGenres)
                {
                    movieGenres.Add(new MovieGenre() {Genre = newGenre});
                }
            }            

            newMovie.MovieGenres = movieGenres;

            //add actors to movie
            var movieActors = new List<MovieActor>();

            //create only if new actors
            if (createModel.createdActors != null)
            {
                var createdActors = StringEdits.FirstLettterToUpper(createModel.createdActors);
                var createdActorsArray = createdActors.Split(", ");

                var newActors = new List<Actor>();
                var actorsFromDb = await _mediaWebDbContext.Actors.ToListAsync();

                foreach (var createdActor in createdActorsArray)
                {
                    //check for duplicates
                    if (!actorsFromDb.Select(g => g.Name).Contains(createdActor))
                    {
                        newActors.Add(new Actor() { Name = createdActor });
                    }
                    else
                    {
                        movieActors.Add(new MovieActor() { Actor = actorsFromDb.Find(g => g.Name == createdActor) });
                    }
                }

                foreach (var newActor in newActors)
                {
                    movieActors.Add(new MovieActor() {Actor = newActor });
                }
            }

            newMovie.MovieActors = movieActors;

            //add directors to movie
            var movieDirectors = new List<MovieDirector>();

            //create only if new directors
            if (createModel.createdDirectors != null)
            {
                var createdDirectors = StringEdits.FirstLettterToUpper(createModel.createdDirectors);
                var createdDirectorsArray = createdDirectors.Split(", ");

                var newDirectors = new List<Director>();
                var directorsFromDb = await _mediaWebDbContext.Directors.ToListAsync();

                foreach (var createdDirector in createdDirectorsArray)
                {
                    if (!directorsFromDb.Select(g => g.Name).Contains(createdDirector))
                    {
                        newDirectors.Add(new Director() { Name = createdDirector });
                    }
                    else
                    {
                        movieDirectors.Add(new MovieDirector() { Director = directorsFromDb.Find(g => g.Name == createdDirector) });
                    }
                }

                foreach (var newDirector in newDirectors)
                {
                    movieDirectors.Add(new MovieDirector() {Director = newDirector });
                }
            }

            newMovie.MovieDirectors = movieDirectors;

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
            Movie movieFromDb =
                await _mediaWebDbContext.Movies
                .Include(movie => movie.MovieGenres)
                .ThenInclude(movieGenre => movieGenre.Genre)
                .Include(movie => movie.MovieDirectors)
                .ThenInclude(movieDirector => movieDirector.Director)
                .Include(movie => movie.MovieActors)
                .ThenInclude(movieActor => movieActor.Actor)
                .Include(movie => movie.MovieReviews)
                .ThenInclude(movieReview => movieReview.Review)
                .ThenInclude(review => review.MediaWebUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            MovieDetailViewModel detailModel = new MovieDetailViewModel()
            {
                Id = movieFromDb.Id,
                Title = movieFromDb.Title,
                ReleaseDate = movieFromDb.ReleaseDate,
                Summary = movieFromDb.Summary,
                Photo = movieFromDb.Photo,
                MovieGenres = movieFromDb.MovieGenres,
                MovieReviews = movieFromDb.MovieReviews.Where(mr => mr.Review.IsChecked == true),
                MovieDirectors = movieFromDb.MovieDirectors,
                MovieActors = movieFromDb.MovieActors,
                IsHidden = movieFromDb.IsHidden
            };

            return View(detailModel);
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            Movie movieFromDb = await _mediaWebDbContext.Movies.FirstOrDefaultAsync(m => m.Id == id);

            MovieEditViewModel editModel = new MovieEditViewModel()
            {
                Title = movieFromDb.Title,
                ReleaseDate = movieFromDb.ReleaseDate,
                Summary = movieFromDb.Summary,
                PhotoUrl = movieFromDb.Photo
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
                movieFromDb.Photo = UploadUtility.UploadFile(editModel.Photo, "pics", _hostingEnvironment);
            }
            else
            {
                movieFromDb.Photo = editModel.PhotoUrl;
            }                     

            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [Authorize(Roles ="Admin")]
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

            //delete file aswell
            if (!string.IsNullOrEmpty(movieFromDb.Photo))
            {
                _uploadUtility.DeleteFile(_hostingEnvironment.WebRootPath, movieFromDb.Photo);
            }
            
            _mediaWebDbContext.Movies.Remove(movieFromDb);
            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> HideMovie(int id)
        {
            Movie movieFromDb = await _mediaWebDbContext.Movies.FirstOrDefaultAsync(m => m.Id == id);
            movieFromDb.IsHidden = true;

            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
