namespace FitCore.Application.Services.Users.Commands.RegisterUser
{

    public partial class RegisterUserService
    {
        public class RegisterUserRequest
        {
            public string FullName { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }

            public string RePassword { get; set; }

            public long GymId { get; set; }
        }

    }
}
