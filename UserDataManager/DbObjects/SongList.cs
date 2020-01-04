using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserDataManager.DbObjects
{
    public class SongList
    {
        public string ListName { get; set; }
        public List<JsonLDSong> Songs { get; set; }
    }
}
