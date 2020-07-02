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

        public IEnumerable<string> MusicGenres { get; set; }
    }
}
