# PaylocityCodingChallenge

Calculates benefit costs for Employee plus dependents.  
Per the specification, applies a discount if Employee name or Dependent name starts with the letter "A" (case-insensitive)

Frontend - Blazor (Web Assembly)

Backend - Asp.Net Core WebAPI with seperate BusinessLogic and DataAccess layers.

Possible Improvements
  
  Move data (benefit costs, discount, covered types) to a database
  Allow for other Dependent types such as Domestic Partner
  Unit tests on front end
  More API routes to allow retrieval of covered types on the front end
