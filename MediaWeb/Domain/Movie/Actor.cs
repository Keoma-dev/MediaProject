using MediaWeb.Domain.TVShow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.Movie
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; }
        public IEnumerable<TVShowActor> TVShowActors { get; set; }        
    }
}
