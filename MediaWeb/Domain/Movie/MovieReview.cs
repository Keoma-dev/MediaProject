﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain.Movie
{
    public class MovieReview
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int ReviewId { get; set; }
        public Review Review { get; set; }
    }
}
