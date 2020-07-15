using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediaWeb.Database;
using MediaWeb.Domain;
using MediaWeb.Domain.Movie;
using MediaWeb.Domain.Music;
using MediaWeb.Domain.Podcast;
using MediaWeb.Domain.TVShow;
using MediaWeb.Models.User;
using MediaWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace MediaWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly MediaWebDbContext _mediaWebDbContext;
        private readonly UserManager<MediaWebUser> _userManager;

        public UserController(MediaWebDbContext mediaWebDbContext, UserManager<MediaWebUser> userManager)
        {
            _mediaWebDbContext = mediaWebDbContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {            
            return View();
        }

        public IActionResult MovieReview()
        {
            return View();
        }
        public IActionResult TVShowReview()
        {
            return View();
        }
        public IActionResult SongReview()
        {
            return View();
        }
        public IActionResult PodcastReview()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MovieReview(int id, ReviewCreateViewModel reviewCreate)
        {
            Movie movieFromDb = _mediaWebDbContext.Movies.Find(id);

            Review newReview = new Review()
            {
                Title = reviewCreate.Title,
                Content = reviewCreate.Content,
                IsChecked = false
            };

            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var movieReviews = new List<MovieReview>();
            user.Reviews.Add(newReview);

            movieReviews.Add(new MovieReview() { Review = newReview });
            movieFromDb.MovieReviews = movieReviews;

            _mediaWebDbContext.Reviews.Add(newReview);

            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> TVShowReview(int id, ReviewCreateViewModel reviewCreate)
        {
            TVshow tvshowFromDb = _mediaWebDbContext.TVShows.Find(id);

            Review newReview = new Review()
            {
                Title = reviewCreate.Title,
                Content = reviewCreate.Content,
                IsChecked = false
            };

            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var tvshowReviews = new List<TVShowReview>();
            user.Reviews.Add(newReview);

            tvshowReviews.Add(new TVShowReview() { Review = newReview });
            tvshowFromDb.TVShowReviews = tvshowReviews;

            _mediaWebDbContext.Reviews.Add(newReview);

            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SongReview(int id, ReviewCreateViewModel reviewCreate)
        {
            Song songFromDb = _mediaWebDbContext.Songs.Find(id);

            Review newReview = new Review()
            {
                Title = reviewCreate.Title,
                Content = reviewCreate.Content,
                IsChecked = false
            };

            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var songReviews = new List<MusicReview>();
            user.Reviews.Add(newReview);

            songReviews.Add(new MusicReview() { Review = newReview });
            songFromDb.SongReviews = songReviews;

            _mediaWebDbContext.Reviews.Add(newReview);

            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PodcastReview(int id, ReviewCreateViewModel reviewCreate)
        {
            PodCast podcastFromDb = _mediaWebDbContext.PodCasts.Find(id);

            Review newReview = new Review()
            {
                Title = reviewCreate.Title,
                Content = reviewCreate.Content,
                IsChecked = false
            };

            var user = _userManager.GetUserAsync(HttpContext.User).Result;
            var podcastReviews = new List<PodcastReview>();
            user.Reviews.Add(newReview);

            podcastReviews.Add(new PodcastReview() { Review = newReview });
            podcastFromDb.PodcastReviews = podcastReviews;

            _mediaWebDbContext.Reviews.Add(newReview);

            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            var usersFromDb = await _mediaWebDbContext.MediaWebUsers.ToListAsync();

            UserShowViewModel showUsers = new UserShowViewModel()
            {
                Users = usersFromDb
            };

            return View(showUsers);
        }
        public async Task<IActionResult> Reviews()
        {
            ReviewShowViewModel reviews = new ReviewShowViewModel()
            {
                Reviews = await _mediaWebDbContext.Reviews.Where(r => r.IsChecked == false).ToListAsync()
            };

            return View(reviews);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CheckReview(int id)
        {
            Review reviewFromDb = _mediaWebDbContext.Reviews.Find(id);
            reviewFromDb.IsChecked = true;
            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteReview(int id)
        {
            _mediaWebDbContext.Reviews.Remove(_mediaWebDbContext.Reviews.Find(id));
            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AddUserRole(string id)
        {
            var user = await _mediaWebDbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

            await _userManager.AddToRoleAsync(user, "User");
            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AddAdminRole(string id)
        {
            var user = await _mediaWebDbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

            await _userManager.AddToRoleAsync(user, "Admin");
            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
