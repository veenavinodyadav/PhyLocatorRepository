using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PhysicianLocator.Models
{
    public class ExperienceGridModel
    {
        public int ExpId { get; set; }

        [Display(Name = "Join Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yy}")]
        public DateTime JoinStartDate { get; set; }
        
        [Display(Name = "Leave Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy}")]
        public DateTime JoinEndDate { get; set; }
     
        [Display(Name = "Leave Reson")]
        public string ReasonForLeaving { get; set; }

        public string Designation { get; set; }
    }
}