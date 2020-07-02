using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.Music
{
    public class MusicReview 
    {
        public int SongId { get; set; }
        public Song Song { get; set; }
        public int AlbumId { get; set; }
        public Album Album { get; set; }
        public int ReviewId { get; set; }
        public Review Review { get; set; }
    }
}
