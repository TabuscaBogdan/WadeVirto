﻿using System;
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
            var preferencesTask = NaturalLanguageProcessor.InterpretPreferences(request).GetAwaiter().GetResult();
            songs.Add(song1);

            
            return songs;
        }
    }
}