# Identity Collection

[![Build status](https://bmcdavid.visualstudio.com/bmcdavid.IdentityCollection/_apis/build/status/bmcdavid.IdentityCollection-ASP.NET%20Core-CI)](https://bmcdavid.visualstudio.com/bmcdavid.IdentityCollection/_build/latest?definitionId=10)

This project contains packages to extend identity claims for Episerver CMS.

* [AzureGraphService](https://github.com/bmcdavid/IdentityCollection/tree/master/src/bmcdavid.AzureGraphService) - provides ability to add Azure Active Directory groups as role claims, included support for nested groups.
* [ClaimsCriteria](https://github.com/bmcdavid/IdentityCollection/tree/master/src/bmcdavid.Episerver.ClaimsCriteria) - provides ability to create Episerver visitor groups criteria enabled as a security group to be applied to a claims identity upon login.
* [SynchronizedProviderExtensions](https://github.com/bmcdavid/IdentityCollection/tree/master/src/bmcdavid.Episerver.SynchronizedProviderExtensions) - provides ability to manually create roles in Episerver admin and assign synchronized users for manual control of ad-hoc groupings.