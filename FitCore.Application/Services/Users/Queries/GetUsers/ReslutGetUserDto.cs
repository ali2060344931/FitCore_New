using System.Collections.Generic;

namespace FitCore.Application.Services.Users.Queries.GetUsers
{
    public class ReslutGetUserDto
    {
        public List<GetUsersDto> Users { get; set; }
        public int Rows { get; set; }
        public int TotalRows { get; set; }


    }
}
