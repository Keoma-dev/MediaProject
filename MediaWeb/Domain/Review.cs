using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Domain
{
    public class Review
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Grade { get; set; }
        public string Content { get; set; }

        public MediaWebUser MediaWebUser { get; set; }
        public int MediaWebUserId { get; set; }
    }
}
