namespace SayHelloApp
{
    public class OpenIdConnectSettings
    {
        public string Authority { get; set; }

        public string ClientId { get; set; }

        public string Scopes { get; set; }

        public bool GetClaimsFromUserInfoEndpoint { get; set; }

        public bool SaveTokens { get; set; }

        public bool RequireHttpsMetadata { get; set; }
    }
}