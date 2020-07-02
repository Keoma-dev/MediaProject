using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.Music
{
    public class MusicEditViewModel
    {
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }        
        public IFormFile Song { get; set; }
    }
}
