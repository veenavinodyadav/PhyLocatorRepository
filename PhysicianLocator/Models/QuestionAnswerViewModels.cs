using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PhysicianLocator.Models
{
    [Table("tblQuestions")]
    public class QuestionViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionId { get; set; }//primary key      
        public int UserId { get; set; }//primary key   
        public int QuestionCategoryId { get; set; }//foriegn key
        [Required]
        public string Question { get; set; }

        public string QuestionDescription { get; set; }
        public string SearchTags { get; set; }
        public bool AcceptingAnswers { get; set; }
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

        public QuestionViewModel()
        {
            //CreatedOn = DateTime.Now;
            //LastModifiedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }
        public RegistrationViewModel questionregistration_Id { get; set; }
        public ICollection<AnswerViewModel> questionanswerviewmodel { get; set; }
        //public QuestionCategoryViewModel questionquestioncategory_Id { get; set; }

    }
    public class QuestionAnswerViewModel
    {
        public int QuestionAnswerId { get; set; }
        public int QuestionAnswerAnswerId { get; set; }
        public string QuestionAnswerQuestion { get; set; }
        public string QuestionAnswerUserName { get; set; }
        public DateTime QuestionAnswerCreatedDate { get; set; }
        public string QuestionAnswerDescription { get; set; }
        public string QuestionAnswerAnswer { get; set; }
        public int QuestionCommentId { get; set; }
        public int QuestionAnswerUserID { get; set; }
        public string QuestionAnswerDocumentPath { get; set; }
        public string date { get; set; }
    }
    public class commentview
    {
        public int CommentId { get; set; }//primary key
        public int answerid { get; set; }
        public string comm { get; set; }
        public int ParentCommentId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string date { get; set; }

    }
    public class AnswerOnQuestionViewModel
    {
        public string AnswerOnQuestionQuestion { get; set; }
        public string AnswerOnAnswer { get; set; }
        public string AnswerOnQuestionQuestionDescription { get; set; }
        public string AnswerOnQuestionUserName { get; set; }
        //public DateTime QuestionRegistrationCreatedDate { get; set; }


    }

    [Table("tblAnswers")]
    public class AnswerViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnswerId { get; set; }//primary key
        public int QuestionId { get; set; }//foriegn key   
        public string Answer { get; set; }
        public int ParentAnswerId { get; set; }//foriegn key  
        public bool IsThisTopAnswer { get; set; }
        public string SearchTags { get; set; }
        public string AcceptingComments { get; set; }
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

        public AnswerViewModel()
        {
            CreatedOn = DateTime.Now;
            LastModifiedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;

        }
        public QuestionViewModel answerquestion_Id { get; set; }
        public ICollection<CommentViewModel> answercommentviewmodel { get; set; }

    }

    [Table("tblComments")]
    public class CommentViewModel
    {
        [Key]
        public int CommentId { get; set; }//primary key
        public int ParentCommentId { get; set; }
        public int AnswerId { get; set; }//foriegn key
        public string Comment { get; set; }
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
        public CommentViewModel()
        {
            CreatedOn = DateTime.Now;
            LastModifiedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }
        public AnswerViewModel answercomment_Id { get; set; }

    }


    [Table("tblDocuments")]
    public class DocumentViewModel
    {
        [Key]

        public int DocumentId { get; set; }//primary key
        public string DocumentName { get; set; }//foriegn key
        public string DocumentPath { get; set; }
        public int PrimaryKeyId { get; set; }
        public string PrimaryObjectId { get; set; }
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

        public DocumentViewModel()
        {
            CreatedOn = DateTime.Now;
            LastModifiedOn = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }
    }

    [Table("LkUpQuestionCategory")]
    public class QuestionCategoryViewModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionCategoryId { get; set; }//primary key
        public string QuestionCategoryName { get; set; }
        public bool QuestionCategoryIsActive { get; set; }
        public DateTime QuestionCategoryCreatedDate { get; set; }
        public int QuestionCategoryCreatedBy { get; set; }
        public DateTime QuestionCategoryModifiedDate { get; set; }
        public int QuestionCategoryModifiedBy { get; set; }
        public ICollection<QuestionViewModel> questioncategoryquestionviewmodel { get; set; }
    }


}


