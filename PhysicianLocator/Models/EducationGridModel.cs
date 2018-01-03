using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhysicianLocator.Models
{
    public class EducationGridModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EduId { get; set; }
        public string InstituteName { get; set; }
        [Required(ErrorMessage = "This field can not be empty.")]
        [Display(Name = "Institute")]
        public int CurrentEducationInstituteId { get; set; }

        [Display(Name = "Degree")]
        public string Degree { get; set; }

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