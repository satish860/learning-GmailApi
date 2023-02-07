using FastEndpoints;

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
            await SendAsync("Hello world",
                             200,
                             ct);
        }
    }
}
