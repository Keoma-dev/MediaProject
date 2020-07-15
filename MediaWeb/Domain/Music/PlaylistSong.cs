using MediaWeb.Domain.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain
{
    public class PlaylistSong
    {
        public Song Song { get; set; }
        public int SongId { get; set; }
        public PlayList PlayList { get; set; }
        public int PlayListId { get; set; }
    }
}
