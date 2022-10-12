using FluentValidation;

namespace ProjectX.Core.Validation;

public class DynamicValidator<T> : AbstractValidator<T>
{
    public DynamicValidator(Action<DynamicValidator<T>> configuration)
    {
        configuration(this);
    }
}