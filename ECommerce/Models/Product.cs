using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace ECommerce.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "Company")]
        [Index("Product_CompanyId_Description_Index", 1, IsUnique = true)]
        [Index("Product_CompanyId_Barcode_Index", 1, IsUnique = true)]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The Field {0} must be Maximum {1} characters Length")]
        [Index("Product_CompanyId_Description_Index", 2, IsUnique = true)]
        [Display(Name = "Product")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(13, ErrorMessage = "The Field {0} must be Maximum {1} characters Length")]
        [Index("Product_CompanyId_Barcode_Index", 2, IsUnique = true)]
        [Display(Name = "Bar Code")]
        public string Barcode { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "Tax")]
        public int TaxId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "You Must Select a {0} between {1} and {2}")]
        public decimal Price { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        [DataType(DataType.MultilineText)]
        public string Remarks { get; set; }

        public virtual Company Company { get; set; }
        public virtual Category Category { get; set; }
        public virtual Tax Tax { get; set; }
    }
}