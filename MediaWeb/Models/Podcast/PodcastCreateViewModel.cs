using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.Podcast
{
    public class PodcastCreateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public string PodcastLink { get; set; }
        public IFormFile Podcast { get; set; }       
        public string createdGuests { get; set; }
        public string createdHosts { get; set; }
    }
}
