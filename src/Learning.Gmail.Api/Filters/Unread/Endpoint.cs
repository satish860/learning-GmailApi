using FastEndpoints;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using YamlDotNet.Core.Tokens;
using System.Linq;

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
            ListMessagesResponse listMessagesResponse = null;
            List<string> ids = new List<string>();
            do
            {
                UsersResource.MessagesResource.ListRequest ListRequest = service.Users.Messages.List(req.EmailId);
                ListRequest.IncludeSpamTrash = false;
                ListRequest.Q = "(is:unread) and (older_than:6m)";
                ListRequest.MaxResults = 500;
                ListRequest.PageToken = listMessagesResponse?.NextPageToken;
                listMessagesResponse = await ListRequest.ExecuteAsync();
                ids.AddRange(listMessagesResponse.Messages.Select(p=>p.Id).ToList());
            } 
            while (listMessagesResponse.NextPageToken != null);

           
            await SendAsync(ids, 200, ct);
            
        }
    }
}
