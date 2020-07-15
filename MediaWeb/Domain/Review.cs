using MediaWeb.Domain.Movie;
using MediaWeb.Domain.Music;
using MediaWeb.Domain.Podcast;
using MediaWeb.Domain.TVShow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public MediaWebUser MediaWebUser { get; set; }
        public string MediaWebUserId { get; set; }
        public bool IsChecked { get; set; }
        public IEnumerable<MovieReview> MovieReviews { get; set; }
        public IEnumerable<MusicReview> MusicReviews { get; set; }
        public IEnumerable<PodcastReview> PodcastReviews { get; set; }
        public IEnumerable<TVShowReview> TVShowReviews { get; set; }        
    }
}
