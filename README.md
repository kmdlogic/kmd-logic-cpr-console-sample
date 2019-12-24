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

When run you should see the details of the _Citizen_ for the nominated CPR number is printed to the console.

## CPR Fake Provider

The Fake Provider is a great solution for use in Demo or Test environments and also allows you to begin development immediately whilst you wait for your formal credentials.

The Fake Provider will return well-described test data for a large number of CPR numbers and identifiers. These can be viewed in the [fake](./fake) folder of this repository. This includes the [CPR test data set](https://cprservicedesk.atlassian.net/wiki/spaces/CPR/pages/11436127/Testdata) plus additional examples that have been requested by developers.

If not one of the well-described tests, the Fake Provider exhibits the following behaviour:

- The CPR number must be 10 digits long, with the first 6 digits being a valid `ddMMyy` formatted date
- If the CPR number ends in `000`, `001` or `002` it returns NotFound
- The Fake provider returns random data, using the CPR number as the seed. This ensures recurring calls return the same response

NOTE: While every attempt is made to keep the generated random data consistent, this is **not guaranteed**. If you need a reliable response, please use a well-known test or request for a suitable one to be added.

When requesting CPR details by id, the same process applies. If it is not one of the well-described test identifiers then the id must be in the format "fa4e2c`<CPR number>`fa4e2c`<CPR number>`". For example, the CPR `0301821005` has a corresponding id of `fa4e2c03-0182-1005-fa4e-2c0301821005`. All other identifiers will return NotFound.
