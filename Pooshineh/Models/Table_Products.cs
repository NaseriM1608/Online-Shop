namespace Pooshineh.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Table_Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Table_Products()
        {
            this.Table_CartItem = new HashSet<Table_CartItem>();
            this.Table_ProductColor = new HashSet<Table_ProductColor>();
        }

        [Display(Name = "آیدی محصول")]
        public int ProductID { get; set; }
        [Display(Name = "آیدی دسته‌بندی")]
        public int CategoryID { get; set; }
        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "فیلد {0} اجباری است.")]
        public string ProductName { get; set; }
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "فیلد {0} اجباری است.")]
        public string ProductDescription { get; set; }
        [Display(Name = "تصویر محصول")]
        [Required(ErrorMessage = "فیلد {0} اجباری است.")]
        public string ProductImagePath { get; set; }
        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "فیلد {0} اجباری است.")]
        public int ProductPrice { get; set; }
        [Display(Name = "موجودی")]
        [Required(ErrorMessage = "فیلد {0} اجباری است.")]
        public int ProductQuantity { get; set; }
        [Display(Name = "برند")]
        [Required(ErrorMessage = "فیلد {0} اجباری است.")]
        public string ProductBrand { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Table_CartItem> Table_CartItem { get; set; }
        public virtual Table_Categories Table_Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Table_ProductColor> Table_ProductColor { get; set; }
    }
}
