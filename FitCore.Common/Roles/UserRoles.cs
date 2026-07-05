namespace FitCore.Common.Roles
{
    public class UserRoles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string Trainer = "Trainer";
        public const string Member = "Member";
        public const string Operator = "Operator";
        public const string Customer = "Customer";

        public const string AdminHigher = "SuperAdmin,Admin";
        public const string Staff = "SuperAdmin,Admin,Operator";

    }
}
