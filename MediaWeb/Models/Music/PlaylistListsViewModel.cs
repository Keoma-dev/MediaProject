using MediaWeb.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.Music
{
    public class PlaylistListsViewModel
    {
        public List<PlayList> PublicLists { get; set; } = new List<PlayList>();
        public List<PlayList> PrivateLists { get; set; } = new List<PlayList>();
    }
}
