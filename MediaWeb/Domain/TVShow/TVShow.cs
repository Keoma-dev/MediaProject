using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.TVShow
{
    public class TVshow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Seasons { get; set; }
        public string Summary { get; set; }
        public IEnumerable<Episode> Episodes { get; set; }
        public IEnumerable<TVShowActor> TVShowActors { get; set; }
        public IEnumerable<TVShowDirector> TVShowDirectors { get; set; }
        public IEnumerable<TVShowGenre> TVShowGenres { get; set; }
    }
}
