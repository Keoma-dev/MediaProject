using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.Podcast
{
    public class PodcastEditViewModel
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public string PodcastLink { get; set; }
        public IFormFile Podcast { get; set; }
    }
}
