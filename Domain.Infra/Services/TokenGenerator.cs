using Microsoft.IdentityModel.Tokens;
using Now.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Now.Infra.Services
{
    public class TokenGenerator : ITokenGenerator
    {

        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _expiryMinutes;

        public TokenGenerator(string key, string issueer, string audience, string expiryMinutes)
        {
            _key = key;
            _issuer = issueer;
            _audience = audience;
            _expiryMinutes = expiryMinutes;
        }

        public string GenerateJWTToken((int? userId, string userName, string ipaddress, string browser) userDetails)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var (userId, userName, ipaddress,browser) = userDetails;

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Convert.ToString(userId)),
                new Claim(ClaimTypes.Name, userName),
                new Claim("UserId",  Convert.ToString(userId)),
                new Claim("ipaddress", ipaddress),
                new Claim("browser", browser)
            };


            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_expiryMinutes)),
                signingCredentials: signingCredentials
           );

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }
        public string GetUserIdFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Assuming the user ID is stored in a claim called "UserId"
            var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "UserId");
            return userIdClaim?.Value;
        }
    }
}
