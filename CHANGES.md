1.4.4
Spouse and Namemarking properties are updated in citizen detailed response.

1.4.3
* Added middle name property in response

1.4.2
* Added care of address property in response

1.4.1
* Added new properties in CPR detailed response and Address 
 - Properties included in CPR detailed response are Addressing name, Position, Sex, Date Of Birth,Date of birth uncertainitymarking,Birth registration locationcode, Birth registration locationName, Supplemental birth registartion place, From marrieddate, From married uncertainitymarking, Status date, Status date uncertainitymarking, Children
 - Properties included in Address are Moved to, Moved away, Is current.

1.4.0
* New methods to provide detailed cpr
 - Citizen details by cpr
 - Citizen details by Cpr id

1.3.1
* Updated Id in Citizen response class and ActualCount in SubscribedCitizenEvents response class with required field attributes

1.3.0
* Included new method GetSubscribedCprEvents

1.2.0
* Updated CprSubscription to subscribe and unsubscribe for CPR events,it includes these methods
 - Subscribe To The Events By CPR number
 - Subscribe To The Events By PersonId
 - Unsubscribe By CPR number
 - Unsubscribe By PersonId
 - Get Events

1.1.0
* Updated the Kmd.Logic.Identity.Authorization dependency

1.0.2
* Updated CprConfigurationException to return the underlying error message. The InnerMessage property is now obsolete.

1.0.1
* Updated dependencies and moved build to dotnet core 3.1
* Added information about the Service Platform and Datafordeler Providers

1.0.0
* Created a nuget package

0.1.0
* Initial version
