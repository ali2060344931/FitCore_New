using Microsoft.AspNetCore.Mvc.Rendering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Auth
{
    public class RegisterUserViewModel
    {
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; }

        public long GymId { get; set; }

        public List<SelectListItem> Gyms { get; set; }


    }
}
