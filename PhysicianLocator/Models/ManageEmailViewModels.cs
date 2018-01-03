using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhysicianLocator.Models
{
    [Table("tblEmailTemplate")]
    public class EmailTemplateViewModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int templateid { get; set; }

        [Required(ErrorMessage = "Please template code.")]
        [StringLength(250, ErrorMessage = "Template code must be less than {1} characters long.")]
        [Display(Name = "Template Code ")]
        public string templateCode { get; set; }

        //[Required(ErrorMessage = "Please enter  template content.")]
        //[Display(Name = "Template content ")]
        public string templateContent { get; set; }

        //EmailAddress
        //[Required(ErrorMessage = "Please enter from email address.")]
        [EmailAddress]
        [Display(Name = "From")]
        [StringLength(250, ErrorMessage = "The  email address must be  less than {1} characters long.")]
        public string From { get; set; }

        //EmailAddress
        [EmailAddress]
        [Display(Name = "Bcc")]
        [StringLength(250, ErrorMessage = "The email address must be  less than {1} characters long.")]
        public string Bcc { get; set; }

       // EmailAddress
        [EmailAddress]
        [Display(Name = "Cc")]
        [StringLength(250, ErrorMessage = "The email address must be  less than {1} characters long.")]
        public string Cc { get; set; }

        [Required(ErrorMessage = "Please enter  subject.")]
        [StringLength(500, ErrorMessage = "Template code must be less than {1} characters long.")]
        [Display(Name = "Subject ")]
        public string Subject { get; set; }

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
        public EmailTemplateViewModel()
        {
            CreatedOn = DateTime.Now;
            LastModifiedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }
        public ICollection<EmailLogViewModel> temlpateemaillog { get; set; }
        public ICollection<EmailTemplateAttachment> temlpateattachment { get; set; }

    }

    [Table("tblEmailLog")]
    public class EmailLogViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmailLogid { get; set; }
        public int templateid { get; set; }
        public string EmailLogFrom { get; set; } 
        public string EmailLogTo { get; set; }
        public bool IsEmailSend { get; set; }
        public bool AttachmentIds { get; set; }
        public DateTime SendOn { get; set; }     
        public EmailLogViewModel()
        {
            SendOn = DateTime.Now;         
        }
        public EmailTemplateViewModel emaillogtemplate_Id { get; set; }
    }
    [Table("tblTemplateAttachment")]
    public class EmailTemplateAttachment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int attachmentid { get; set; }
        public int templateid { get; set; }
        public string attachmentName { get; set; }
        public string attachmentType { get; set; }

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
        public EmailTemplateAttachment()
        {
            CreatedOn = DateTime.Now;
            LastModifiedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }
        public EmailTemplateViewModel attachmenttemplate_Id { get; set; }

    }
}