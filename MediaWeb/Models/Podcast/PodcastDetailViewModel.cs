using MediaWeb.Domain.Podcast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.Podcast
{
    public class PodcastDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public string File { get; set; }
        public bool IsHidden { get; set; }
        public IEnumerable<PodcastHost> PodcastHosts { get; set; }
        public IEnumerable<PodcastGuest> PodcastGuests { get; set; }
        public IEnumerable<PodcastReview> PodcastReviews { get; set; }
    }
}
