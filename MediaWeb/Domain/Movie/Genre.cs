using MediaWeb.Domain.TVShow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.Movie
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<MovieGenre> MovieGenres { get; set; }
        public IEnumerable<TVShowGenre> TVShowGenres { get; set; }
    }
}
