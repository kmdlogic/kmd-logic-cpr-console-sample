# KMD Logic CPR Client

A dotnet client library for accessing the Danish CPR register via the Logic platform.

## How to use this client library

In projects or components where you need to access the register, add a NuGet package reference to [Kmd.Logic.Cpr.Client](https://www.nuget.org/packages/Kmd.Logic.Cpr.Client).

The simplest example to get a citizens details is:

```csharp
using (var httpClient = new HttpClient())
{
    var tokenProviderFactory = new LogicTokenProviderFactory(configuration.TokenProvider);
    var cprClient = new CprClient(httpClient, tokenProviderFactory, configuration.Cpr);
    var citizen = await cprClient.GetCitizenByCprAsync(configuration.CprNumber).ConfigureAwait(false);
}
```

The `LogicTokenProviderFactory` authorizes access to the Logic platform through the use of a Logic Identity issued client credential. The authorization token is reused until it  expires. You would generally create a single instance of `LogicTokenProviderFactory`.

The `CprClient` accesses the Logic CPR service which in turn interacts with one of the data providers.

## How to configure the CPR client

Perhaps the easiest way to configure the CPR client is from Application Settings.

```json
{
  "TokenProvider": {
    "ClientId": "",
    "ClientSecret": ""
  },
  "Cpr": {
    "SubscriptionId": "",
    "CprConfigurationId": ""
  }
}
```

To get started:

1. Create a subscription in [Logic Console](https://console.kmdlogic.io). This will provide you the `SubscriptionId`.
2. Request a client credential. Once issued you can view the `ClientId` and `ClientSecret` in [Logic Console](https://console.kmdlogic.io).
3. Create a CPR configuration. Select the CPR provider you have an agreement with and upload the access certificate. If you haven't done this already, you can begin testing with the Fake CPR Provider. This will give you the `CprConfigurationId`.

## Sample application

A simple console application is included to demonstrate how to call Logic CPR API. You will need to provide the settings described above in `appsettings.json`.

When run you should see the details of the _Citizen_ for the nominated CPR number printed to the console.

## CPR Fake Provider

The following citizens are known by the Fake Provider:

- Case #1 _(will generate real data taken from Datafolderer test environment)_:
  - CPR: 0101015084
  - ID: 5e3d9df6-d082-467a-88bd-dca56edc7328
- Case #2 _(will generate random data)_:
  - CPR: 1802860028
  - ID: 9eb934ac-3680-465a-96ce-40f3f24f9514
- Case #3 _(will generate random data)_:
  - CPR: 0101774481
  - ID: cf1b281d-2fb6-4ee2-b602-e3468f90f5b6
- Case #4 _(will generate random data)_:
  - CPR: 2712990093
  - ID: e6478bde-1c0a-4d1a-91a0-ceb6bea709f6

Any other CPR or ID provided to the Fake Provider will give you `404 NotFound` if the Logic service is called directly or `null` from the CprClient.
