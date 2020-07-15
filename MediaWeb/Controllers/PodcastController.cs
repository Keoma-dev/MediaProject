using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaWeb.Database;
using MediaWeb.Domain;
using MediaWeb.Domain.Podcast;
using MediaWeb.Models.Podcast;
using MediaWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MediaWeb.Controllers
{
    public class PodcastController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly MediaWebDbContext _mediaWebDbContext;
        private readonly UploadUtility _uploadUtility;


        public PodcastController(IWebHostEnvironment hostingEnvironment, MediaWebDbContext mediaWebDbContext)
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
        public IActionResult Create()
        {     
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PodcastCreateViewModel createModel)
        {
            List<string> podcastTitlesFromDb = await _mediaWebDbContext.PodCasts.Select(p => p.Title).ToListAsync();

            if (podcastTitlesFromDb.Contains(StringEdits.FirstLettterToUpper(createModel.Title)))
            {
                return RedirectToAction("Index");
            }

            PodCast newPodcast = new PodCast()
        {
            Title = StringEdits.FirstLettterToUpper(createModel.Title),
            ReleaseDate = createModel.ReleaseDate,
            Description = createModel.Description,
            File = createModel.PodcastLink
        };

        //add guests to podcast
        var podcastGuests = new List<PodcastGuest>();

        //create only if new guests
        if (createModel.createdGuests != null)
            {
                var createdGuests = StringEdits.FirstLettterToUpper(createModel.createdGuests);
                var createdGuestsArray = createdGuests.Split(", ");

                var newGuests = new List<Guest>();
                var guestsFromDb = await _mediaWebDbContext.Guests.ToListAsync();

                foreach (var createdGuest in createdGuestsArray)
                {
                    if (!guestsFromDb.Select(g => g.Name).Contains(createdGuest))
                    {
                        newGuests.Add(new Guest() { Name = createdGuest });
                    }
                    else
                    {
                        podcastGuests.Add(new PodcastGuest() { Guest = guestsFromDb.Find(g => g.Name == createdGuest) });
                    }
                }

                foreach (var newGuest in newGuests)
                {
                    podcastGuests.Add(new PodcastGuest() { Guest = newGuest });
                }
            }

            newPodcast.PodCastGuests = podcastGuests;

        //add hosts to podcast
        var podcastHosts = new List<PodcastHost>();

        //create only if new hosts
        if (createModel.createdHosts != null)
            {
                var createdHosts = StringEdits.FirstLettterToUpper(createModel.createdHosts);
                var createdHostsArray = createdHosts.Split(", ");

                var newHosts = new List<Host>();
                var hostsFromDb = await _mediaWebDbContext.Hosts.ToListAsync();

                foreach (var createdHost in createdHostsArray)
                {
                    if (!hostsFromDb.Select(g => g.Name).Contains(createdHost))
                    {
                        newHosts.Add(new Host() { Name = createdHost });
                    }
                    else
                    {
                        podcastHosts.Add(new PodcastHost() { Host = hostsFromDb.Find(g => g.Name == createdHost) });
                    }
                }

                foreach (var newHost in newHosts)
                {
                    podcastHosts.Add(new PodcastHost() { Host = newHost });
                }
            }

            newPodcast.PodCastHosts = podcastHosts;

            _mediaWebDbContext.Update(newPodcast);
            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
       }
        public async Task<IActionResult> List()
        {
            IEnumerable<PodCast> podcastsFromDb = await _mediaWebDbContext.PodCasts.ToListAsync();

            List<PodcastListViewModel> podcasts = new List<PodcastListViewModel>();

            foreach (var podcast in podcastsFromDb)
            {
                podcasts.Add(new PodcastListViewModel() { Id = podcast.Id, Title = podcast.Title });
            }

            return View(podcasts);
        }
        public async Task<IActionResult> Detail(int id)
        {
            PodCast podcastFromDb =
                await _mediaWebDbContext.PodCasts
                .Include(podcast => podcast.PodCastHosts)
                .ThenInclude(podcastHost => podcastHost.Host)
                .Include(podcast => podcast.PodCastGuests)
                .ThenInclude(podcastGuest => podcastGuest.Guest)
                .Include(podcast => podcast.PodcastReviews)
                .ThenInclude(podcastReview => podcastReview.Review)
                .ThenInclude(review => review.MediaWebUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            PodcastDetailViewModel detailModel = new PodcastDetailViewModel()
            {
                Id = podcastFromDb.Id,
                Title = podcastFromDb.Title,
                ReleaseDate = podcastFromDb.ReleaseDate,
                Description = podcastFromDb.Description,
                File = podcastFromDb.File,
                PodcastHosts = podcastFromDb.PodCastHosts,
                PodcastGuests = podcastFromDb.PodCastGuests,
                PodcastReviews = podcastFromDb.PodcastReviews.Where(pr => pr.Review.IsChecked == true),
                IsHidden = podcastFromDb.IsHidden
            };

            return View(detailModel);
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            PodCast podcastFromDb = await _mediaWebDbContext.PodCasts.FirstOrDefaultAsync(m => m.Id == id);            

            PodcastEditViewModel editModel = new PodcastEditViewModel()
            {
                Title = podcastFromDb.Title,
                ReleaseDate = podcastFromDb.ReleaseDate,
                Description = podcastFromDb.Description,
                PodcastLink = podcastFromDb.File
            };

            return View(editModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, PodcastEditViewModel editModel)
        {
            PodCast podcastFromDb = await _mediaWebDbContext.PodCasts.FirstOrDefaultAsync(m => m.Id == id);
            List<string> podcastTitlesFromDb = await _mediaWebDbContext.PodCasts.Where(podcast => podcast != podcastFromDb).Select(p => p.Title).ToListAsync();

            if (podcastTitlesFromDb.Contains(StringEdits.FirstLettterToUpper(editModel.Title)))
            {
                return RedirectToAction("index");
            }

            podcastFromDb.Title = editModel.Title;
            podcastFromDb.ReleaseDate = editModel.ReleaseDate;
            podcastFromDb.Description = editModel.Description;
            podcastFromDb.File = UploadUtility.UploadFile(editModel.Podcast, "podcasts", _hostingEnvironment);

            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            PodCast podcastFromDb = await _mediaWebDbContext.PodCasts.FirstOrDefaultAsync(m => m.Id == id);

            PodcastDeleteViewModel deleteModel = new PodcastDeleteViewModel()
            {
                Id = podcastFromDb.Id,
                Title = podcastFromDb.Title
            };

            return View(deleteModel);
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            PodCast podcastFromDb = await _mediaWebDbContext.PodCasts.FirstOrDefaultAsync(m => m.Id == id);

            if (!string.IsNullOrEmpty(podcastFromDb.File))
            {
                _uploadUtility.DeleteFile(_hostingEnvironment.WebRootPath, podcastFromDb.File);
            }

            _mediaWebDbContext.PodCasts.Remove(podcastFromDb);
            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> HidePodcast(int id)
        {
            PodCast podcastFromDb = await _mediaWebDbContext.PodCasts.FirstOrDefaultAsync(m => m.Id == id);
            podcastFromDb.IsHidden = true;

            await _mediaWebDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }

}