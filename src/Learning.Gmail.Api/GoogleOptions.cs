namespace Learning.Gmail.Api
{
    public class GoogleOptions
    {
        public const string Google = "google";

        public string Audience { get; set; }

        public string Authority { get; set; }

        public string AuthorizationUrl { get; set; }

        public string TokenUrl { get; set; }
    }
}
