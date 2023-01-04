using System.Configuration;

namespace bmcdavid.AzureGraphService
{
    /// <summary>
    /// Options for Azure graph service
    /// </summary>
    public class AzureGraphServiceOptions : IAzureGraphServiceOptions
    {
        /// <summary>
        /// Empty constructor
        /// </summary>
        public AzureGraphServiceOptions() : this(clientId: null) { }

        /// <summary>
        /// DI Constructor
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="graphUrl"></param>
        /// <param name="tenantName"></param>
        /// <param name="tenantId"></param>
        /// <param name="metaAddress"></param>
        /// <param name="wtRealm"></param>
        protected AzureGraphServiceOptions(string clientId = "", string clientSecret = "", string graphUrl = "", string tenantName = "", string tenantId = "", string metaAddress = "", string wtRealm = "")
        {
            ClientId = !string.IsNullOrWhiteSpace(clientId) ? clientId : ConfigurationManager.AppSettings["azuregraphservice:ClientId"];
            ClientSecret = !string.IsNullOrWhiteSpace(clientSecret) ? clientSecret : ConfigurationManager.AppSettings["azuregraphservice:ClientSecret"];
            GraphUrl = !string.IsNullOrWhiteSpace(graphUrl) ? graphUrl : ConfigurationManager.AppSettings["azuregraphservice:GraphUrl"];
            TenantName = !string.IsNullOrWhiteSpace(tenantName) ? tenantName : ConfigurationManager.AppSettings["azuregraphservice:TenantName"];
            TenantId = !string.IsNullOrWhiteSpace(tenantName) ? tenantName : ConfigurationManager.AppSettings["azuregraphservice:TenantId"];
            MetadataAddress = !string.IsNullOrWhiteSpace(metaAddress) ? metaAddress : ConfigurationManager.AppSettings["azuregraphservice:MetadataAddress"];
            WtRealm = !string.IsNullOrWhiteSpace(wtRealm) ? metaAddress : ConfigurationManager.AppSettings["azuregraphservice:Wtrealm"];
        }

        /// <summary>
        /// Application Id in Azure AD
        /// </summary>
        public string ClientId { get; }

        /// <summary>
        /// Application Secret created in Azure AD
        /// </summary>
        public string ClientSecret { get; }

        /// <summary>
        /// Default is https://graph.windows.net
        /// </summary>
        public string GraphUrl { get; }

        /// <summary>
        /// Trusted URL to federation server meta data 
        /// </summary>
        public string MetadataAddress { get; }

        /// <summary>
        /// Tenant ID, typically found in WtRealm address
        /// </summary>
        public string TenantId { get; }

        /// <summary>
        /// Tenant Name of Azure AD in format of {name}.onmicrosoft.com
        /// </summary>
        public string TenantName { get; }

        /// <summary>
        /// Value of Wtreal must *exactly* match what is configured in the federation server
        /// </summary>
        public string WtRealm { get; }
    }
}