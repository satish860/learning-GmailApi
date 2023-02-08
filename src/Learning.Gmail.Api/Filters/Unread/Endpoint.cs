using FastEndpoints;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using YamlDotNet.Core.Tokens;

namespace Learning.Gmail.Api.Filters.Unread
{
    public class Endpoint : Endpoint<UnReadRequest>
    { 
        public override void Configure()
        {
            Get("/api/unread");
        }

        public override async Task HandleAsync(UnReadRequest req ,CancellationToken ct)
        {
            var token = await HttpContext
                .GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            GmailService service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GoogleCredential.FromAccessToken(token),
                ApplicationName = "Gmail API Application",
            });
            UsersResource.MessagesResource.ListRequest ListRequest = service.Users.Messages.List(req.EmailId);
            ListRequest.IncludeSpamTrash = false;
            ListRequest.Q = "(is:unread) and (older_than:6m)"; 
            var result = await ListRequest.ExecuteAsync();
            await SendAsync(result, 200, ct);
            
        }
    }
}
