using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaWeb.Database;
using MediaWeb.Domain;
using MediaWeb.Domain.Movie;
using MediaWeb.Domain.Music;
using MediaWeb.Models.Music;
using MediaWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MediaWeb.Controllers
{
    public class MusicController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly MediaWebDbContext _mediaWebDbContext;
        private readonly UploadUtility _uploadUtility;
        private readonly UserManager<MediaWebUser> _userManager;


        public MusicController(IWebHostEnvironment hostingEnvironment, MediaWebDbContext mediaWebDbContext, UserManager<MediaWebUser> userManager)
        {
            _hostingEnvironment = hostingEnvironment;
            _mediaWebDbContext = mediaWebDbContext;
            _uploadUtility = new UploadUtility();
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="User")]
        public IActionResult Create()
        {    
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MusicCreateViewModel createModel)
        {
            List<string> songsFromDb = await _mediaWebDbContext.Songs.Select(s => s.Title).ToListAsync();
            List<string> artistFromDb = await _mediaWebDbContext.Artists.Select(a => a.Name).ToListAsync();

            //check for duplicates
            if (songsFromDb.Contains(StringEdits.FirstLettterToUpper(createModel.Title)))
            {
                return RedirectToAction("Index");
            }

            Song newSong = new Song()
            {
                Title = StringEdits.FirstLettterToUpper(createModel.Title),
                ReleaseDate = createModel.ReleaseDate,                
                SongFile = UploadUtility.UploadFile(createModel.Song, "songs", _hostingEnvironment)
            };           

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

            newSong.SongArtists = songArtists;

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
                .Include(song => song.SongArtists)
                .ThenInclude(songArtist => songArtist.Artist)
                .Include(song => song.SongReviews)
                .ThenInclude(songReviews => songReviews.Review)
                .ThenInclude(review => review.MediaWebUser)
                .FirstOrDefaultAsync(s => s.Id == id);

            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            MusicDetailViewModel detailModel = new MusicDetailViewModel()
            {
                Id = songFromDb.Id,
                Title = songFromDb.Title,
                ReleaseDate = songFromDb.ReleaseDate,
                SongFile = songFromDb.SongFile,
                SongArtists = songFromDb.SongArtists,
                SongReviews = songFromDb.SongReviews.Where(sr => sr.Review.IsChecked == true),
                IsHidden = songFromDb.IsHidden,
                MyPlaylists = await _mediaWebDbContext.PlayLists.Where(pl => pl.MediaWebUser == user).ToListAsync()
            };

            return View(detailModel);
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            Song songFromDb = await _mediaWebDbContext.Songs.FirstOrDefaultAsync(s => s.Id == id);

            MusicEditViewModel editModel = new MusicEditViewModel()
            {
                Title = songFromDb.Title,
                ReleaseDate = songFromDb.ReleaseDate,
                SongFile = songFromDb.SongFile
            };

            return View(editModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, MusicEditViewModel editModel)
        {
            Song songFromDb = await _mediaWebDbContext.Songs.FirstOrDefaultAsync(s => s.Id == id);
            var tempFile = songFromDb.SongFile;

            List<string> songTitlesFromDb = await _mediaWebDbContext.Songs.Where(song => song != songFromDb).Select(s => s.Title).ToListAsync();

            //check for duplicates
            if (songTitlesFromDb.Contains(StringEdits.FirstLettterToUpper(editModel.Title)))
            {
                return RedirectToAction("Index");
            }

            songFromDb.Title = editModel.Title;
            songFromDb.ReleaseDate = editModel.ReleaseDate;

            var songArtists = new List<SongArtist>();

            if (editModel.createdArtists != null)
            {
                var createdArtists = StringEdits.FirstLettterToUpper(editModel.createdArtists);
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

            songFromDb.SongArtists = songArtists;

            if (editModel.Song != null)
            {
                songFromDb.SongFile = UploadUtility.UploadFile(editModel.Song, "songs", _hostingEnvironment);
            }
            else
            {
                songFromDb.SongFile = tempFile;
            }            

            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [Authorize(Roles ="Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Song songFromDb = await _mediaWebDbContext.Songs.FirstOrDefaultAsync(s => s.Id == id);

            if (!string.IsNullOrEmpty(songFromDb.SongFile))
            {
                _uploadUtility.DeleteFile(_hostingEnvironment.WebRootPath, songFromDb.SongFile);
            }

            _mediaWebDbContext.Songs.Remove(songFromDb);
            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> HideSong(int id)
        {
            Song songFromDb = await _mediaWebDbContext.Songs.FirstOrDefaultAsync(m => m.Id == id);
            songFromDb.IsHidden = true;

            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [Authorize]
        public async Task<IActionResult> CreatePlayList()
        {
            PlaylistCreateViewModel createModel = new PlaylistCreateViewModel();

            var songs = await _mediaWebDbContext.Songs.Select(song => new SelectListItem()
            {
                Value = song.Id.ToString(),
                Text = song.Title
            }).ToListAsync();

            createModel.Songs = songs;

            return View(createModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreatePlayList(PlaylistCreateViewModel createModel)
        {
            PlayList newPlayList = new PlayList()
            {
                Name = StringEdits.FirstLettterToUpper(createModel.Name),
                IsPrivate = createModel.IsPrivate
            };

            //Add songs to playlist
            var playlistSongs = new List<PlaylistSong>();

            //Select from existing songs
            if (createModel.SelectedSongs != null)
            {
                foreach (var selectedSong in createModel.SelectedSongs)
                {
                    playlistSongs.Add(new PlaylistSong() { SongId = selectedSong });
                }
            }

            newPlayList.PlaylistSongs = playlistSongs;

            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            user.PlayLists.Add(newPlayList);

            _mediaWebDbContext.Update(newPlayList);
            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> PlaylistIndex()
        {
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            PlaylistListsViewModel playlists = new PlaylistListsViewModel()
            {
                PrivateLists = await _mediaWebDbContext.PlayLists.Where(s => s.MediaWebUser == user).ToListAsync(),
                PublicLists = await _mediaWebDbContext.PlayLists.Where(s => s.MediaWebUser != user && s.IsPrivate == false).ToListAsync()
            };
            return View(playlists);
        }
        public async Task<IActionResult> ShowPlayList(int id)
        {
            PlayList playlistFromDb =
                await _mediaWebDbContext.PlayLists
                .Include(pl => pl.PlaylistSongs)
                .ThenInclude(plSongs => plSongs.Song)
                .ThenInclude(s => s.SongArtists)
                .ThenInclude(sa => sa.Artist)
                .FirstOrDefaultAsync(pl => pl.Id == id);

            PlaylistShowViewModel showModel = new PlaylistShowViewModel()
            {
                Id = playlistFromDb.Id,
                Name = playlistFromDb.Name,
                PlaylistSongs = playlistFromDb.PlaylistSongs,
                Playlistsongfiles = playlistFromDb.PlaylistSongs.Select(ps => ps.Song.SongFile).ToList()
            };

            return View(showModel);
        }
        public async Task<IActionResult> AddToPlaylist(int id, string playListName)
        {
            Song songFromDb = await _mediaWebDbContext.Songs.FirstOrDefaultAsync(s => s.Id == id);
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            PlayList playlistFromDb = await _mediaWebDbContext.PlayLists
                .Include(pl => pl.PlaylistSongs)
                .ThenInclude(ps => ps.Song)
                .FirstOrDefaultAsync(pl => pl.Name == playListName && pl.MediaWebUser == user);

            List<PlaylistSong> playlistSongs = new List<PlaylistSong>();

            foreach (var item in playlistFromDb.PlaylistSongs)
            {
                playlistSongs.Add(item);
            }

            playlistSongs.Add(new PlaylistSong { Song = songFromDb });

            playlistFromDb.PlaylistSongs = playlistSongs;

            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}

