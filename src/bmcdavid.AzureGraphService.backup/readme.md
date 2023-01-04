# Azure Graph Service

```cs

      // Enable federated authentication 
      app.UseWsFederationAuthentication(new WsFederationAuthenticationOptions
      {
        MetadataAddress = _azureGraphServiceOptions.MetadataAddress,
        Wtrealm = _azureGraphServiceOptions.WtRealm,
        Notifications = new WsFederationAuthenticationNotifications
        {
          RedirectToIdentityProvider = ctx =>
          {
            //  To avoid a redirect loop to the federation server send 403 when user is authenticated but does not have access 
            if (ctx.OwinContext.Response.StatusCode == 401 && ctx.OwinContext.Authentication.User.Identity.IsAuthenticated)
            {
              ctx.OwinContext.Response.StatusCode = 403;
              ctx.HandleResponse();
            }

            return Task.FromResult(0);
          },
          SecurityTokenValidated = async ctx =>
          {
            // Ignore scheme/host name in redirect Uri to make sure a redirect to HTTPS does not redirect back to HTTP 
            var redirectUri = new Uri(ctx.AuthenticationTicket.Properties.RedirectUri, UriKind.RelativeOrAbsolute);

            if (redirectUri.IsAbsoluteUri)
            {
              ctx.AuthenticationTicket.Properties.RedirectUri = redirectUri.PathAndQuery;
            }

            // NOTE: Here is an example of where this package can read in nested Azure groups to roles for given Identity
            await ServiceLocator.Current.GetInstance<AzureGraphService>().AddGroupsAsRoleClaimsAsync(ctx.AuthenticationTicket.Identity);

            ctx.AuthenticationTicket = new AuthenticationTicket
            (
              new CustomClaimsIdentity(ctx.AuthenticationTicket.Identity),
              ctx.AuthenticationTicket.Properties
            );

            // Sync user and the roles to EPiServer in the background
            await _syncUserServiceFactory().SynchronizeAsync(ctx.AuthenticationTicket.Identity);
          }
        }
      });
```