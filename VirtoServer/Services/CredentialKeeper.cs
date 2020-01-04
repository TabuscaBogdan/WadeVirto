using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoServer.Models;

namespace VirtoServer.Services
{
    public static class CredentialKeeper
    {
        private static Dictionary<LoginTokenModel, string> loginCache = new Dictionary<LoginTokenModel, string>();

        public static LoginTokenModel GenerateLoginToken()
        {
            Guid guid = Guid.NewGuid();
            var token = new LoginTokenModel
            {
                Token = guid.ToString(),
                Timestamp = DateTime.Now
            };
            return token;
        }

        public static void AddTokenToCache(LoginTokenModel token, string email)
        {
            loginCache.Add(token, email);
        }

        public static bool IsTokenCached(LoginTokenModel token)
        {
            foreach (var tk in loginCache.Keys)
                if(tk.Token == token.Token)
                    return true;
            return false;
        }

        public static string GetTokenUser(LoginTokenModel token)
        {
            foreach (var tk in loginCache.Keys)
                if (tk.Token == token.Token)
                    return loginCache[tk];
            return null;
        }

        public static void RemoveToken(LoginTokenModel token)
        {
            foreach (var tk in loginCache.Keys)
                if (tk.Token == token.Token)
                    loginCache.Remove(tk);
        }
    }
}
