# Claim Criteria for Episerver Visitor Groups

Adds a visitor group criteria for claims. Also provides a **IClaimTypeSelectorExtension** interface to provide additional claims to the criteria module. To use simple add a **ModuleDependency** attribute on an Episerver configuration module for the **ClaimsCriteriaConfigurationModule** and add a custom implementation of the above interface as a singleton.

Below is a sample of extending claim types for the criteria type dropdown.

```cs
using bmcdavid.Episerver.ClaimsCriteria;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using System.Collections.Generic;

namespace AlloyWeb
{
  public class AppAdditionalClaims : IClaimTypeSelectorExtension
  {
    Dictionary<string, string> IClaimTypeSelectorExtension.AdditionalClaims => new Dictionary<string, string>
    {
      {"ExampleClaim", "http:/example-claim.com/example" },
    };
  }

  [ModuleDependency(typeof(ClaimsCriteriaConfigurationModule))]
  public class ConfigureClaimsExtender : IConfigurableModule
  {
    public void ConfigureContainer(ServiceConfigurationContext context)
    {
      context.Services
        .AddSingleton<IClaimTypeSelectorExtension, AppAdditionalClaims>();
    }

    public void Initialize(InitializationEngine context) { }

    public void Uninitialize(InitializationEngine context) { }
  }
}
```