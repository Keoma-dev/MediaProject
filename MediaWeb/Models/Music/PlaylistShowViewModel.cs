using MediaWeb.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.Music
{
    public class PlaylistShowViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PlaylistSong> PlaylistSongs { get; set; }
        public List<string> Playlistsongfiles { get; set; } = new List<string>();
        public bool IsPrivate { get; set; }
        public MediaWebUser MediaWebUser { get; set; }
        public string MediaWebUserId { get; set; }
    }
}
