using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaWeb.Database;
using MediaWeb.Domain.Movie;
using MediaWeb.Domain.Music;
using MediaWeb.Models.Music;
using MediaWeb.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MediaWeb.Controllers
{
    public class MusicController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly MediaWebDbContext _mediaWebDbContext;

        public MusicController(IWebHostEnvironment hostingEnvironment, MediaWebDbContext mediaWebDbContext)
        {
            _hostingEnvironment = hostingEnvironment;
            _mediaWebDbContext = mediaWebDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Create()
        {
            MusicCreateViewModel createModel = new MusicCreateViewModel();

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
        public async Task<IActionResult> Create(MusicCreateViewModel createModel)
        {
            Song newSong = new Song()
            {
                Title = StringEdits.FirstLettterToUpper(createModel.Title),
                ReleaseDate = createModel.ReleaseDate,                
                SongFile = UploadUtility.UploadFile(createModel.Song, "songs", _hostingEnvironment)
            };

            //Add genres to song
            var songGenres = new List<MusicGenre>();

            //Select from existing genres
            if (createModel.SelectedGenres != null)
            {
                foreach (var selectedGenre in createModel.SelectedGenres)
                {
                    songGenres.Add(new MusicGenre() { GenreId = selectedGenre });
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
                        songGenres.Add(new MusicGenre() { Genre = genresFromDb.Find(g => g.Name == createdGenre) });
                    }
                }

                foreach (var newGenre in newGenres)
                {
                    songGenres.Add(new MusicGenre() { GenreId = newGenre.Id, Genre = newGenre });
                }
            }

            newSong.MusicGenres = songGenres;

            //add artists to song
            var songArtists = new List<SongArtist>();

            //create only if new artists
            if (createModel.createdArtists != null)
            {
                var createdArtists = StringEdits.FirstLettterToUpper(createModel.createdArtists);
                var createdArtistsArray = createdArtists.Split(", ");

                var newArtists = new List<Artist>();
                var artistsFromDb = await _mediaWebDbContext.Artists.ToListAsync();

                foreach (var createdArtist in createdArtistsArray)
                {
                    if (!artistsFromDb.Select(g => g.Name).Contains(createdArtist))
                    {
                        newArtists.Add(new Artist() { Name = createdArtist });
                    }
                    else
                    {
                        songArtists.Add(new SongArtist() { Artist = artistsFromDb.Find(g => g.Name == createdArtist) });
                    }
                }

                foreach (var newArtist in newArtists)
                {
                    songArtists.Add(new SongArtist() { ArtistId = newArtist.Id, Artist = newArtist });
                }
            }           

            _mediaWebDbContext.Update(newSong);
            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> List()
        {
            IEnumerable<Song> songsFromDb = await _mediaWebDbContext.Songs.ToListAsync();

            List<MusicListViewModel> songs = new List<MusicListViewModel>();

            foreach (var song in songsFromDb)
            {
                songs.Add(new MusicListViewModel() { Id = song.Id, Title = song.Title });
            }

            return View(songs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Song songFromDb =
                await _mediaWebDbContext.Songs
                .Include(song => song.MusicGenres)
                .ThenInclude(musicGenre => musicGenre.Genre)
                .FirstOrDefaultAsync(s => s.Id == id);

            MusicDetailViewModel detailModel = new MusicDetailViewModel()
            {
                Id = songFromDb.Id,
                Title = songFromDb.Title,
                ReleaseDate = songFromDb.ReleaseDate,                
                SongFile = songFromDb.SongFile,
                MusicGenres = songFromDb.MusicGenres.Select(musicGenre => musicGenre.Genre.Name)
            };

            return View(detailModel);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Song songFromDb = await _mediaWebDbContext.Songs.FirstOrDefaultAsync(s => s.Id == id);

            MusicEditViewModel editModel = new MusicEditViewModel()
            {
                Title = songFromDb.Title,
                ReleaseDate = songFromDb.ReleaseDate                
            };

            return View(editModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, MusicEditViewModel editModel)
        {
            Song songFromDb = await _mediaWebDbContext.Songs.FirstOrDefaultAsync(s => s.Id == id);

            songFromDb.Title = editModel.Title;
            songFromDb.ReleaseDate = editModel.ReleaseDate;            
            songFromDb.SongFile = UploadUtility.UploadFile(editModel.Song, "pics", _hostingEnvironment);

            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            Song songFromDb = await _mediaWebDbContext.Songs.FirstOrDefaultAsync(s => s.Id == id);

            MusicDeleteViewModel deleteModel = new MusicDeleteViewModel()
            {
                Id = songFromDb.Id,
                Title = songFromDb.Title
            };

            return View(deleteModel);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Song songFromDb = await _mediaWebDbContext.Songs.FirstOrDefaultAsync(s => s.Id == id);
            _mediaWebDbContext.Songs.Remove(songFromDb);
            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
