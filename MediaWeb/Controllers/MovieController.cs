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

        public MovieController(IWebHostEnvironment hostingEnvironment, MediaWebDbContext mediaWebDbContext)
        {
            _hostingEnvironment = hostingEnvironment;
            _mediaWebDbContext = mediaWebDbContext;
        }
        public  IActionResult Index()
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
                Title = StringEdits.FirstLettterToUpper(createModel.Title),
                ReleaseDate = createModel.ReleaseDate,
                Summary = createModel.Summary,
                Photo = UploadUtility.UploadFile(createModel.Photo, "pics", _hostingEnvironment)                
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
                    movieGenres.Add(new MovieGenre() { GenreId = newGenre.Id, Genre = newGenre});
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
                    movieActors.Add(new MovieActor() { ActorId = newActor.Id, Actor = newActor });
                }
            }

            //add direcotsr to movie
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
                    movieDirectors.Add(new MovieDirector() { DirectorId = newDirector.Id, Director = newDirector });
                }
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
            Movie movieFromDb =
                await _mediaWebDbContext.Movies
                .Include(movie => movie.MovieGenres)
                .ThenInclude(movieGenre => movieGenre.Genre)
                .Include(movie => movie.MovieReviews)
                .ThenInclude(movieReview => movieReview.Review)
                .FirstOrDefaultAsync(m => m.Id == id);                

            MovieDetailViewModel detailModel = new MovieDetailViewModel()
            {
                Id = movieFromDb.Id,
                Title = movieFromDb.Title,
                ReleaseDate = movieFromDb.ReleaseDate,
                Summary = movieFromDb.Summary,
                Photo = movieFromDb.Photo,
                MovieGenres = movieFromDb.MovieGenres.Select(movieGenre => movieGenre.Genre.Name),
                MovieReviews = movieFromDb.MovieReviews
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
            movieFromDb.Photo = UploadUtility.UploadFile(editModel.Photo, "pics", _hostingEnvironment);            

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
