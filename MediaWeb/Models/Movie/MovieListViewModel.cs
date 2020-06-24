using MediaWeb.Domain.Movie;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace MediaWeb.Models.Movie
{
    public class MovieListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }      
        
    }
}
