using FastEndpoints;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Learning.Gmail.Api.Filters.Categories
{
    public class CategoryEndpoint : Endpoint<CategoryRequest>
    {
        public override void Configure()
        {
            Get("/api/mailsbyCategory");
        }

        public override async Task HandleAsync(CategoryRequest req, CancellationToken ct)
        {
            var token = await HttpContext
                .GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            GmailService service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = GoogleCredential.FromAccessToken(token),
                ApplicationName = "Gmail API Application",
            });
            var ListRequest = service.Users.Labels.Get(req.EmailId, req.CategoryId);
            ListRequest.AccessToken = token; //ONLY FOR UNDREAD EMAIL'S...

            //GET ALL EMAILS
            var ListResponse = await ListRequest.ExecuteAsync();
            
            await SendAsync(ListResponse,
                             200,
                             ct);
        }
    }
}
