using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.TVShow
{
    public class Episode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public int Season { get; set; }
        public IEnumerable<EpisodeActor> EpisodeActors { get; set; }
        public IEnumerable<EpisodeDirector> EpisodeDirectors { get; set; }
        public IEnumerable<TVShowReview> EpisodeReviews { get; set; }
    }
}
