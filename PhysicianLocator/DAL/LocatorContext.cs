using PhysicianLocator.Models;
using System.Data.Entity;

using System.Data.Entity.ModelConfiguration.Conventions;

namespace PhysicianLocator.DAL
{
    public class LocatorContext : DbContext
    {

        public LocatorContext() : base("LocatorContext1")
        {
            Database.SetInitializer<LocatorContext>(new CreateDatabaseIfNotExists<LocatorContext>());
        }
        //public DbSet<RoleViewModel> DBContext_role { get; set; }
        public DbSet<RegistrationViewModel> DBContext_register { get; set; }
        public DbSet<LookupCategoryViewModel> DBContext_lookupcategory { get; set; }
        public DbSet<LookupDetailViewModel> DBContext_lookupdetail { get; set; }
        public DbSet<CountryViewModels> DBContext_country { get; set; }
        public DbSet<AddressViewModel> DBContext_address { get; set; }
        public DbSet<PhysicianReferencesViewModel> DBContext_physician { get; set; }
        public DbSet<LoginHistoryViewModel> DBContext_loginhistory { get; set; }
        public DbSet<EducationInstitutesViewModel> DBContext_educationinstitute { get; set; }
        public DbSet<PhysicianEducationViewModel> DBContext_physicianeducation { get; set; }
        public DbSet<PhysicianExperienceViewModel> DBContext_experience { get; set; }
        public DbSet<HealthHistoryViewModel> DBContext_healthhistory { get; set; }
        //QuestionAnswerViewModel
        public DbSet<QuestionViewModel> DBContext_question { get; set; }
        public DbSet<AnswerViewModel> DBContext_answer { get; set; }
        public DbSet<DocumentViewModel> DBContext_document { get; set; }
        public DbSet<CommentViewModel> DBContext_comment { get; set; }
        //ManageEmailViewModels
        public DbSet<EmailTemplateViewModel> DBContext_emailtemplate { get; set; }
        public DbSet<EmailLogViewModel> DBContext_emaillog{ get; set; }
        public DbSet<EmailTemplateAttachment> DBContext_emailattachment { get; set; }
        public DbSet<RoleViewModel> DBContext_role { get; set; }
       public DbSet<HospitalsViewModel> DBContext_hospital { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            Database.SetInitializer<LocatorContext>(null);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);

        }


    }
}