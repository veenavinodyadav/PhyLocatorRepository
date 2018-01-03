
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhysicianLocator.Models
{
    [Table("tblMSTCountry ")]
    public class CountryViewModels
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }
        [Required]
        [StringLength(35, ErrorMessage = "The {0} must be at least {2} characters long.")]
        public string CountryName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [Required]
        public DateTime? CreatedOn { get; set; }
        [Required]
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public int LastModifiedBy { get; set; }
        public CountryViewModels()
        {
            CreatedOn = DateTime.Now;
            LastModifiedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }

    }
}

