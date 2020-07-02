﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.Music
{
    public class MusicCreateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }        
        public IFormFile Song { get; set; }
        public List<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
        public int[] SelectedGenres { get; set; }
        public string createdGenres { get; set; }
        public string createdArtists { get; set; }
        
    }
}
