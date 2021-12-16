using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace ElvyAuthorizationSDK
{
    public static class TokenHandler
    {
        private static readonly JwtSecurityTokenHandler Jsth = new();
        private static List<Claim> Decode(string token, out string message)
        {
            message = default;
            try
            {
                JwtSecurityToken jwt = Jsth.ReadJwtToken(token);
                return jwt.Claims.ToList();
            }
            catch (Exception ex) when (ex is ArgumentNullException or ArgumentException)
            {
                message = "token invalid";
                return null;
            }
            catch (Exception ex)
            {
                message = "unknown error";
                return null;
            }
        }

        public static bool TokenNotExpired(string token, out DateTime expiration)
        {
            expiration = default;
            List<Claim> claims = Decode(token, out string message);
            if (claims == null) return false;

            Claim expirationClaim = claims.First(x => x.Type == "exp");
            expiration = DateTime.UnixEpoch.AddMilliseconds(Convert.ToUInt32(expirationClaim.Value));
            return expiration >= DateTime.Now;
        }
    }
}
