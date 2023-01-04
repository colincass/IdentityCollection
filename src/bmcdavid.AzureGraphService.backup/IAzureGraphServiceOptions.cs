namespace bmcdavid.AzureGraphService
{
    /// <summary>
    /// Options for the Azure graph service
    /// </summary>
    public interface IAzureGraphServiceOptions
    {
        /// <summary>
        /// Application Id in Azure AD
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// Application Secret created in Azure AD
        /// </summary>
        string ClientSecret { get; }

        /// <summary>
        /// Default is https://graph.windows.net
        /// </summary>
        string GraphUrl { get; }

        /// <summary>
        /// Trusted URL to federation server meta data 
        /// </summary>
        string MetadataAddress { get; }

        /// <summary>
        /// Tenant ID, typically found in WtRealm address
        /// </summary>
        string TenantId { get; }

        /// <summary>
        /// Tenant Name of Azure AD in format of {name}.onmicrosoft.com
        /// </summary>
        string TenantName { get; }

        /// <summary>
        /// Value of Wtreal must *exactly* match what is configured in the federation server
        /// </summary>
        string WtRealm { get; }
    }
}