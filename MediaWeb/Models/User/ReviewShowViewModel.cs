using MediaWeb.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.User
{
    public class ReviewShowViewModel
    {
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}
