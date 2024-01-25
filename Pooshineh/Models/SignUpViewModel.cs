using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Pooshineh.Models
{
    public class SignUpViewModel
    {
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "فیلد {0} اجباری است.")]
        [RegularExpression("09[0-9]{9}", ErrorMessage = "فرمت {0} نادرست است")]
        public string PhoneNumber { get; set; }
        [Display(Name = "رمز عبور")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "فیلد {0} اجباری است.")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "لطفا از کاراکتر های انگلیسی استفاده کنید.")]
        public string Password { get; set; }
        [Display(Name = "تکرار رمز عبور")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "فیلد {0} اجباری است.")]
        [Compare("Password", ErrorMessage = "رمز مشابه تکرار رمز نیست.")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "لطفا از کاراکتر های انگلیسی استفاده کنید.")]
        public string PasswordRepeat { get; set; }
    }
}