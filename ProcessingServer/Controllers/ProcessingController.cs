using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProcessingServer.Models;
using ProcessingServer.Services;

namespace ProcessingServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessingController : ControllerBase
    {
        [HttpPost("[action]")]


        //TODO The KEY!!!
        /*
         * SELECT DISTINCT ?item
           WHERE
           {
           ?item rdfs:label "Hammerfall"
           } to get artist id

            SELECT DISTINCT ?property ?hasValue ?isValueOf
           WHERE {
           { <http://dbtune.org/musicbrainz/resource/artist/00039b8a-3da6-4cb2-85e3-f93e30f43049> ?property ?hasValue 
           }
           }
           ORDER BY (!BOUND(?hasValue)) ?property ?hasValue ?isValueOf
           to get information about artist, use /artist id you got from previous querry
         */


        public SongList SeekSongs([FromBody] string request)
        {
            var songs = new List<JsonLDSong>();
            var preferencesTask = NaturalLanguageProcessor.InterpretPreferences(request).GetAwaiter().GetResult();
            var seeker = new SongSeeker();

            var songList = seeker.SeekSongsBasedOnPreferences(preferencesTask);

            return songList;
        }
    }
}