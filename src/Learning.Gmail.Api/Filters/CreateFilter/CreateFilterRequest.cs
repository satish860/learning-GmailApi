using FastEndpoints;

namespace Learning.Gmail.Api.Filters.CreateFilter
{
    public class CreateFilterRequest
    {
        public string? Filter { get; set; }

        [FromClaim("email")]
        public string Email { get; set; }
    }
}
