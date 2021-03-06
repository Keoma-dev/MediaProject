﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain
{
    public class MediaWebUser : IdentityUser
    {
        public List<PlayList> PlayLists { get; set; } = new List<PlayList>();
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}
