using MediaWeb.Domain.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.TVShow
{
    public class TVShowDirector
    {
        public int TVShowId { get; set; }
        public TVshow TVShow { get; set; }
        public int DirectorId { get; set; }
        public Director Director { get; set; }
    }
}
