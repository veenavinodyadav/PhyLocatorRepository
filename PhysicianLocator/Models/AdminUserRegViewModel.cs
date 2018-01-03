using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PhysicianLocator.Models
{
    public class AdminUserRegViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdminId { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = " Date of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Required]
        [Display(Name = "Cell Phone")]
        public string CellPhone { get; set; }
        public string WhoAreYou { get; set; }

        //Profile pic

        [Display(Name = "path ")]
        public string documentpath { get; set; }

            [Display(Name = "documentName ")]
            public string documentName { get; set; }
        public AddressViewModel AddressViewModel2 { get; set; } = new AddressViewModel();
        public AddressViewModel AddressViewModel1 { get; set; } = new AddressViewModel();
        [Display(Name = "Country")]
        public string CountryName1 { get; set; }
        public string CountryName2 { get; set; }
        public int CountryId1 { get; set; }
        public int CountryId2 { get; set; }

        [Required(ErrorMessage = "This field can not be empty.")]
        [Display(Name = "Institute Name")]
        public int CurrentEducationInstituteId { get; set; }
        public string InstituteName { get; set; }
        //Degree
        [Required]
        public string Degree { get; set; }
        //StartDate
        [Required]
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime StartDate { get; set; }
        //EndDate
        [Required]
        [Display(Name = " End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime EndDate { get; set; }
        [Required]
        [Display(Name = "Join Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime JoinStartDate { get; set; }

        //DateOfBirth
        [Required]
        [Display(Name = "Leave Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime JoinEndDate { get; set; }

        //CellPhone
        [Required]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use numbers only please")]
        [Display(Name = "CellPhone")]
        public string ReasonForLeaving { get; set; }

  
        public string Designation { get; set; }
        public string Type { get; set; }

        //Description
        public string Description { get; set; }


        [Required]
      
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime HealthStartDate { get; set; }

        //DateOfBirth
        [Required]
   
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime HealthEndDate { get; set; }
    }
}