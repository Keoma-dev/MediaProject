using MediaWeb.Domain.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.TVShow
{
    public class TVShowActor
    {
        public int TVShowId { get; set; }
        public TVshow TVShow { get; set; }
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
