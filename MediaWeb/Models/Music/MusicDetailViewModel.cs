using MediaWeb.Domain;
using MediaWeb.Domain.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.Music
{
    public class MusicDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }       
        public string SongFile { get; set; }
        public bool IsHidden { get; set; }
        public IEnumerable<SongArtist> SongArtists { get; set; }
        public IEnumerable<MusicReview> SongReviews { get; set; }
        public IEnumerable<PlayList> MyPlaylists { get; set; }
    }
}
