using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.Music
{
    public class PlaylistCreateViewModel
    {
        public string Name { get; set; }
        public List<SelectListItem> Songs { get; set; } = new List<SelectListItem>();
        public bool IsPrivate { get; set; }
        public int[] SelectedSongs { get; set; }
    }
}
