using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pooshineh.Models
{
    public class DiscountViewModel
    {
        [Display(Name = "کد تخفیف")]
        public string DiscountCode { get; set; }
    }
}