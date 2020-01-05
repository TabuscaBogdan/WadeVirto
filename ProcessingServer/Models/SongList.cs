using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcessingServer.Models
{
    public class SongList
    {
        public string ListName { get; set; }
        public List<JsonLDSong> Songs { get; set; }
    }
}
