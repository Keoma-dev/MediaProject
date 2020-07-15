using MediaWeb.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.User
{
    public class UserIndexViewModel
    {
        public string UserName { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<string> Movies { get; set; }
        public IEnumerable<PlayList> Playlists { get; set; }
        public IEnumerable<string> Podcasts { get; set; }
        public IEnumerable<string> TVShows { get; set; }
        public IEnumerable<MediaWebUser> Users { get; set; }
    }
}
