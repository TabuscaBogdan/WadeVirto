using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserDataManager
{
    internal class Connection
    {
        private string authSecret = "";
        private string basePath = "https://musicpreferencessparq.firebaseio.com/";

        public FirebaseClient SetupClient()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = authSecret,
                BasePath = basePath
            };
            return new FirebaseClient(config);
        }
    }
}
