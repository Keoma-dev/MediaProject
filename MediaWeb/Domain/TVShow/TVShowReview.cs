using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.TVShow
{
    public class TVShowReview
    {
        public int TVShowId { get; set; }
        public TVshow TvShow { get; set; }
        public int EpisodeId { get; set; }
        public Episode Episode { get; set; }
        public int ReviewId { get; set; }
        public Review Review { get; set; }
    }
}
