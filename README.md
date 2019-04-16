# KMD Logic CPR Console Sample
This is a simple console application to demonstrate how to call Logic CPR API.

Sample calls `/subscriptions/{subscriptionId}/cpr/by-cpr/{cpr}` endpoint.

## Usage
1. Configure the `appsettings.json` with your KMD Logic subscription settings.
   - **LogicAccount**
     - **SubscriptionId** - get your subscription at https://console.kmdlogic.io/subscriptions
     - **ClientId** and **ClientSecret** - get your client credentials at https://console.kmdlogic.io/subscriptions/{subscriptionId}/client-credentials
   - **Cpr**
     - **ConfigurationId** - get your CPR service configuration at https://console.kmdlogic.io/cpr/configurations
2. [Install dotnet core](https://dotnet.microsoft.com/download)
3. Execute `dotnet run` in the root project folder.

As a success you should able to display sample data of _Citizen_ for a test CPR number.
