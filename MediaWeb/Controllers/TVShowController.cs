using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaWeb.Database;
using MediaWeb.Domain.Movie;
using MediaWeb.Domain.TVShow;
using MediaWeb.Models.TVShow;
using MediaWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MediaWeb.Controllers
{
    public class TVShowController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly MediaWebDbContext _mediaWebDbContext;
        private readonly UploadUtility _uploadUtility;

        public TVShowController(IWebHostEnvironment hostingEnvironment, MediaWebDbContext mediaWebDbContext)
        {
            _hostingEnvironment = hostingEnvironment;
            _mediaWebDbContext = mediaWebDbContext;
            _uploadUtility = new UploadUtility();
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="User")]
        public async Task<IActionResult> Create()
        {
            TVShowCreateViewModel createModel = new TVShowCreateViewModel();

            var genres = await _mediaWebDbContext.Genres.Select(genre => new SelectListItem()
            {
                Value = genre.Id.ToString(),
                Text = genre.Name
            }).ToListAsync();

            createModel.Genres = genres;

            return View(createModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TVShowCreateViewModel createModel)
        {
            List<string> tvshowTitlesFromDb = await _mediaWebDbContext.TVShows.Select(tvs => tvs.Name).ToListAsync();

            if (tvshowTitlesFromDb.Contains(StringEdits.FirstLettterToUpper(createModel.Name)))
            {
                return RedirectToAction("Index");
            }

            TVshow newTvshow = new TVshow()
            {
                Name = StringEdits.FirstLettterToUpper(createModel.Name),
                ReleaseDate = createModel.ReleaseDate,
                Summary = createModel.Summary,
                Picture = UploadUtility.UploadFile(createModel.Picture, "tvshows", _hostingEnvironment)
            };

            //Add genres to tvshow
            var tvshowGenres = new List<TVShowGenre>();

            //Select from existing genres
            if (createModel.SelectedGenres != null)
            {
                foreach (var selectedGenre in createModel.SelectedGenres)
                {
                    tvshowGenres.Add(new TVShowGenre() { GenreId = selectedGenre });
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
                        tvshowGenres.Add(new TVShowGenre() { Genre = genresFromDb.Find(g => g.Name == createdGenre) });
                    }
                }

                foreach (var newGenre in newGenres)
                {
                    tvshowGenres.Add(new TVShowGenre() {Genre = newGenre });
                }
            }

            newTvshow.TVShowGenres = tvshowGenres;

            //add actors to tvshow
            var tvshowActors = new List<TVShowActor>();

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
                        tvshowActors.Add(new TVShowActor() { Actor = actorsFromDb.Find(g => g.Name == createdActor) });
                    }
                }

                foreach (var newActor in newActors)
                {
                    tvshowActors.Add(new TVShowActor() { Actor = newActor });
                }
            }

            newTvshow.TVShowActors = tvshowActors;

            //add directors to tvshow
            var tvshowDirectors = new List<TVShowDirector>();

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
                        tvshowDirectors.Add(new TVShowDirector() { Director = directorsFromDb.Find(g => g.Name == createdDirector) });
                    }
                }

                foreach (var newDirector in newDirectors)
                {
                    tvshowDirectors.Add(new TVShowDirector() {Director = newDirector });
                }
            }

            newTvshow.TVShowDirectors = tvshowDirectors;

            _mediaWebDbContext.Update(newTvshow);
            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> List()
        {
            IEnumerable<TVshow> tvshowsFromDb = await _mediaWebDbContext.TVShows.ToListAsync();

            List<TVShowListViewModel> tvshows = new List<TVShowListViewModel>();

            foreach (var tvshow in tvshowsFromDb)
            {
                tvshows.Add(new TVShowListViewModel() { Id = tvshow.Id, Name = tvshow.Name });
            }

            return View(tvshows);
        }
        public async Task<IActionResult> Detail(int id)
        {
            TVshow tvshowFromDb =
                await _mediaWebDbContext.TVShows
                .Include(tvshow =>tvshow.Episodes)
                .Include(tvshow => tvshow.TVShowGenres)
                .ThenInclude(tvshowGenre => tvshowGenre.Genre)
                .Include(tvshow => tvshow.TVShowDirectors)
                .ThenInclude(TVShowDirector => TVShowDirector.Director)
                .Include(tvshow => tvshow.TVShowActors)
                .ThenInclude(TVShowActor => TVShowActor.Actor)
                .Include(tvshow => tvshow.TVShowReviews)
                .ThenInclude(tvshowReview => tvshowReview.Review)
                .ThenInclude(review => review.MediaWebUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            TVShowDetailViewModel detailModel = new TVShowDetailViewModel()
            {
                Id = tvshowFromDb.Id,
                Name = tvshowFromDb.Name,
                ReleaseDate = tvshowFromDb.ReleaseDate,
                Summary = tvshowFromDb.Summary,
                Picture = tvshowFromDb.Picture,
                TVShowGenres = tvshowFromDb.TVShowGenres,
                TVShowReviews = tvshowFromDb.TVShowReviews.Where(tr => tr.Review.IsChecked == true),
                TVShowDirectors = tvshowFromDb.TVShowDirectors,
                TVShowActors = tvshowFromDb.TVShowActors,
                Episodes = tvshowFromDb.Episodes.OrderBy(e => e.EpisodeNumber),
                IsHidden = tvshowFromDb.IsHidden,
                NumberOfSeaons = tvshowFromDb.Seasons
            };

            return View(detailModel);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            TVshow tvshowFromDb = await _mediaWebDbContext.TVShows.FirstOrDefaultAsync(m => m.Id == id);

            TVShowEditViewModel editModel = new TVShowEditViewModel()
            {
                Name = tvshowFromDb.Name,
                ReleaseDate = tvshowFromDb.ReleaseDate,
                Summary = tvshowFromDb.Summary,
                PictureFile = tvshowFromDb.Picture
            };

            return View(editModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, TVShowEditViewModel editModel)
        {
            TVshow tvshowFromDB = await _mediaWebDbContext.TVShows.FirstOrDefaultAsync(m => m.Id == id);
            List<string> tvshowTitlesFromDb = await _mediaWebDbContext.TVShows.Where(tv => tv != tvshowFromDB).Select(tvs => tvs.Name).ToListAsync();

            if (tvshowTitlesFromDb.Contains(StringEdits.FirstLettterToUpper(editModel.Name)))
            {
                return RedirectToAction("Index");
            }

            tvshowFromDB.Name = editModel.Name;
            tvshowFromDB.ReleaseDate = editModel.ReleaseDate;
            tvshowFromDB.Summary = editModel.Summary;

            if (editModel.Picture != null)
            {
                tvshowFromDB.Picture = UploadUtility.UploadFile(editModel.Picture, "tvshows", _hostingEnvironment);
            }
            else
            {
                tvshowFromDB.Picture = editModel.PictureFile;
            }            

            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            TVshow tvshowFromDb = await _mediaWebDbContext.TVShows.FirstOrDefaultAsync(m => m.Id == id);

            TVShowDeleteViewModel deleteModel = new TVShowDeleteViewModel()
            {
                Id = tvshowFromDb.Id,
                Name = tvshowFromDb.Name
            };

            return View(deleteModel);
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            TVshow tvshowFromDB = await _mediaWebDbContext.TVShows.FirstOrDefaultAsync(m => m.Id == id);

            if (!string.IsNullOrEmpty(tvshowFromDB.Picture))
            {
                _uploadUtility.DeleteFile(_hostingEnvironment.WebRootPath, tvshowFromDB.Picture);
            }

            _mediaWebDbContext.TVShows.Remove(tvshowFromDB);
            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [Authorize(Roles ="User")]
        public IActionResult CreateEpisode()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateEpisode(EpisodeCreateViewModel createModel, int id)
        {
            TVshow tvshowFromDb = await _mediaWebDbContext.TVShows.FirstOrDefaultAsync(m => m.Id == id);
            Episode newEpisode = new Episode()
            {
                Name = StringEdits.FirstLettterToUpper(createModel.Name),                
                Summary = createModel.Summary,
                Season = createModel.Season,
                EpisodeNumber = createModel.EpisodeNumber
            };
            
            tvshowFromDb.Seasons = createModel.Season;
            tvshowFromDb.Episodes.Add(newEpisode);

            if (tvshowFromDb.Seasons != 0)
            {
                if (newEpisode.Season > tvshowFromDb.Seasons)
                {
                    tvshowFromDb.Seasons = newEpisode.Season;
                }
            }
            else
            {
                tvshowFromDb.Seasons = newEpisode.Season;
            }

            _mediaWebDbContext.Update(tvshowFromDb);
            _mediaWebDbContext.Update(newEpisode);

            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> HideTvshow(int id)
        {
            TVshow tvshowFromDb = await _mediaWebDbContext.TVShows.FirstOrDefaultAsync(m => m.Id == id);
            tvshowFromDb.IsHidden = true;

            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
