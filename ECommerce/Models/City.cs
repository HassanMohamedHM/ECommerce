using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ECommerce.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        [MaxLength(50, ErrorMessage = "The Field {0} must be Maximum {1} characters Length")]
        [Index("City_Name_Index",2,IsUnique =true)]
        [Display(Name = "City")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The field {0} is required")]
        [Display(Name ="Department")]
        [Index("City_Name_Index", 1, IsUnique = true)]
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<User>Users { get; set; }
       
    }
}