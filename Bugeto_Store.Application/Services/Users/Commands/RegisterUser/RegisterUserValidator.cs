using FluentValidation;

namespace Bugeto_Store.Application.Services.Users.Commands.RgegisterUser
{
    public class RegisterUserValidator : AbstractValidator<RequestRegisterUserDto>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("نام را وارد نمایید");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("ایمیل را وارد نمایید")
                .EmailAddress()
                .WithMessage("ایمیل معتبر نیست");

            RuleFor(x => x.Tel)
                .NotEmpty()
                .WithMessage("شماره موبایل را وارد نمایید")
                .Matches(@"^09\d{9}$")
                .WithMessage("شماره موبایل معتبر نیست");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("رمز عبور را وارد نمایید")
                .MinimumLength(6)
                .WithMessage("رمز عبور باید حداقل 6 کاراکتر باشد");

            RuleFor(x => x.RePasword)
                .NotEmpty()
                .WithMessage("تکرار رمز عبور را وارد نمایید")
                .Equal(x => x.Password)
                .WithMessage("رمز عبور و تکرار آن برابر نیست");
        }
    }
}
