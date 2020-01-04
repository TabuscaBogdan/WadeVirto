using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserDataManager.DbObjects;
using VirtoServer.Models;
using VirtoServer.Services;

namespace VirtoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessRequestController : ControllerBase
    {
        /// <summary>
        /// An endpoint for checking the availability of the processing server.
        /// </summary>
        [HttpGet("[action]")]
        public ServerCapacityModel CheckAvailability()
        {
            return new ServerCapacityModel
            {
                Loaded = false,
                ServerName = "Python Processing server 1"
            };
        }

        [HttpPost("[action]")]
        public bool Songs(string token, DateTime tokenTime, SongList songs)
        {
            string email;
            var loginToken = new LoginTokenModel
            {
                Token = token,
                Timestamp = tokenTime
            };
            if (CredentialKeeper.IsTokenCached(loginToken))
            {
                email = CredentialKeeper.GetTokenUser(loginToken);
                var result = Program.database.SaveList(email, songs).Result;
                return result;
            }
            else
                return false;
            
        }

        /// <summary>
        /// An endpoint for processing an user search request.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Post /Register
        /// {
        ///   "username": "Jimmy J."
        ///   "request":"I like blues music but not the modern stuff that is out there now and I also want it
        ///              to be more oriented towards metal with heavier bass and guitars."
        /// }
        /// </remarks>
        /// <param name="username"> The one who requested the search</param>
        /// <param name="request"> Actual request written in natural language to be processed</param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IList<JsonLDSong> ProcessRequest(string username,[FromBody] string request)
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
