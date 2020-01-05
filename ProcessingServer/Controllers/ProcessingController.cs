using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProcessingServer.Models;

namespace ProcessingServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessingController : ControllerBase
    {
        [HttpPost("[action]")]
        public List<JsonLDSong> SeekSongs([FromBody] string request)
        {
            var songs = new List<JsonLDSong>();
            var song1 = new JsonLDSong
            {
                id = "http://example.org/#track1",
                type = "mo:Track",
                dcTitle = "Turnover",
                foafMaker = new JsonLDMaker
                {
                    id = "http://musicbrainz.org/artist/233fc3f3-6de2-465c-985e-e721dbabbace#_",
                    type = "mo:MusicGroup",
                    foafName = "Fugazi"
                }
            };
            songs.Add(song1);
            return songs;
        }
    }
}