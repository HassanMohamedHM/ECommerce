using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The Field {0} must be Maximum {1} characters Length")]
        [Index("Company_Name_Index", IsUnique = true)]
        [Display(Name = "Company")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(20, ErrorMessage = "The Field {0} must be Maximum {1} characters Length")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(100, ErrorMessage = "The Field {0} must be Maximum {1} characters Length")]
        public string Address { get; set; }

        [DataType(DataType.ImageUrl)]
        public string Logo { get; set; }

        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Display(Name = "City")]
        public int CityId { get; set; }

        [NotMapped]
        public HttpPostedFileBase LogoFile { get; set; }

        public virtual Department Department { get; set; }
        public virtual City City { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Tax> Taxes { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}