﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using VirtoServer.Models;
using UserDataManager;
using System.Threading.Tasks;
using VirtoServer.Services;

namespace VirtoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        /// <summary>
        /// An endpoint for registering new users.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Post /Register
        /// {
        ///   Email: 'someEmail@email.com',
        ///   Password: 'Password'
        /// }
        /// </remarks>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public LoginTokenModel Registration([FromBody] JObject credentials)
        {
            var parameters = credentials.ToObject<Dictionary<string, string>>();
            var registered = Program.database.RegisterUser(parameters["Email"], parameters["Password"]).Result;
            var token = CredentialKeeper.GenerateLoginToken();
            CredentialKeeper.AddTokenToCache(token, parameters["Email"]);
            return token;
        }

        /// <summary>
        /// An endpoint for logging in users.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// Post /Register
        /// {
        ///   Email:"someEmail@email.com,
        ///   "Password":"Password"
        /// }
        /// </remarks>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public LoginTokenModel Login([FromBody] JObject credentials)
        {
            var parameters = credentials.ToObject<Dictionary<string, string>>();
            if(Program.database.LoginUser(parameters["Email"],parameters["Password"]).Result)
            {
                var token = CredentialKeeper.GenerateLoginToken();
                CredentialKeeper.AddTokenToCache(token, parameters["Email"]);
                return token;
            }
            return new LoginTokenModel { Token = "Login failed!", Timestamp = DateTime.Now };
        }

        /// <summary>
        /// An endpoint for logging out users.
        /// </summary>
        [HttpDelete("[action]")]
        public void Logout(LoginTokenModel token)
        {
            CredentialKeeper.RemoveToken(token);
        }
    }
}
