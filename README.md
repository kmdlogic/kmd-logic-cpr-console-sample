# KMD Logic CPR Client

A dotnet client library for accessing the Danish CPR register via the Logic platform.

## How to use this client library

In projects or components where you need to access the register, add a NuGet package reference to [Kmd.Logic.Cpr.Client](https://www.nuget.org/packages/Kmd.Logic.Cpr.Client).

The simplest example to get a citizens details is:

```csharp
using (var httpClient = new HttpClient())
using (var tokenProviderFactory = new LogicTokenProviderFactory(configuration.TokenProvider))
{
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
    "ClientSecret": "",
    "AuthorizationScope": ""
  },
  "Cpr": {
    "SubscriptionId": "",
    "CprConfigurationId": ""
  }
}
```

To get started:

1. Create a subscription in [Logic Console](https://console.kmdlogic.io). This will provide you the `SubscriptionId`.
2. Request a client credential. Once issued you can view the `ClientId`, `ClientSecret` and `AuthorizationScope` in [Logic Console](https://console.kmdlogic.io).
3. Create a CPR configuration. Select the CPR provider you have an agreement with and upload the access certificate. If you haven't done this already, you can begin testing with the Fake CPR Provider. This will give you the `CprConfigurationId`.

## Sample application

A simple console application is included to demonstrate how to call Logic CPR API. You will need to provide the settings described above in `appsettings.json`.

When run you should see the details of the _Citizen_ for the nominated CPR number is printed to the console.

## Datafordeler Provider

The Datafordeler service is available to any organisation which require access to the CPR register.

To gain access, you must:

1. Create a user in the Self Service Portal
2. Add a Service User by supplying a FOCES certificate
3. Request access to the CPR Service `CprPersonFullComplete`
4. Optionally, request access to CPR Events `CprHaendelse` and configure a corresponding Event subscription (Hændelser). 

Useful links:

1. [Datafordeler Website](https://datafordeler.dk)
2. [Self Service Portal (Production)](https://selfservice.datafordeler.dk)
3. [Self Service Portal (Test)](https://test04-selfservice.datafordeler.dk)
4. [Requesting Access](https://datafordeler.dk/vejledning/brugeradgang/anmodning-om-adgang/det-centrale-personregister-cpr/)
5. [CPR Service Details](https://datafordeler.dk/dataoversigt/det-centrale-personregister-cpr/cprpersonfullcomplete/)

## Service Platform Provider

The Service Platform provider is for exclusive use by municipalities.

For CPR purposes Logic connects to [PersonBaseDataExtended](https://www.serviceplatformen.dk/administration/serviceOverview/show?uuid=e6be2436-bf35-4df2-83fe-925142825dc2) service.

To gain access, a user with a MOCES certificate must send the request for a Service Agreement in the STS Administration portal for the required environment (Test or Production).

The process of Service Agreement approval can be sometimes accelerated up by sending e-mail to the Service Platform Help-desk, including service agreement UUID. When service agreement is approved, the client must create the configuration at Logic Console.

Logic CPR configuration parameters for Service Platform:

- Name: your customer name in Service Platform which identifies specific configuration within all resources
- Certificate: The `p12` certificate which has been uploaded during configuration of IT-Service at STS Administration portal
- Municipality CVR: The CVR of the municipality that you will be requesting access to Service Platform on behalf of

Useful links:

1. [Service Platform (Production)](https://www.serviceplatformen.dk)
2. [Service Platform (Test)](https://exttestwww.serviceplatformen.dk)
3. [STS Administration Portal](https://www.serviceplatformen.dk/administration/dashboard/outerpage?page=sts)
4. [General technical documentation](https://www.serviceplatformen.dk/administration/help/faq)
5. [More specific documentation files](https://share-komm.kombit.dk/P133/Ibrugtagning%20og%20test/Delte%20dokumenter/Forms/Vejledninger%20%20Serviceplatformen.aspx)
6. [Service Platform Help-desk](mailto:helpdesk@serviceplatformen.dk)

## CPR Fake Provider

The Fake Provider is a great solution for use in Demo or Test environments and also allows you to begin development immediately whilst you wait for your formal credentials.

The Fake Provider will return well-described test data for a large number of CPR numbers and identifiers. These can be viewed in the [fake](./fake) folder of this repository. This includes the [CPR test data set](https://cprservicedesk.atlassian.net/wiki/spaces/CPR/pages/11436127/Testdata) plus additional examples that have been requested by developers.

If not one of the well-described tests, the Fake Provider exhibits the following behaviour:

- The CPR number must be 10 digits long, with the first 6 digits being a valid `ddMMyy` formatted date
- If the CPR number ends in `000`, `001` or `002` it returns NotFound
- The Fake provider returns random data, using the CPR number as the seed. This ensures recurring calls return the same response

## CPR Subscription

When ever there is a change in Data of Citizen, The respsective provider will raise events.
 
Example -
 [Datafordeler (Events)](https://confluence.datafordeler.dk/pages/viewpage.action?pageId=17137834#H%C3%A6ndelserp%C3%A5Datafordeleren-Brugsscenarier)
 
- Subscribe To The Events By CPR number
In Order to recieve events, you must subscribe with the `SubscriptionId` and `ConfigurationId` and the `CPR number` for which the events needs to be received.
 
- Subscribe To The Events By PersonId
To recieve events, you must subscribe with the `SubscriptionId` and `ConfigurationId` and the `CPR Person ID` for which the events needs to be received.
 
- Unsubscribe By CPR number
To Stop receiveing the events, you must unsubscribe the previously created subscription either with `CPR number` or `CPR Person ID`.
 
- Get Events
To generate these events for which you have subscibed, use `Get Events` along with your `SubscriptionId` and `ConfigurationId` along with the desired time period.

NOTE: While every attempt is made to keep the generated random data consistent, this is **not guaranteed**. If you need a reliable response, please use a well-known test or request for a suitable one to be added.

When requesting CPR details by id, the same process applies. If it is not one of the well-described test identifiers then the id must be in the format "fa4e2c`<CPR number>`fa4e2c`<CPR number>`". For example, the CPR `0301821005` has a corresponding id of `fa4e2c03-0182-1005-fa4e-2c0301821005`. All other identifiers will return NotFound.
