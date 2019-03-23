# Synchronized Provider Extensions

Provides Episerver roles managed within the CMS for synchronized users. Also provides ability to add these roles and visitor groups as roles during the OWIN identity setup using the **IExtendedUserTools** interface.

**Important:** Roles are only added upon user login, so for any changes to take, user must logout and login for new assignments to take effect.

## Example Usage

Below is an example of using extended roles

```cs
// Sync user and the roles to EPiServer
var synchUserService = ServiceLocator.Current.GetInstance<ISynchronizingUserService>();
await synchUserService.SynchronizeAsync(ctx.AuthenticationTicket.Identity);

// Set users loging date and manually assigned roles
var extendedUserTools = ServiceLocator.Current.GetInstance<IExtendedUserTools>();
await extendedUserTools.SetExtendedRolesAsync(ctx.AuthenticationTicket.Identity, DateTime.UtcNow);
// Sets visitor group roles as assigned claim roles
await extendedUserTools.AddVisitorGroupRolesAsClaimsAsync(ctx.AuthenticationTicket.Identity, new HttpContextWrapper(HttpContext.Current));  
```

## ASP.NET Framework Support 

For the full ASP.NET framework targeting version 4.7.1 and up add the following assembly to the web.config file's compilation section to add NETSTANDARD support. Previous versions will use the NuGet package.

```xml
<compilation debug="true" targetFramework="4.7.1" optimizeCompilations="false">
    <assemblies>
    <add assembly="netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51" />
    </assemblies>
</compilation>
```
