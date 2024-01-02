using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pooshineh.Models
{
    public class LoginViewModel
    {
        [Display(Name = "شماره موبایل")]
        [RegularExpression("09[0-9]{9}")]
        [Required(ErrorMessage = "فیلد {0} اجباری است.")]
        public string PhoneNumber { get; set; }
        [Display(Name = "رمز عبور")]
        [DataType(DataType.Password)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "لطفا از کاراکتر های انگلیسی استفاده کنید.")]
        [Required(ErrorMessage = "فیلد {0} اجباری است.")]
        public string Password { get; set; }
    }
}