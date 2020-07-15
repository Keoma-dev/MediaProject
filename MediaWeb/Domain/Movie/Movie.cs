using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.Movie
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Summary { get; set; }
        public string Photo { get; set; }
        public bool IsHidden { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; }
        public ICollection<MovieDirector> MovieDirectors { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; }
        public ICollection<MovieReview> MovieReviews { get; set; }
    }
}
