using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.Podcast
{
    public class PodcastGuest
    {
        public int PodcastId { get; set; }
        public PodCast Podcast { get; set; }
        public int GuestId { get; set; }
        public Guest Guest { get; set; }
    }
}
