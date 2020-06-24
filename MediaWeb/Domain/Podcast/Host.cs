using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.Podcast
{
    public class Host
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PodcastHost> PodCastHosts { get; set; }
    }
}
