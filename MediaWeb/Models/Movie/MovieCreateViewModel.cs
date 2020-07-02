using MediaWeb.Domain.Movie;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.Movie
{
    public class MovieCreateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Summary { get; set; }
        public IFormFile Photo { get; set; }
        public List<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
        public int[] SelectedGenres { get; set; }     
        public string createdGenres { get; set; }
        public string createdActors { get; set; }
        public string createdDirectors { get; set; }
    }
}
