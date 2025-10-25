# AuthifyPass API - A Secure, Open, and Privacy-Respectful 2FA Ecosystem

AuthifyPass is an **open-source and public API** designed to simplify secure Two-Factor Authentication (2FA) integration for developers.
It provides a **trusted backend service** to generate and validate TOTP codes (Time-based One-Time Passwords), compatible with standard authenticator apps — while ensuring **total privacy** for end users.

Unlike traditional providers (such as Google or Microsoft Authenticator), AuthifyPass offers a privacy-first approach:
when developers register users in the system, **no personal data or identifiers are ever stored**.
Instead, only **hashed identifiers and shared secrets** are maintained, meaning AuthifyPass has no way to trace real user information.

---

## Why AuthifyPass?
### 1. Public API, Private Data

AuthifyPass operates as a **public authentication service** — developers can register and use it instantly without managing servers or databases.
Even though the API is hosted by AuthifyPass, it never stores sensitive or identifiable information.
Every userId is hashed (SHA256) before being processed, ensuring that no personal or system-related data ever leaves the developer’s control.

### 2. Developer-Focused Security

Each developer or application registers once and receives:

- A **Client ID**
- A **Shared Secret Key**

These credentials are required for every request, providing a secure and authenticated integration model.

### 3. Cross-App Compatibility

AuthifyPass generates **standard TOTP-compatible QR codes**.
They can be read by the **AuthifyPass App** or by any standard authenticator (Google Authenticator, Microsoft Authenticator, etc.).
However, AuthifyPass adds **optional custom parameter**s that enhance validation for those using the AuthifyPass ecosystem.

### 4. Zero Knowledge by Design

- No user data, email, or identifiers are stored.
- All userId values are SHA256-hashed.
- Only your clientId and sharedSecret are stored for validation.

Even if the database were compromised, it would be impossible to trace or reconstruct user identities.

### 5. Enhanced Privacy and Flexibility

Because AuthifyPass handles only anonymized identifiers and pre-hashed values:

- Your system maintains ownership of user identity.
- AuthifyPass acts as a **trusted, stateless verification service**.
- You can use AuthifyPass for both public and enterprise-grade authentication flows without exposing private user data.

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

## AuthifyPass Mobile App

The official **AuthifyPass App** acts as a secure authenticator, fully compatible with QR codes generated by the API.
It can also read standard TOTP QR codes from Google or Microsoft Authenticator.

However, when used with the AuthifyPass API, it provides an **extra security layer**:

- Codes are bound to registered clients.
- Revoking a key from one device invalidates it on all others.
- The app automatically syncs trusted identifiers.

---

## **Presentation Client**
### **1. Home page**
Displays all registered codes.

### **2. Add code**
Allows scanning or manual input of new authentication entries.

### **3. Storage data**
Uses **IndexedDB** (through a repository interface) to securely store authentication data locally. ```IRepository```.

---

## Advantages Over Traditional 2FA

| Feature                 | Google / Microsoft Authenticator | AuthifyPass                                  |
| ----------------------- | -------------------------------- | -------------------------------------------- |
| Open Source             | -                                | +                                            |
| User Data Privacy       | - (App stores labels)            | + (No user data stored)                      |
| Developer Registration  | -                                | +                                            |
| Public API              | -                                | +                                            |
| Client-Based Revocation | -                                | + (is delete from some device ivalidate all) |
| Multi-Device Sync       | + (each device have own store)   | - (each device store his own codes)          |
| TOTP Standard Support   | +                                | +                                            |

---

## License

AuthifyPass is released under the GNU AFFERO GENERAL PUBLIC LICENSE

---

## Contributing

Contributions, feature requests, and bug reports are welcome!
Feel free to open an issue or pull request on GitHub.


