using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VirtoServer.Models
{
    public class JsonLDSong
    {
        public string id { get; set; }
        public string type { get; set; }
        public string dcTitle { get; set; }
        public JsonLDMaker foafMaker { get; set; }
    }
}
