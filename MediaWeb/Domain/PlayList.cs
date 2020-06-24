using MediaWeb.Domain.Music;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain
{
    public class PlayList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Song> Songs { get; set; }

        public MediaWebUser MediaWebUser { get; set; }
        public int MediaWebUserId { get; set; }
    }
}
