# KMD Logic CPR Client

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

## CPR Fake Provider

In case that you do not want to test our service with real CPR providers you could use the Fake Provider.

### Usage

1. Configure CPR Fake Provider at [Logic Console](https://console.kmdlogic.io/cpr/configurations).
2. Use one of below CPR or ID. **Allowed CPR List**:
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
3. Any other CPR or ID provided to the Fake Provider will give you a 404 result.