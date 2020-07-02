using MediaWeb.Domain.Podcast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain
{
    public class PodCast
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public IEnumerable<PodcastHost> PodCastHosts { get; set; }
        public IEnumerable<PodcastGuest> PodCastGuests { get; set; }
        public IEnumerable<PodcastReview> PodcastReviews { get; set; }
    }
}
