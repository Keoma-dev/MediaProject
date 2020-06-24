using MediaWeb.Domain.TVShow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.Movie
{
    public class Director
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<MovieDirector> MovieDirectors { get; set; }
        public IEnumerable<TVShowDirector> TVShowDirectors { get; set; }
        public IEnumerable<EpisodeDirector> EpisodeDirectors { get; set; }
    }
}
