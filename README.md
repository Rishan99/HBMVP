# House Broker MVP

## Database Configure
  HouserBrokerMVP>HouseBrokerMVP.API>appsettings.json: Change ConnetionSettings:DefaultConnection as per your sql server hostname and authentication mechanism
  
## Migrations 

**addmigration.bat**
  To add migration, supply migration name. Example in CLI: addmigration first

**dbupdate.bat**
  To updated database with added migrations, Example in CLI: dbupdate

## Endpoints

## Authentication
## `api/auth`

### Login
Authenticate a user by providing valid credentials.

- **Endpoint:** `POST /login`
- **Request Body Type:** `JSON Body`
- **Request Body:**
  - `emailAddress`: Valid Email Addresss
  - `password`: Password of the registered user
- **Response:**
  - Success: `200 OK` with authentication details.
  - Failure: `400 Bad Request` with an error message.

### Register Broker
Register a new broker user.

- **Endpoint:** `POST /register-broker`
- **Request Body Type:** `JSON Body`
- **Request Body:**
  - `emailAddress`: Email Address of broker
  - `password`: Password to be used for registration
  - `confirmPassword`: Confirm Password to be used for registration
  - `phoneNumber`: Phonenumber of broker
- **Response:**
  - Success: `200 OK` with the registered broker's username.
  - Failure: `400 Bad Request` with an error message.


### Change Password
Change the password of the authenticated user.

- **Endpoint:** `POST /change-password`
- **Request Body:**
  - `oldPassword`: Old Password of the user
  - `newPassword`: New Password to be used
- **Authorization Header:**
  - Required for authentication.
- **Response:**
  - Success: `200 OK` with a success message.
  - Failure: `400 Bad Request` with an error message.

### Get My Details
Retrieve details of the authenticated user.

- **Endpoint:** `GET /me`
- **Authorization Header:**
  - Required for authentication.
- **Response:**
  - Success: `200 OK` with user details.
  - Failure: `400 Bad Request` with an error message.



### Property Type
### `api/property-type`

### Get List of Property Types
Retrieve a list of all property types.

- **Endpoint:** `GET /`
- **Authorization Header:**
  - Required for authentication.
- **Response:**
  - Success: `200 OK` with a list of property types.
  - Failure: `400 Bad Request` with an error message.

### Get Property Type by ID
Retrieve details of a specific property type by its ID.

- **Endpoint:** `GET /{id}`
- **Parameters:**
  - `id` (required): ID of the property type.
- **Authorization Header:**
  - Required for authentication.
- **Response:**
  - Success: `200 OK` with property type details.
  - Failure: `400 Bad Request` with an error message.

### Add Property Type
Add a new property type with details.

- **Endpoint:** `POST /`
- **Request Body:**
  - `{"name":""}` JSON object containing property type details.
- **Authorization Header:**
  - Required for authentication.
- **Response:**
  - Success: `200 OK` with the created property type details.
  - Failure: `400 Bad Request` with an error message.

### Update Property Type
Update an existing property type with new details.

- **Endpoint:** `PUT /{id}`
- **Parameters:**
  - `id` (required): ID of the property type to update.
- **Authorization Header:**
  - Required for authentication.
- **Request Body:**
  - `{"id":null,"name":""}` JSON object containing property type details.
- **Response:**
  - Success: `200 OK` with the updated property type details.
  - Failure: `400 Bad Request` with an error message.

### Delete Property Type
Delete a property type by its ID.

- **Endpoint:** `DELETE /{id}`
- **Parameters:**
  - `id` (required): ID of the property type to delete.
- **Response:**
  - Success: `200 OK` with a success message.
  - Failure: `400 Bad Request` with an error message.


### Property 
### `api/property`

### Search Property 
Search for properties based on location, price range, and property type.

- **Endpoint:** `GET /search`
- **Parameters:**
  - `location` (optional): Location to search for properties.
  - `minPrice` (optional): Minimum price of the property.
  - `maxPrice` (optional): Maximum price of the property.
  - `propertyType` (optional): ID of the property type.
- **Response:**
  - Success: 200 OK with a list of properties.
  - Failure: 400 Bad Request with an error message.

### Get List of Properties
Retrieve a list of all properties.

- **Endpoint:** `GET /`
- **Authorization Header:**
  - Required for authentication.
- **Response:**
  - Success: 200 OK with a list of properties.
  - Failure: 400 Bad Request with an error message.

### Get Property by ID
Retrieve details of a specific property by its ID.

- **Endpoint:** `GET /{id}`
- **Parameters:**
  - `id` (required): ID of the property.
  - **Authorization Header:**
  - Required for authentication.
- **Response:**
  - Success: 200 OK with property details.
  - Failure: 400 Bad Request with an error message.

### Add Property
Add a new property with details.

- **Endpoint:** `POST /`
- **Request Body Type:** `Form Data`
- **Request Body:**
  - `Name`: 
  - `price`: 
  - `propertyTypeId`: 
  - `address`: 
  - `description`: 
  - `Images`: Files representing property images.
  - **Authorization Header:**
  - Required for authentication.
- **Response:**
  - Success: 200 OK with the created property details.
  - Failure: 400 Bad Request with an error message.

### Update Property
Update an existing property with new details.

- **Endpoint:** `PUT /{id}`
- **Request Body Type:** `Form Data`
- **Parameters:**
  - `id` (required): ID of the property to update.
  - **Authorization Header:**
  - Required for authentication.
- **Request Body:**
  - `Id`
  - `Name`: 
  - `price`: 
  - `propertyTypeId`: 
  - `address`: 
  - `description`: 
  - `Images`: Files representing property images.
- **Response:**
  - Success: 200 OK with the updated property details.
  - Failure: 400 Bad Request with an error message.

### Delete Property
Delete a property by its ID.

- **Endpoint:** `DELETE /{id}`
- **Parameters:**
  - `id` (required): ID of the property to delete.
  - **Authorization Header:**
  - Required for authentication.
- **Response:**
  - Success: 200 OK with a success message.
  - Failure: 400 Bad Request with an error message.

