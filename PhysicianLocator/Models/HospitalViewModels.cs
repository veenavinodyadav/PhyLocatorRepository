using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PhysicianLocator.Models
{
    [Table("tblHospitals")]
    public class HospitalsViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //HospitalId
        public int HospitalId { get; set; }

        //ParentHospitalId    
        public int ParentHospitalId { get; set; }

        //HospitalName
        [Required]
        [Display(Name = "Hospital Name")]
        [StringLength(100, ErrorMessage = "The street1 must be less than {1} characters long.")]
        public string HospitalName { get; set; }

        //NumberOfBranches
        [Required]
        [Display(Name = "Number Of Branches")]      
        public int NumberOfBranches { get; set; }

        //NumberOfBeds  
        [Display(Name = "Number Of Beds")]
        public int NumberOfBeds { get; set; }

        //NumberOfDoctors
        [Required]
        [Display(Name = "Number Of Doctors")]    
        public int NumberOfDoctors { get; set; }

        //Specialities
        [Required]
        [Display(Name = "Specialities")]
        [StringLength(50, ErrorMessage = "The state must be less than {1} characters long.")]
        public string Specialities { get; set; }

        //IsActive
        public bool IsActive { get; set; }

        //IsDeleted
        public bool IsDeleted { get; set; }

        //CreatedBy
        public int CreatedBy { get; set; }

        //CreatedOn
        public DateTime CreatedOn { get; set; }

        //LastModifiedBy
        public int LastModifiedBy { get; set; }

        //LastModifiedOn
        public DateTime LastModifiedOn { get; set; }

        public HospitalsViewModel()
        {
            CreatedOn = DateTime.Now;
            LastModifiedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
            
        }
    
    }
}