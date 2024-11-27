# AuthifyPass API

AuthifyPass is an API system designed to manage users 2FA, providing functionality for client registration, user code validation. The project is built with .NET 9 and ASP.NET Core using Minimal APIs.

---

## **Project Structure**

The solution is organized into multiple projects to ensure separation of concerns, enhancing maintainability and scalability. Below is a detailed breakdown of each component:

### **1. Application**
Contains use cases and business logic. It is divided into:
- **AuthifyPass.API.Core**: Includes interfaces, DTOs, and exception handlers to streamline component integration.
- **AuthifyPass.API.UseCases**: Implements the use cases defined in the core interfaces, encapsulating the primary workflow logic.

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

### **5. Presentation**
Encapsulates endpoints and configurations related to API presentation and access.
- **AuthifyPass.API**: The main project containing endpoints and ASP.NET Core pipeline configuration.
- **AuthifyPass.API.IoC**: Dependency injection configuration.

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
    - **Response**:
      ```json
      "string"
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