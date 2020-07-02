using MediaWeb.Domain.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.Music
{
    public class MusicGenre
    {
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public int SongId { get; set; }
        public Song Song { get; set; }
    }
}
