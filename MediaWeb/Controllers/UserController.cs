using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediaWeb.Database;
using MediaWeb.Domain;
using MediaWeb.Domain.Movie;
using MediaWeb.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index()
        {
            UserIndexViewModel indexView = new UserIndexViewModel();
            indexView.UserName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userManager.GetUserAsync(HttpContext.User).Result;

            if (User.IsInRole("Admin"))
            {
                indexView.Movies = await _mediaWebDbContext.Movies.Select(m => m.Title).ToListAsync();
                indexView.Reviews = user.Reviews;
            }

            return View(indexView);
        }
        
        public IActionResult Review()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Review(int id, ReviewCreateViewModel reviewCreate)
        {
            Movie movieFromDb = _mediaWebDbContext.Movies.Find(id);            

            var movieReviews = new List<MovieReview>();

            Review newReview = new Review()
            {
                Title = reviewCreate.Title,
                Content = reviewCreate.Content,                
            };

            var user = _userManager.GetUserAsync(HttpContext.User).Result;            

            user.Reviews.Add(newReview);          

            movieReviews.Add(new MovieReview { ReviewId = newReview.Id, Review = newReview });

            movieFromDb.MovieReviews = movieReviews;            

            _mediaWebDbContext.Reviews.Add(newReview);  
            
            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
