using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.Music
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SongFile { get; set; }
        public bool IsHidden { get; set; } = false;
        public DateTime ReleaseDate { get; set; }
        public ICollection<SongArtist> SongArtists { get; set; }
        public ICollection<MusicReview> SongReviews { get; set; }
        public ICollection<MusicGenre> MusicGenres { get; set; }
        public ICollection<PlaylistSong> PlaylistSongs { get; set; }
    }
}
