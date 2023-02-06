using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Learning.Gmail.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var googleOptions = builder.Configuration.GetSection(GoogleOptions.Google)
                                                                  .Get<GoogleOptions>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                           .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, token =>
                           {
                               token.Audience = googleOptions.Audience;
                               token.Authority = googleOptions.Authority;
                           });
            builder.Services.AddFastEndpoints();
            builder.Services.AddSwaggerDoc(config =>
            {
                config.AddAuth(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = googleOptions.AuthorizationUrl,
                            TokenUrl = googleOptions.TokenUrl,
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid","Open Identity" },
                                {"profile","profile" },
                                {"email","email" }
                            }
                        }
                    },
                    Description = "Gmail OAuth2 Service",
                });

            }, addJWTBearerAuth: false);
            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseFastEndpoints();
            app.UseSwaggerGen();

            app.Run();
        }
    }
}