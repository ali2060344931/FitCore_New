using FitCore.Application.Services.Users.Commands.RegisterUser;

using FluentValidation;

using static FitCore.Application.Services.Users.Commands.RegisterUser.RegisterUserService;

public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("نام و نام خانوادگی الزامی است")
            .MinimumLength(3)
            .WithMessage("نام باید حداقل 3 کاراکتر باشد");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("ایمیل الزامی است")
            .EmailAddress()
            .WithMessage("فرمت ایمیل صحیح نیست");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("رمز عبور الزامی است")
            .MinimumLength(6)
            .WithMessage("رمز عبور باید حداقل 6 کاراکتر باشد");

        RuleFor(x => x.RePassword)
            .Equal(x => x.Password)
            .WithMessage("رمز عبور و تکرار آن یکسان نیست");
    }
}
