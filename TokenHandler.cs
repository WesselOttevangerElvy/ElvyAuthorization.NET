using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Elvy.Result;
using Elvy.Result.Enums;

namespace ElvyAuthorizationSDK
{
    public static class TokenHandler
    {
        private static readonly JwtSecurityTokenHandler Jsth = new();
        private static Result<List<Claim>> Decode(string token)
        {
            try
            {
                JwtSecurityToken jwt = Jsth.ReadJwtToken(token);
                return new Result<List<Claim>>(jwt.Claims.ToList());
            }
            catch (Exception ex) when (ex is ArgumentNullException or ArgumentException)
            {
                return new Result<List<Claim>>(ResultCode.InvalidToken, ex);
            }
            catch (Exception ex)
            {
                return new Result<List<Claim>>(ResultCode.UnknownError, ex);
            }
        }

        public static Result TokenNotExpired(string token, out DateTime expiration)
        {
            expiration = default;
            Result<List<Claim>> claimsResult = Decode(token);
            if (!claimsResult.Succes()) return Result.CreateFrom(claimsResult);

            Claim expirationClaim = claimsResult.Value.First(x => x.Type == "exp");
            expiration = DateTime.UnixEpoch.AddMilliseconds(Convert.ToUInt32(expirationClaim.Value));
            return expiration < DateTime.Now ? new Result(ResultCode.Expired) : new Result(ResultCode.OK);
        }
    }
}
