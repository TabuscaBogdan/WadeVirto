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


        public List<JsonLDSong> SeekSongs([FromBody] string request)
        {
            var songs = new List<JsonLDSong>();

            var preferencesTask = NaturalLanguageProcessor.InterpretPreferences(request).GetAwaiter().GetResult();

            //TODO Work on this interogator!!!
            var sparqlInterogator = new SPARQLInterogator();

            var artists = sparqlInterogator.GetArtistLinks(preferencesTask["LikeArtist"]);
            //var tags = sparqlInterogator.GetTagLinks(preferencesTask["LikeMusicTypes"]);
            //var tagInfo = sparqlInterogator.GetTagInformation(tags[0]);
            foreach(var artistLink in artists)
            {
                var info = sparqlInterogator.GetArtistInformation(artistLink);
                var maker = sparqlInterogator.ExtractArtistMakerInformation(info);
                var trackLinks = sparqlInterogator.ExtractTrackLinks(info);
                foreach(var trackLink in trackLinks)
                {
                    var trackInfo = sparqlInterogator.GetTrackInformation(trackLink);
                    var songLD = sparqlInterogator.ExtractSongInformation(trackInfo, maker);
                    songs.Add(songLD);
                }
                
            }

            //var trackInfo = sparqlInterogator.GetTrackInformation("http://dbtune.org/musicbrainz/resource/track/ae9c96ed-1e5f-49fb-86f0-0cf4df72dddc");
            //var songLD = sparqlInterogator.ExtractSongInformation(trackInfo);
            //songs.Add(song1);
            //songs.Add(songLD);

            
            return songs;
        }
    }
}