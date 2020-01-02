using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserDataManager
{
    public sealed class DatabaseManager
    {
        private static DatabaseManager instance = null;
        private static IFirebaseClient client = null;
        private static readonly object padlock = new object();
        private static readonly string userPath = "Users/";

        public DatabaseManager()
        {
            var connector = new Connection();
            client = connector.SetupClient();
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
            FirebaseResponse response = await client.GetAsync(userPath+email);
            if(response.Body=="null")
            {
                SetResponse setResponse = await client.SetAsync($"{userPath}{email}/Password",password);
                var result = setResponse.ResultAs<string>();

                if(result!=null)
                {
                    return true;
                }
            }
            return false;

        }
    }
}
