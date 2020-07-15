using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.TVShow
{
    public class EpisodeCreateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public string Summary { get; set; }
        public int Season { get; set; }
        public int EpisodeNumber { get; set; }
    }
}
