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
    
    public partial class Table_Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Table_Products()
        {
            this.OrderDetails = new HashSet<OrderDetails>();
            this.OrderDetails1 = new HashSet<OrderDetails>();
            this.Table_CartItem = new HashSet<Table_CartItem>();
        }
    
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImagePath { get; set; }
        public int ProductPrice { get; set; }
        public string ProductQuantity { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails1 { get; set; }
        public virtual Table_Categories Table_Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Table_CartItem> Table_CartItem { get; set; }
    }
}