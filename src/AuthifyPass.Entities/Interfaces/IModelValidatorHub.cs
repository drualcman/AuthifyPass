using AuthifyPass.Entities.ValueObjects;

namespace AuthifyPass.Entities.Interfaces;
public interface IModelValidatorHub<T>
{
    IEnumerable<ValidationError> Errors { get; }
    Task<bool> Validate(T model);
}
