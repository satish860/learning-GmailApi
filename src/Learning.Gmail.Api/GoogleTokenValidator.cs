using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Learning.Gmail.Api
{
    public class GmailTokenValidator : ISecurityTokenValidator
    {
        private int _maxTokenSizeInBytes = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;
        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get => _maxTokenSizeInBytes; set => _maxTokenSizeInBytes = value; }

        public bool CanReadToken(string securityToken)
        {

            return true;
        }

        public ClaimsPrincipal? ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            HttpClient httpClient = new HttpClient();
            validatedToken = null;
            var httpresponse = httpClient.GetAsync($"https://www.googleapis.com/oauth2/v3/userinfo?access_token={securityToken}").Result;
            if (httpresponse.IsSuccessStatusCode)
            {
                var claims = httpresponse.Content.ReadFromJsonAsync<Dictionary<string, object>>().Result;
                List<Claim> claimsTokens = new List<Claim>();
                foreach (var item in claims)
                {
                    claimsTokens.Add(new Claim(item.Key, item.Value.ToString()));
                }
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claimsTokens, JwtBearerDefaults.AuthenticationScheme);
                validatedToken = new JwtSecurityToken(claims: claimsTokens);

                return new ClaimsPrincipal(claimsIdentity);
            }
            return null;

        }
    }
}
