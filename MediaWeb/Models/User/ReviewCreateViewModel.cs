using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.User
{
    public class ReviewCreateViewModel
    {
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
