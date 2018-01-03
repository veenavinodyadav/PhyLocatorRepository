using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PhysicianLocator.Models
{
    public class AdminHospitalViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminId { get; set; }
        public AddressViewModel AddressViewModel2 { get; set; } = new AddressViewModel();
        public AddressViewModel AddressViewModel1 { get; set; } = new AddressViewModel();
        [Display(Name = "Country")]
        public string CountryName1 { get; set; }
        public string CountryName2 { get; set; }
        //ParentHospitalId    
        public int HospitalId { get; set; }
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
        //LastModifiedBy
        public int LastModifiedBy { get; set; }

        //LastModifiedOn
        public DateTime LastModifiedOn { get; set; }

    }
}