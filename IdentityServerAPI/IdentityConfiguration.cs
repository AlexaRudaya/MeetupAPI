namespace IdentityServerAPI
{
    public sealed class IdentityConfiguration
    {
        /// <summary>
        ///  Returns a TestUser with some specific JWT Claims.
        /// </summary>
        public static List<TestUser> TestUsers =>
              new List<TestUser>
              {
                    new TestUser
                    {
                        SubjectId = "1111",
                        Username = "john",
                        Password = "password75",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "John Smith"),
                            new Claim(JwtClaimTypes.GivenName, "John"),
                            new Claim(JwtClaimTypes.WebSite, "http://localhost4455.com"),
                        }
                    }
              };

        /// <summary>
        /// Particular user unique data. 
        /// </summary>
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        /// <summary>
        /// Authorized user capabilities: read and write.
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("myApi.read"),
                new ApiScope("myApi.write"),
            };

        /// <summary>
        /// API defining: supported scopes and the secret. Hashing the secret code.
        /// </summary>
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("myApi")
                {
                    Scopes = new List<string>{ "myApi.read","myApi.write" },
                    ApiSecrets = new List<Secret>{ new Secret("supersecret".Sha256()) }
                }
            };

        /// <summary>
        /// Establishing who will have access to "myApi".
        /// </summary>
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "cwm.client",
                    ClientName = "Client Credentials Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedScopes = { "myApi.read" }
                }
            };
    }
}


