//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Pooshineh.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Table_Discounts
    {
        public int DiscountID { get; set; }
        [Display(Name = "کد تخفیف")]
        [Required(ErrorMessage = "فیلد {0} اجباری است.")]
        public string DiscountCode { get; set; }
        [Display(Name = "تخفیف کل")]
        [Required(ErrorMessage = "فیلد {0} اجباری است.")]
        public int TotalDiscount { get; set; }

        public Nullable<int> OrderID { get; set; }

        public virtual Table_Orders Table_Orders { get; set; }
    }
}