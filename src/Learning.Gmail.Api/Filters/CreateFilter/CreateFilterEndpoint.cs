using FastEndpoints;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Threading;

namespace Learning.Gmail.Api.Filters.CreateFilter
{
    public class CreateFilterEndpoint : Endpoint<CreateFilterRequest>
    {
        public override void Configure()
        {
            Post("/api/CreateFilter");
        }

        public override async Task HandleAsync(CreateFilterRequest req, CancellationToken ct)
        {
            var token = await HttpContext
               .GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            GmailService service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GoogleCredential.FromAccessToken(token),
                ApplicationName = "Gmail API Application",
            });
            Filter filter = new Filter
            {
                Criteria = new FilterCriteria
                {
                    Query = "(is:unread) and (Older_than:6m)",
                },
                Action = new FilterAction
                {
                    AddLabelIds = new List<string> { "Label_296967431985014407" }
                },
                Id = "UnreadMailFilter"
            };
            var filterRequest = service.Users.Settings.Filters.Create(filter,req.Email);
            var result = filterRequest.Execute();
            await SendAsync("Hello Filters", 201, ct);
        }

    }
}
