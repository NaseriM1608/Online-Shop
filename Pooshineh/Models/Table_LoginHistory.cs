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
    using System.Resources;

    public partial class Table_LoginHistory
    {
        public int LoginID { get; set; }
        [Display(Name = "تاریخ ورود")]
        [DisplayFormat(DataFormatString = "{0: dddd، dd MMMM yyyy، HH:mm}")]
        public System.DateTime LoginDate { get; set; }
        public int UserID { get; set; }

        public virtual Table_User Table_User { get; set; }
    }
}