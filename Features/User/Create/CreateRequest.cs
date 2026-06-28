using FastEndpoints;
using FluentValidation;

namespace bank.Features.User.Create;

public record CreateUserRequest(string Name, string Gmail);

public class CreateUserValidator : Validator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên không được để trống");
        RuleFor(x => x.Gmail)
            .NotEmpty().WithMessage("Gmail không được để trống")
            .EmailAddress().WithMessage("Sai định dạng Gmail");
    }
}
