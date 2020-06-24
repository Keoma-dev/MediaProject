using MediaWeb.Domain.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.TVShow
{
    public class EpisodeActor
    {
        public int EpisodeId { get; set; }
        public Episode Episode { get; set; }
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
