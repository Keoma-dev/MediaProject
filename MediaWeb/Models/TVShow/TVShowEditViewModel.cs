using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.TVShow
{
    public class TVShowEditViewModel
    {
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Summary { get; set; }
        public string PictureFile { get; set; }
        public IFormFile Picture { get; set; }
    }
}
