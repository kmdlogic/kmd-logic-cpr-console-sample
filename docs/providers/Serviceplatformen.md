## Serviceplatformen provider
Serviceplatformen primarily exhibits personal data. It is the responsibility of the municipality to ensure that this data is processed in accordance with 
current legislation and thus the municipality's responsibility to ensure that via the Service Platform only access to the necessary personal data is given 
for a specific IT system.

For CPR purposes Logic connects to 'PersonBaseDataExtended' service [(LINK)](https://www.serviceplatformen.dk/administration/serviceOverview/show?uuid=e6be2436-bf35-4df2-83fe-925142825dc2). 
Before Logic connection, client must firstly send the request for service agreement at STS Administration portal at specific environment (Test or Production). 
It must be done by person who has MOCES certificate within KMD. When service agreement is approved, the client must create the configuration at Logic Console.

Logic CPR configuration parameters for Serviceplatform:
- Name - your custom name, that identifies specific configuration within all resources
- Certificate & Password - `p12` certificate, which has been uploaded during configuration of IT-Service at STS Administration portal; 
... and password for this certificate
- Municipality CVR - CVR of the danish municipality that you request Serviceplatform via Logic on behalf of

Useful links:
1. General technical documentation - https://www.serviceplatformen.dk/administration/help/faq
2. More specific documentation files - https://share-komm.kombit.dk/P133/Ibrugtagning%20og%20test/Delte%20dokumenter/Forms/Vejledninger%20%20Serviceplatformen.aspx