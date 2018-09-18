using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
    public class Tax
    {
        [Key]
        public int TaxId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The Field {0} must be Maximum {1} characters Length")]
        [Index("Tax_CompanyId_Description_Index", 2, IsUnique = true)]
        [Display(Name = "Tax")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [DisplayFormat(DataFormatString ="{0:P2}",ApplyFormatInEditMode =false)]
        [Range(0,1,ErrorMessage ="You Must Select a {0} between {1} and {2}")]
        public double Rate { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name = "Company")]
        [Index("Tax_CompanyId_Description_Index", 1, IsUnique = true)]
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}