using MediaWeb.Domain.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.Movie
{
    public class MovieDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Summary { get; set; }
        public string Photo { get; set; }
        public bool IsHidden { get; set; }
        public IEnumerable<MovieGenre> MovieGenres { get; set; }
        public IEnumerable<MovieReview> MovieReviews { get; set; }
        public IEnumerable<MovieActor> MovieActors { get; set; }
        public IEnumerable<MovieDirector> MovieDirectors { get; set; }
    }
}
