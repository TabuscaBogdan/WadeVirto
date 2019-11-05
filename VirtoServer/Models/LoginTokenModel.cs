using System;

namespace VirtoServer.Models
{
    public class LoginTokenModel
    {
        public string Token { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
