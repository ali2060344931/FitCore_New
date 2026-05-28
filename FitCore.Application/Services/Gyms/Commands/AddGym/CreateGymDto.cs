using System;
using System.ComponentModel.DataAnnotations;

namespace FitCore.Application.ViewModels.Gyms
{
    public class CreateGymDto
    {
        [Display(Name = "نام مدیر باشگاه")]
        [Required(ErrorMessage = "نام مدیر باشگاه الزامی است")]
        [MaxLength(200)]
        public string FullName { get; set; }
        
        
        [Display(Name = "نام باشگاه")]
        [Required(ErrorMessage = "نام باشگاه الزامی است")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Display(Name = "کد یکتای باشگاه")]
        [Required(ErrorMessage = "کد باشگاه الزامی است")]
        [MaxLength(50)]
        public string Code { get; set; }


        [Display(Name = "توضیحات")]
        [MaxLength(1000)]
        public string Description { get; set; }


        [Required(ErrorMessage = "شماره همراه الزامی است")]
        [Display(Name = "شماره موبایل")]
        [MaxLength(20)]
        public string MobileNumber { get; set; }

        
        
    }
}
