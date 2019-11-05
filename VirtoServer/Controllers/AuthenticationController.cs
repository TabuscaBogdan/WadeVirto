using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using VirtoServer.Models;

namespace VirtoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        // GET: api/Authentication
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Authentication/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// An endpoint for registering new users.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Post /Register
        /// {
        ///   "Email":"someEmail@email.com,
        ///   "Password":"Password"
        /// }
        /// </remarks>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public LoginTokenModel Register([FromBody] JObject credentials)
        {
            var parameters = credentials.ToObject<Dictionary<string, string>>();
            var token = new LoginTokenModel
            {
                Token = "OK",
                Timestamp = DateTime.Now
            };
            return token;
        }

        // PUT: api/Authentication/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
