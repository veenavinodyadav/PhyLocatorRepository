using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PhysicianLocator.Models
{
    public class MergeInstitute
    {
        [System.ComponentModel.DataAnnotations.Required]
        [Display(Name = "Assign Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.")]
        public string AssignName { get; set; }
    }
    [Table("tblMSTRole")]
    public  class RoleViewModel
    {
       [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public int RoleId { get; set; }
       public string RoleName { get; set; }
    }

}