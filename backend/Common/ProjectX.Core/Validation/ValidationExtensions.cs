using FluentValidation;

namespace ProjectX.Core.Validation;

public static class ValidationExtensions
{
    public static void ThrowIfInvalid<TObject>(this TObject obj, IEnumerable<IValidator<TObject>> validators)
    {
        var context = new ValidationContext<TObject>(obj);

        var failures = validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x != null)
            .ToList();

        if (failures.Count != 0)
        {
            var stringErrors = failures.Select(x => $"{Environment.NewLine} -- {x.PropertyName}: {x.ErrorMessage}");

            //TODO: Create Custom Validation Exception
            throw new InvalidDataException("Validation failed: " + string.Join(string.Empty, stringErrors));
        }
    }

    public static void ThrowIfInvalid<TObject>(this TObject obj, Action<DynamicValidator<TObject>> configuration)
    {
        obj.ThrowIfInvalid(new[] { new DynamicValidator<TObject>(configuration) });
    }
}