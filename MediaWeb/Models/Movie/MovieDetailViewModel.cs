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

        public IEnumerable<string> MovieGenres { get; set; }
        public IEnumerable<MovieReview> MovieReviews { get; set; }
    }
}
