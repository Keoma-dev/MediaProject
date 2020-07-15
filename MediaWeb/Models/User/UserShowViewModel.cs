using MediaWeb.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Models.User
{
    public class UserShowViewModel
    {
        public List<MediaWebUser> Users { get; set; } = new List<MediaWebUser>();
    }
}
