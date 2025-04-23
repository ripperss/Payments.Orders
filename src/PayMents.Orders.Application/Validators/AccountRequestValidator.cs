using FluentValidation;
using PayMents.Orders.Application.Models.Account;

namespace PayMents.Orders.Application.Validators;

public class AccountRequestValidator : AbstractValidator<AccountRequest>
{
    public AccountRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id не может быть пустым");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email не может быть пустым")
            .EmailAddress()
            .WithMessage("Неверный формат email");

        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Имя пользователя не может быть пустым")
            .MinimumLength(3)
            .WithMessage("Имя пользователя должно содержать минимум 3 символа")
            .MaximumLength(50)
            .WithMessage("Имя пользователя не должно превышать 50 символов");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Телефон не может быть пустым")
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Неверный формат телефона");
    }
} 