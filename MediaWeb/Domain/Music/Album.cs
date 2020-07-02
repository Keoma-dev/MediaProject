using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.Music
{
    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<Song> Songs { get; set; } = new List<Song>();
        public IEnumerable<MusicReview> AlbumReviews { get; set; }
    }
}
