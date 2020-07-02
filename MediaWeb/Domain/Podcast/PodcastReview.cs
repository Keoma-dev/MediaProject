using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.Podcast
{
    public class PodcastReview
    {
        public int PodcastId { get; set; }
        public PodCast PodCast { get; set; }
        public int ReviewId { get; set; }
        public Review Review { get; set; }
    }
}
