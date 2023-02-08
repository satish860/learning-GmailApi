using FastEndpoints;

namespace Learning.Gmail.Api.Filters.Unread
{
    public class UnReadRequest
    {
        [FromClaim("email")]
        public string? EmailId { get; set; }
    }
}
