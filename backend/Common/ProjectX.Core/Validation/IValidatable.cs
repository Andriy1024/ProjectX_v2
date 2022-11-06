namespace ProjectX.Core.Validation;

public interface IValidatable
{
    IEnumerable<ValidationFailure> Validate();
}