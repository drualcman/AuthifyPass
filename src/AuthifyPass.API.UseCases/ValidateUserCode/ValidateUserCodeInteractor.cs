using AuthifyPass.Entities.Helpers;

namespace AuthifyPass.API.UseCases.ValidateUserCode;
internal class ValidateUserCodeInteractor(
    IUserSecretRepository userRepository,
    IClientRepository clientRepository,
    IStringLocalizer<RegisterUserContent> localizer,
    IModelValidatorHub<ValidateUserCodeDto> validator) : IValidateUserCodeInputPort
{
    public async Task<bool> ValidateUserCode(ValidateUserCodeDto data, string secretShared)
    {
        await GuardModel.AgainstNotValid(validator, data);
        bool result = false;
        var client = await clientRepository.GetByClientIdAsync(data.ClientId);
        ThrowIfNullOrNotValid(secretShared, client);
        var user = await userRepository.GetByClientIdAndUserIdAsync(data.ClientId, data.UserId);
        if (user is not null)
        {
            result = TOTPHelper.ValidateTOTP(data.UserCode, user.ActiveSharedSecret);
        }
        else
            throw new KeyNotFoundException("Invalid User");
        return result;
    }

    private static void ThrowIfNullOrNotValid(string secretShared, Client? client)
    {
        if (client == null)
        {
            throw new KeyNotFoundException("Invalid Client");
        }
        if (!client.SharedSecret.Equals(secretShared))
        {
            throw new KeyNotFoundException("Invalid Client");
        }
    }
}
