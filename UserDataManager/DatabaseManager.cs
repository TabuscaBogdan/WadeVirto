using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserDataManager.DbObjects;

namespace UserDataManager
{
    public sealed class DatabaseManager
    {
        private static DatabaseManager instance = null;
        private static IFirebaseClient client = null;
        private static readonly object padlock = new object();
        private static readonly string userPath = "Users/";
        private static readonly string userCount = "UserCount";

        public DatabaseManager()
        {
            var connector = new Connection();
            client = connector.SetupClient();
        }
        private string BadCharacterTrim(string value)
        {
            value = value.Replace("\"", "");
            value = value.Replace(".", "");
            value = value.Replace("@", "");
            return value;
        }

        public static DatabaseManager Instance
        {
            get
            {
                lock(padlock)
                {
                    if (instance == null)
                        throw new Exception("Object was not created!");
                }
                return instance;
            }
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            var username = email.Split("@")[0];
            email = BadCharacterTrim(email);
            password = BadCharacterTrim(password);

            

            FirebaseResponse response = await client.GetAsync(userPath+email);
            if(response.Body=="null")
            {
                SetResponse userSetResponse = await client.SetAsync($"{userPath}{email}/Username", username);
                SetResponse passSetResponse = await client.SetAsync($"{userPath}{email}/Password",password);
                var result = passSetResponse.ResultAs<string>();

                if(result!=null)
                {
                    return true;
                }
            }
            return false;

        }

        public async Task<bool> LoginUser(string email, string password)
        {
            email = BadCharacterTrim(email);
            password = BadCharacterTrim(password);

            FirebaseResponse response = await client.GetAsync($"{userPath}{email}/Password");
            if(response.Body!="null")
            {
                var dbPass = BadCharacterTrim(response.Body);
                if (password.Equals(dbPass))
                    return true;
            }
            return false;
        }

        public async Task<bool> SaveList(string email, SongList songs)
        {
            email = BadCharacterTrim(email);
            SetResponse setResponse = await client.SetAsync($"{userPath}{email}/Lists/{songs.ListName}", songs);
            var result = setResponse.ResultAs<SongList>();
            if(result!=null)
                return true;
            return false;
        }
    }
}
