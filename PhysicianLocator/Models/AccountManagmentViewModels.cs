using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PhysicianLocator.Models
{
    [Table("tblUsers")]
    public class RegistrationViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //UserId
        public int UserId { get; set; }
        //UserName
        [Required(ErrorMessage = "Please enter  user name.")]
        [RegularExpression(@"^[a-zA-Z]\w+|[0-9][0-9_]*[a-zA-Z]+\w*$", ErrorMessage = "Please use letter,underscore,number only.")]
        [StringLength(15, ErrorMessage = "The user name must be less than {1} characters long.")]
        [Display(Name = "User Name ")]
        public string UserName { get; set; }

        //RoleId
        public int RoleId { get; set; }

        //FirstName
        [Required(ErrorMessage = "Please enter  first name.")]
        [RegularExpression(@"^[a-zA-Z']+$", ErrorMessage = "Please use letter and '  only.")]
        [StringLength(50, ErrorMessage = "The first name must be less than {1} characters long.")]
        [Display(Name = "First Name ")]
        public string FirstName { get; set; }

        //LastName
        [Required(ErrorMessage = "Please enter  last name.")]
        [RegularExpression(@"^[a-zA-Z']+$", ErrorMessage = "Please use letter and '  only.")]
        [Display(Name = "Last Name ")]
        [StringLength(50, ErrorMessage = "The last name must be  less than {1} characters long.")]
        public string LastName { get; set; }

        //EmailAddress
        [Required(ErrorMessage = "Please enter  email address.")]
        [EmailAddress]
        [Display(Name = "Email Address ")]
        [StringLength(100, ErrorMessage = "The email address must be  less than {1} characters long.")]
        public string EmailAddress { get; set; }

        //Password
        [Required(ErrorMessage = "Please enter Password.")]
        [StringLength(50, ErrorMessage = "The password must be  less than {1} characters and greater than {2} long.", MinimumLength = 8)]
        //[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Please use minimum eight characters, at least one letter, one number and one special character only.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password ")]
        public string Password { get; set; }

        //DateOfBirth
        [Required(ErrorMessage = "Please enter date of birth.")]
        [Display(Name = "Date of Birth ")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime DateOfBirth { get; set; }

        //CellPhone
        [Required(ErrorMessage = "Please enter  cell phone. ")]
        [StringLength(15, ErrorMessage = "The cell phone must be  less than {1} characters long.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please use numbers only ")]
        [Display(Name = "Cell Phone ")]
        public string CellPhone { get; set; }

        //GenderId
        [Required(ErrorMessage = "Please enter  gender. ")]
        [Display(Name = "Gender ")]
        public int GenderId { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "The {0} must be at least {2} characters long.")]
        public string WhoAreYou { get; set; }
        //IMAI

        [Display(Name = "IMAI")]
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        public string IMAI { get; set; }

        //Reference1
        
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} characters long.")]
        [Display(Name = "Name")]
        public string Ref1_Name { get; set; }

        
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.")]
        public string Ref1_Email { get; set; }

       
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use numbers only please")]
        [Display(Name = "Phone No")]
        public string Ref1_ContactNo { get; set; }


        //Reference2
        
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        [StringLength(25, ErrorMessage = "The {0} must be at least {2} characters long.")]
        [Display(Name = "Name")]
        public string Ref2_Name { get; set; }
        
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.")]
        public string Ref2_Email { get; set; }
        
        [StringLength(10, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use numbers only please")]
        [Display(Name = "Phone No")]
        public string Ref2_ContactNo { get; set; }

        //Profile pic
        [Display(Name = "UserPhoto ")]
        public byte[] UserPhoto { get; set; }

        //ActivationCode
        [StringLength(10)]
        public string ActivationCode { get; set; }

        //CanLogin
        public int CanLogin { get; set; }

        //IsActive
        public bool IsActive { get; set; }

        //IsDeleted
        public bool IsDeleted { get; set; }

        //CreatedBy
        public int CreatedBy { get; set; }

        //CreatedOn
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime CreatedOn { get; set; }

        //LastModifiedBy
        public int LastModifiedBy { get; set; }

        //LastModifiedOn
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime LastModifiedOn { get; set; }

        public RegistrationViewModel()
        {
            CreatedOn = DateTime.Now;
            LastModifiedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }
        public ICollection<PhysicianReferencesViewModel> userphysicianviewmodel { get; set; }
        public ICollection<LoginHistoryViewModel> registrationloginhistoryviewmodel { get; set; }
        public ICollection<PhysicianEducationViewModel> registrationphysicianeducationviewmodel { get; set; }
        public ICollection<QuestionViewModel> registrationquestion { get; set; }
        public ICollection<HealthHistoryViewModel> registrationhealthhistory { get; set; }



    }
    [Table("tblAddress")]
    public class AddressViewModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //AddressId
        public int AddressId { get; set; }

        //PrimaryKeyId
        [Required]
        public int PrimaryKeyId { get; set; }

        //PrimaryObjectId
        [Required]
        public String PrimaryObjectId { get; set; }

        //Street1
        [Required]
        [Display(Name = "Street 1")]
        [StringLength(100, ErrorMessage = "The street1 must be less than {1} characters long.")]
        public string Street1 { get; set; }

        //Street2
        [Required]
        [Display(Name = "Street 2")]
        [StringLength(100, ErrorMessage = "The street2 must be less than {1} characters long.")]
        public string Street2 { get; set; }

        //Street3
        [StringLength(100, ErrorMessage = "The street3 must be less than {1} characters long.")]
        [Display(Name = "Street 3")]
        public string Street3 { get; set; }

        //City
        [Required]
        [Display(Name = "City")]
        [StringLength(50, ErrorMessage = "The city must be less than {1} characters long.")]
        public string City { get; set; }

        //State
        [Required]
        [Display(Name = "State")]
        [StringLength(50, ErrorMessage = "The state must be less than {1} characters long.")]
        public string State { get; set; }

        //CountryId

        [Display(Name = "Country")]
        public int CountryId { get; set; }

        //Zipcode
        [Required]
        [Display(Name = "Zipcode")]
        [StringLength(10, ErrorMessage = "The zipcode must be less than {1} characters long.")]
        public string Zipcode { get; set; }
        [Required]
        public string latitute { get; set; }
        [Required]
        public string longitute { get; set; }
        //IsPrimary
        public bool IsPrimary { get; set; }
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

        public AddressViewModel()
        {
            CreatedOn = DateTime.Now;
            LastModifiedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
            IsPrimary = true;
        }
        public CountryViewModels addresscountry_Id { get; set; }
    }

    [Table("tblPhysicianReferences")]
    public class PhysicianReferencesViewModel
    {
        //PhysicianReferencesId
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhysicianReferencesId { get; set; }//PK
        //PhysicianId
        [Required]
        [Display(Name = "PhysicianId")]
        [StringLength(15, ErrorMessage = "The {0} must be less than {1} characters long.")]
        public string PhysicianId { get; set; }
        //UserId      
        public int UserId { get; set; }//FK
        //Designation
        [Required]
        [Display(Name = "Designation")]
        [StringLength(100, ErrorMessage = "The {0} must be less than {1} characters long.")]
        public string Designation { get; set; }
        //HowDoYouKnow
        [Required]
        [Display(Name = "How Do You Know")]
        [StringLength(50, ErrorMessage = "The {0} must be less than {1} characters long.")]
        public string HowDoYouKnow { get; set; }
        //PhoneNumber
        [Required]
        [Display(Name = "Phone Number")]
        [StringLength(10, ErrorMessage = "The {0} must be less than {1} characters long.", MinimumLength = 10)]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use numbers only please")]

        public string PhoneNumber { get; set; }
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

        public PhysicianReferencesViewModel()
        {
            CreatedOn = DateTime.Now;
            LastModifiedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }
        public RegistrationViewModel physicianuser_Id { get; set; }
    }
    [Table("tblMSTLookupCategory")]
    public class LookupCategoryViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //LookupCategoryId
        public int LookupCategoryId { get; set; }

        //Category
        [Required(ErrorMessage = "Please enter category name.")]
        [Display(Name = "Category  ")]
        [StringLength(50, ErrorMessage = "The category must be at least {2} characters long.")]
        public string Category { get; set; }
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

        public LookupCategoryViewModel()
        {
            CreatedOn = DateTime.Now;
            LastModifiedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }
        public ICollection<LookupDetailViewModel> lookupcatergorylookupdetailviewmodel { get; set; }

    }

    [Table("tblMSTLookupDetail")]
    public class LookupDetailViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //LookupDetailId
        public int LookupDetailId { get; set; }

        //LookupCategoryId
        [Required(ErrorMessage = "Please select category name.")]
        [Display(Name = "Category  ")]
        public int LookupCategoryId { get; set; }

        //SortOrder
        [Required(ErrorMessage = "Please enter sort Order.")]
        [Display(Name = "Sort Order")]
        public int SortOrder { get; set; }

        //Key
        [Required(ErrorMessage = "Please enter category Key.")]

        [Display(Name = "Key")]
        public int Key { get; set; }

        //Value
        [Required(ErrorMessage = "Please enter category Value.")]
        [StringLength(50, ErrorMessage = "The value must be less than {1} characters long.")]
        [Display(Name = "Value")]
        public string Value { get; set; }

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

        public LookupDetailViewModel()
        {
            CreatedOn = DateTime.Now;
            LastModifiedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;

        }
        public LookupCategoryViewModel lookupcategorylookupdetail_Id { get; set; }
    }
    public class LoginViewModel
    {
        //LoginUserName
        [Required(ErrorMessage = "Please enter  user name.")]
        [RegularExpression(@"^[a-zA-Z]\w+|[0-9][0-9_]*[a-zA-Z]+\w*$", ErrorMessage = "Please use letter,underscore,number only.")]
        [StringLength(15, ErrorMessage = "The user name must be less than {1} characters long.")]
        [Display(Name = "User Name ")]
        public string LoginUserName { get; set; }

        //Password
        [Required(ErrorMessage = "Please enter Password.")]
        [StringLength(15, ErrorMessage = "The {0} must be less than {1} and greater than {2} characters long.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessage = "Please use minimum eight characters, at least one letter, one number and one special character only.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password ")]
        public string LoginPassword { get; set; }
    }
    [Table("tblLoginHistory")]
    public class LoginHistoryViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoginId { get; set; }
        public int UserId { get; set; }

        public DateTime LoginTime { get; set; }

        public LoginHistoryViewModel()
        {
            LoginTime = DateTime.Now;
        }
        public RegistrationViewModel loginhistoryregistration_Id { get; set; }


    }
    public class ForgotPasswordViewModel
    {

        [Required(ErrorMessage = "Please enter  user name.")]
        [RegularExpression(@"^[a-zA-Z]\w+|[0-9][0-9_]*[a-zA-Z]+\w*$", ErrorMessage = "Please use letter,underscore,number only.")]

        [StringLength(15, ErrorMessage = "The {0} must be less than {1} characters long.")]
        [Display(Name = "User Name ")]
        public string ForgotPassword_userName { get; set; }

        //EmailAddress
        [Required(ErrorMessage = "Please enter  email address.")]
        [EmailAddress]
        [Display(Name = "Email Address ")]
        [StringLength(100, ErrorMessage = "The {0} must be less than {1} characters long.")]
        public string ForgotPassword_email { get; set; }
    }
    public class ResetPasswordViewModel
    {

        //Password
        [Required(ErrorMessage = "Please enter Password.")]
        [StringLength(15, ErrorMessage = "The {0} must be less than {1} and greater than {2} characters long.", MinimumLength = 8)]
        //[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessage = "Please use minimum eight characters, at least one letter, one number and one special character only.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password ")]
        public string Reset_Password { get; set; }

        //Password
        [Required(ErrorMessage = "Please enter Password.")]
        [StringLength(15, ErrorMessage = "The {0} must be less than {1} and greater than {2} characters long.", MinimumLength = 8)]
        //[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$", ErrorMessage = "Please use minimum eight characters, at least one letter, one number and one special character only.")]
      //  [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
       // [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string Reset_ConfirmPassword { get; set; }

    }
    public class ForgotUsernameViewModel
    {
        //EmailAddress
        [Required(ErrorMessage = "Please enter  email address.")]
        [EmailAddress]
        [Display(Name = "Email Address ")]
        [StringLength(100, ErrorMessage = "The {0} must be less than {1} and greater than {2} characters long.")]
        public string ForgotUsername_email { get; set; }
    }
    [Table("tblMSTEducationInstitutes")]
    public class EducationInstitutesViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //EducationInstituteId
        public int EducationInstituteId { get; set; }
        //Category
        [Required(ErrorMessage = "Please enter name of the institute.")]
        [StringLength(100, ErrorMessage = "The {0} must be less than {1} characters long.")]
        [Display(Name = "Institute Name")]
        public string InstituteName { get; set; }
        public int ParentEducationInstituteId { get; set; }
        //IsActive
        public bool IsActive { get; set; }
        public int CurrentAssignId { get; set; }

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

        public EducationInstitutesViewModel()
        {
            CreatedOn = DateTime.Now;
            LastModifiedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }

    }
    [Table("tblPhysicianEducations")]
    public class PhysicianEducationViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //PhysicianEducationId
        public int PhysicianEducationId { get; set; }
        //UserId
        public int UserId { get; set; }
        //EducationInstituteId

        [Required(ErrorMessage = "This field can not be empty.")]
        [Display(Name = "Institute Name")]
        public int CurrentEducationInstituteId { get; set; }
        //Degree

        [Required(ErrorMessage = "Please enter Degree.")]
        [StringLength(50, ErrorMessage = "The {0} must be less than {1} and greater than {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Degree ")]
        public string Degree { get; set; }
        //StartDate
        [Required(ErrorMessage = "This field can not be empty.")]
        [Display(Name = "Year of Passing")]
        public DateTime StartDate { get; set; }
        //EndDate
        [Required(ErrorMessage = "This field can not be empty.")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public int OldEducationInstituteId { get; set; }

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
        [Required]
        public DateTime LastModifiedOn { get; set; }

        public PhysicianEducationViewModel()
        {

            CreatedOn = DateTime.Now;
            LastModifiedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }
        public RegistrationViewModel physicianeducationregistration_Id { get; set; }



    }
    [Table("tblPhysicianExperiences")]
    public class PhysicianExperienceViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //UserId
        public int PhysicianExperienceId { get; set; }
        //UserName

        public int UserId { get; set; }

        //RoleId
        [Required]
        public int HospitalId { get; set; }

        [Display(Name = "Current Working Hospital ")]
        public bool CurrentWorkingHospital { get; set; }

        //DateOfBirth
        [Required]
        [Display(Name = "Date of Joining")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime JoinStartDate { get; set; }

        //DateOfBirth
        [Required]
        [Display(Name = "Date of Leaving")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime JoinEndDate { get; set; }

        [StringLength(150, ErrorMessage = "The {0} must be less than {1} characters long.")]
        [Display(Name = "Reason for Leaving password")]
        public string ReasonForLeaving { get; set; }

       

        //Designation
        [Required(ErrorMessage = "Please enter designation.")]
        [StringLength(25, ErrorMessage = "The {0} must be less than {1} characters long.")]
        [Display(Name = "Designation")]
        public string Designation { get; set; }

        //IsActive
        public bool IsActive { get; set; }

        //IsDeleted
        public bool IsDeleted { get; set; }

        //CreatedBy
        public int CreatedBy { get; set; }

        //CreatedOn
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime CreatedOn { get; set; }

        //LastModifiedBy
        public int LastModifiedBy { get; set; }

        //LastModifiedOn
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime LastModifiedOn { get; set; }

        public PhysicianExperienceViewModel()
        {
            CreatedOn = DateTime.Now;
            LastModifiedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }
        public RegistrationViewModel physicianexperienceregistration_Id { get; set; }
    }
    [Table("tblHealthHistories")]
    public class HealthHistoryViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //UserId
        public int HealthHistoryId { get; set; }
        //UserName

        //public int UserId { get; set; }
        //Description
        [Required]
        public string Type { get; set; }

        //Description
        [Required]
        public string Description { get; set; }

        //StartDate
        [Required]
        [Display(Name = "Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime StartDate { get; set; }

        //EndDate
        [Required]
        [Display(Name = "End Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime EndDate { get; set; }

        //IsActive
        public bool IsActive { get; set; }

        //IsDeleted
        public bool IsDeleted { get; set; }

        //CreatedBy
        public int CreatedBy { get; set; }

        //CreatedOn
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime CreatedOn { get; set; }

        //LastModifiedBy
        public int LastModifiedBy { get; set; }

        //LastModifiedOn
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime LastModifiedOn { get; set; }

        public HealthHistoryViewModel()
        {
            CreatedOn = DateTime.Now;
            LastModifiedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }
        public RegistrationViewModel healthhistoryregistration_Id { get; set; }
    }
   
   
}