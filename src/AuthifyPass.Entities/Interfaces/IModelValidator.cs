using AuthifyPass.Entities.Enums;
using AuthifyPass.Entities.ValueObjects;

namespace AuthifyPass.Entities.Interfaces;
public interface IModelValidator<T>
{
    ValidationConstraint Constraint { get; }
    IEnumerable<ValidationError> Errors { get; }
    Task<bool> Validate(T model);
}
