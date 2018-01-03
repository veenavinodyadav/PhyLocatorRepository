using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PhysicianLocator.Models
{
    public class HealthGridModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HealthId { get; set; }
               
        public string Type { get; set; }
      
        public string Description { get; set; }
        [Display(Name = "Document")]
        public string filename { get; set; }

        [Required(ErrorMessage = "This field can not be empty.")]
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yy}")]
        public DateTime StartDate { get; set; }
   
        [Required(ErrorMessage = "This field can not be empty.")]
        [Display(Name = " End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yy}")]
        public DateTime EndDate { get; set; }
    }
}