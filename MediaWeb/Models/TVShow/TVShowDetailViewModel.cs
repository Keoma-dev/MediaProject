using MediaWeb.Domain.TVShow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.TVShow
{
    public class TVShowDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Summary { get; set; }
        public string Picture { get; set; }
        public int NumberOfSeaons { get; set; }
        public bool IsHidden { get; set; }
        public IEnumerable<TVShowGenre> TVShowGenres { get; set; }
        public IEnumerable<TVShowReview> TVShowReviews { get; set; }
        public IEnumerable<TVShowActor> TVShowActors { get; set; }
        public IEnumerable<TVShowDirector> TVShowDirectors { get; set; }
        public IEnumerable<Episode> Episodes { get; set; }
    }
}
