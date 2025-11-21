[![Nuget](https://img.shields.io/nuget/v/AuthifyPass.Client?style=for-the-badge)](https://www.nuget.org/packages/AuthifyPass.Client)
[![Nuget](https://img.shields.io/nuget/dt/AuthifyPass.Client?style=for-the-badge)](https://www.nuget.org/packages/AuthifyPass.Client)

# AuthifyPass.Client Library Documentation

## Overview

The `AuthifyPass.Client` library to consume AuthifyPass API.

## Prerequisites

To use this library, you need:

- You must register first in [AuthifyPass here](https://authifypass.com/register/) to get your CLIENT and SECRET.


## Installation

1. **Install the NuGet Package**:
   - Install the `AuthifyPass.Client` NuGet package in your project using the Package Manager or the .NET CLI:
     ```bash
     dotnet add package AuthifyPass.Client
     ```

2. **Configure Client**:
    - In your appsettings, add
    ```json
      "AuthifyPassOptions": {
        "ClientID": "",
        "Secret": "",
        "BaseUrl": "https://authifypass.com/",
        "UserEndpoint": "user",
        "ValidateCodeEndpoint": "user/validate-code",
        "Header": "x-authify-key"
      }
    ```
    - NOTE: If AuthifyPass API it's in other URL or change the endpoints, here is where can set. Also the Header key.
    - NOTE: Because AuthifyPass is opensource, you can download the code and personalize, so here can change the values.

3. **Register dependencies**:
    - Add the `IAuthifyPassClient` to your dependencies.
   ```csharp
   var builder = WebApplication.CreateBuilder(args); // OR any other like var builder = WebAssemblyHostBuilder.CreateDefault(args);
   [...]
   builder.Services.AddAuthifyPassClient();
   [...]
   ```

## How to use
### Create new QR
```csharp
internal class Enable2FactorQRInteractor(IAuthifyPassClient client)
{
    public async Task<AuthifyPassDto> GetQR(string userId, CancellationToken token)
    {
        var response = await client.RequestUser2FactorQRAsync(userId, user.Email, token);
        var result = new AuthifyPassDto(response.Sharedkey, response.ImageSVG);
        result.Code = userId;
        return result;
    }
}
```
### Login example
```csharp
internal class UserLoginInteractor(
    IRepository<User> repository,
    IValidator<UserRequest> validator,
    IAuthifyPassClient client)
{
    public async Task Login(UserRequest login)
    {
        bool isValidated = await validator.Validate(login);
        if (isValidated)
        {
            User? user = await repository.GetAsync(u => u.Email == login.Email);
            if (user is not null && user.PasswordHash == login.Password)
            {
                var result2Factor = await client.ValidateUser2FactorAsync(user.Id.ToString(), login.DoubleFactorCode);
                if (!result2Factor)
                    throw new UnauthorizedAccessException("Two-factor authentication is enabled for this user.");
            }
        }
        throw new UnauthorizedAccessException("Invalid email or password.");
    }
}
```
### Delete User and all QR
```csharp
internal class DeleteFactorQRInteractor(IAuthifyPassClient client)
{
    public async Task DeleteUserQR(string userId, CancellationToken token)
    {
        await client.RequestDeleteUser2FactorAsync(userId, token);
    }
}
```
#### NOTES:
- UserId It's a string, can send any identityies. This identifier it's HASH in AuthifyPass API Side, so we don't have any data from user.
- When delete a User, delete all QR and no more validation will happen.
- User Client App can also delete the QR and invalidate all QR generated.

### Errors
When have some error client will return a `ProblemDetails` model.


## Models

The library includes two main models:

- **`AuthifyPassOptions`**:
  - Represents the configuration about AuthifyPass API. Check documentation `https://authifypass.com/api/docs`.
  - Properties:
    - SectionKey
    - ClientID
    - Secret
    - BaseUrl
    - UserEndpoint
    - ValidateCodeEndpoint
    - Header
  

- **`AuthifyPassResponse`**:
  - Represents the response when request for some new QR and Shared Key.
  - Properties:
    - Sharedkey
    - ImageSVG: QR

## Contributing

If you encounter issues or have suggestions for improvements, please submit an issue or pull request to the repository hosting this library.