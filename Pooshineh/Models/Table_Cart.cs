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
    
    public partial class Table_Cart
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Table_Cart()
        {
            this.Table_CartItem = new HashSet<Table_CartItem>();
        }
    
        public int CartID { get; set; }
        public int UserID { get; set; }
        public int TotalCost { get; set; }
    
        public virtual Table_User Table_User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Table_CartItem> Table_CartItem { get; set; }
    }
}
