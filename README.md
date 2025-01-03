# AuthifyPass API

AuthifyPass is an API system designed to manage users 2FA, providing functionality for client registration, user code validation. The project is built with .NET 9 and ASP.NET Core using Minimal APIs.

---

## **Project Structure**

The solution is organized into multiple projects to ensure separation of concerns, enhancing maintainability and scalability. Below is a detailed breakdown of each component:

### **1. Application**
Contains use cases and business logic. It is divided into:
- **AuthifyPass.API.Core**: Includes interfaces, DTOs, and exception handlers to streamline component integration.
- **AuthifyPass.API.UseCases**: Implements the use cases defined in the core interfaces, encapsulating the primary workflow logic.
- **AuthifyPass.Client.Core**: Implements interfaces and QRDecor and TwoFactorCode handlers.

### **2. Domain**
Defines the core models and entities of the system.
- **AuthifyPass.Entities**: Includes the main DTOs used to transfer data between endpoints and application layers.

### **3. Frameworks**
Contains framework-specific dependencies interacting with external systems.
- **AuthifyPass.API.DataBaseContext.EF**: Database context implementation using Entity Framework.
- **AuthifyPass.FluentValidator**: Validator to ensure data consistency and integrity.

### **4. Infrastructure**
Provides access to repositories and views necessary for data persistence.
- **AuthifyPass.API.Repositories**: Implements repository patterns to handle data storage interactions.
- **AuthifyPass.Views**: Implements all the components to render Razor pages and Razor componets.

### **5. Presentation**
Encapsulates endpoints and configurations related to API presentation and access.
- **AuthifyPass.API**: The main project containing endpoints and ASP.NET Core pipeline configuration.
- **AuthifyPass.API.IoC**: Dependency injection configuration.
- **AuthifyPass.Client**: Blazor Web Assembly Client.

---

## **Endpoints**

### **1. Client Endpoints**
- **Base Route**: `/client`
- **Description**: Provides functionality related to client management.
- **Endpoints**:
  - **POST `/client`**: Create account and get ClientId and Shared Secret
    - **Request**: 
      ```json
      {
        "name": "string",
        "email": "string",
        "password": "string"
      }
      ```
    - **Response**: 
      ```json
      {
        "clientId": "string",
        "sharedSecret": "string",
        "message": "string"
      }
      ```
  - **DELETE `/client/{id}`**: Delete all data relative to the Client Id
    - **Headers**:
      - `x-authify-key`: Shared Secret authentication key
    - **Response**: No content 

### **2. User Endpoints**
- **Base Route**: `/user`
- **Description**: Manages users associated with clients.
- **Endpoints**:
  - **POST `/user`**: Create User Shared Key
    - **Request**:
      ```json
      {
        "clientId": "string",
        "userId": "string"
      }
      ```
    - **Headers**:
      - `x-authify-key`: Shared Secret authentication key.
    - **Response**: Image SVG it's a QR with QRDataDto in JSON format
      ```json
      {
        "sharedkey": "string",
        "imageSVG": "string"
      }
      ```

  - **POST `/user/validate-code`**:
    - **Request**:
      ```json
      {
        "clientId": "string",
        "userId": "string",
        "userCode": "string"
      }
      ```
    - **Headers**:
      - `x-authify-key`: Shared Secret authentication key.
    - **Response**:
      ```json
      true | false
      ```
  - **DELETE `/client/{id}`**: Delete all data relative to the Client Id
    - **Headers**:
      - `x-authify-key`: Shared Secret authentication key.
    - **Response**: No content 
      ```
---    
#### QRDataDto
This is the object with the data inside the QR
```json
{
    "name": "string",
    "clientId": "string",
    "sharedKey": "string"
}
```
---

## **Presentation Client**
### **1. Home page**
Shows the list of the registered page codes.

### **2. Add code**
Read the QR or add manually information to add new page code.

### **3. Storage data**
Using right now indexedDb but depends of the implementation about ```IRepository```.