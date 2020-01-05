using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        private readonly string processingServerAddress = "https://localhost:44310/";
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

        [HttpGet("[action]")]
        public  Dictionary<string,SongList> Songs(string token)
        {
            string email;
            var loginToken = new LoginTokenModel
            {
                Token = token,
                Timestamp = DateTime.Now
            };
            if (CredentialKeeper.IsTokenCached(loginToken))
            {
                email = CredentialKeeper.GetTokenUser(loginToken);
                var lists = Program.database.GetLists(email).Result;
                return lists;
            }
            else
                return new Dictionary<string, SongList>();
        }
        
        [HttpPost("[action]")]
        public List<JsonLDSong> SongsSearch(string token,[FromBody] string request)
        {
            string email;
            var loginToken = new LoginTokenModel
            {
                Token = token,
                Timestamp = DateTime.Now
            };
            if(CredentialKeeper.IsTokenCached(loginToken))
            {
                email = CredentialKeeper.GetTokenUser(loginToken);
                using(var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(processingServerAddress);
                    var processTask = client.PostAsJsonAsync<string>("api/Processing/SeekSongs", request);
                    processTask.Wait();
                    var result = processTask.Result;
                    if(result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<List<JsonLDSong>>();
                        readTask.Wait();
                        var songs = readTask.Result;
                        return songs;
                    }
                    return new List<JsonLDSong>();
                }
            }
            else
            {
                return new List<JsonLDSong>();
            }
        }
        //[HttpPost("[action]")]
        //public IList<JsonLDSong> ProcessRequest(string username,[FromBody] string request)
        //{
        //    var songs = new List<JsonLDSong>();
        //    var song1 = new JsonLDSong
        //    {
        //        id = "http://example.org/#track1",
        //        type = "mo:Track",
        //        dcTitle = "Turnover",
        //        foafMaker = new JsonLDMaker
        //        {
        //            id = "http://musicbrainz.org/artist/233fc3f3-6de2-465c-985e-e721dbabbace#_",
        //            type = "mo:MusicGroup",
        //            foafName = "Fugazi"
        //        }
        //    };
        //    songs.Add(song1);
        //    return songs;
        //}

    }
}
