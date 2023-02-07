using FastEndpoints;

namespace Learning.Gmail.Api.Filters.Categories
{
    public class CategoryRequest
    {
        [FromClaim("email")]
        public string? EmailId { get; set; }
    }
}
