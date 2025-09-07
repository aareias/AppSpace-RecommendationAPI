namespace API.Validation;

public class ValidatableObject<T>
{
    public ValidatableObject()
    {
        IsValid = true;
        Errors = [];
    }

    public List<IValidationRule<T>> Validations { get; } = new();

    public IEnumerable<string> Errors { get; private set; }

    public T Value { get; set; }

    public bool IsValid { get; private set; }

    public bool Validate()
    {
        Errors = Validations
                     ?.Where(v => !v.Check(Value))
                     ?.Select(v => v.ValidationMessage)
                     ?.ToArray()
                 ?? Enumerable.Empty<string>();

        IsValid = !Errors.Any();

        return IsValid;
    }
}