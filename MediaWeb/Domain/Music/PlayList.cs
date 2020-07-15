using MediaWeb.Domain.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain
{
    public class PlayList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PlaylistSong> PlaylistSongs { get; set; }
        public bool IsPrivate { get; set; }
        public MediaWebUser MediaWebUser { get; set; }
        public string MediaWebUserId { get; set; }

    }
}
