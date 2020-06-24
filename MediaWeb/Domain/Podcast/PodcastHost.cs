using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.Podcast
{
    public class PodcastHost
    {
        public int PodcastId { get; set; }
        public PodCast Podcast { get; set; }
        public int HostId { get; set; }
        public Host Host { get; set; }
    }
}
