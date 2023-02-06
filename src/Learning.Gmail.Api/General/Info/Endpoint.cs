using FastEndpoints;

namespace Learning.Gmail.Api.General.Info
{
    public class Endpoint : EndpointWithoutRequest
    {
        public override void Configure()
        {
            Get("api/info");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            await SendAsync("Hello World!!");
        }
    }
}
