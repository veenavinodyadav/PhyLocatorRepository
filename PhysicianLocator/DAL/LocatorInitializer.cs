
using PhysicianLocator.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PhysicianLocator.DAL
{
    public class LocatorInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<LocatorContext>
    {
        protected override void Seed(LocatorContext context)
        {
            /**********************   MASTER  TABLE START   ********************************************************/

            // This is for populating ROLE table
            var role = new List<RoleViewModel>
            {
                new RoleViewModel{RoleName="Patient"},
                new RoleViewModel{RoleName="Doctor" },
                new RoleViewModel{RoleName="Admin" },
                new RoleViewModel{RoleName="SuperAdmin"},
            };
            role.ForEach(s => context.DBContext_role.Add(s));
            context.SaveChanges();

            

            // This is for populating country into MST_Country table
            List<String> country = new List<String>();
            foreach (RegionInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(p => new RegionInfo(p.LCID)).OrderBy(s => s.EnglishName).Distinct())
            {
                country.Add(ci.DisplayName);
            }
            country.ForEach(delegate (String name)
            {
                context.DBContext_country.Add(new CountryViewModels
                {
                    CountryName = name,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                    CreatedBy = 1,
                    LastModifiedBy = 1,
                    LastModifiedOn = DateTime.Now
                });
                context.SaveChanges();
            });

           //Institute
            var institute = new List<EducationInstitutesViewModel>
            {
                new EducationInstitutesViewModel{
                    InstituteName="Raisoni",ParentEducationInstituteId=0,IsActive=true,CurrentAssignId=1,IsDeleted=false,CreatedBy=1, CreatedOn=DateTime.Now, LastModifiedBy=1,LastModifiedOn=DateTime.Now,
                },
                new EducationInstitutesViewModel{
                    InstituteName="Dr. D. Y. Patil Medical College",ParentEducationInstituteId=0,IsActive=true,CurrentAssignId=2,IsDeleted=false,CreatedBy=1, CreatedOn=DateTime.Now, LastModifiedBy=1,LastModifiedOn=DateTime.Now,
                },
            };
            institute.ForEach(s => context.DBContext_educationinstitute.Add(s));
            context.SaveChanges();

            /**********************   MASTER  TABLE END  ********************************************************/


            
            //registration demo user delete when no need

            CommonFunction commonfunction = new CommonFunction();
            var password = "dummy@123";//password
            var reg = new List<RegistrationViewModel>
            {
                new RegistrationViewModel
                {

                RoleId=1,
                UserName="PatientUser",
                Password = commonfunction.Encrypt(password),//dummy@123
                FirstName = "PatientFirstname",
                LastName = "PatientLastname",
                GenderId = 1,
                DateOfBirth = DateTime.Parse("1990-09-01"),
                CellPhone = "9860085688",
                EmailAddress = "Patient@gmail.com",
                CanLogin=1,
                WhoAreYou="Patient",
                //ActivationCode = "1234567890",
                IsActive=true,
                IsDeleted=false,
                CreatedBy=1,
                CreatedOn=DateTime.Now,
                LastModifiedBy=1,
                LastModifiedOn=DateTime.Now,
                },
                new RegistrationViewModel
                {

                RoleId=2,
                UserName="DoctorUser",
                Password = commonfunction.Encrypt(password),//dummy@123
                FirstName = "DoctorFirstname",
                LastName = "DoctorLastname",
                GenderId = 1,
                DateOfBirth = DateTime.Parse("1990-09-01"),
                CellPhone = "9921007251",
                EmailAddress = "Doctor@gmail.com",
                IMAI="1234567890",
                Ref1_ContactNo="1234567890",
                Ref1_Email="ref1@gmail.com",
                Ref1_Name="referenceone",
                Ref2_ContactNo="1234567890",
                Ref2_Email="ref2@gmail.com",
                Ref2_Name="referencetwo",
                WhoAreYou="Doctor",
                CanLogin=1,              
                IsActive=true,
                IsDeleted=false,
                CreatedBy=1,
                CreatedOn=DateTime.Now,
                LastModifiedBy=1,
                LastModifiedOn=DateTime.Now,
                },
                 new RegistrationViewModel
                {

                RoleId=3,
                UserName="AdminUser",
                Password = commonfunction.Encrypt(password),//dummy@123
                FirstName = "AdminFirstname",
                LastName = "AdminLastname",
                GenderId = 1,
                DateOfBirth = DateTime.Parse("1990-09-01"),
                CellPhone = "9860085688",
                EmailAddress = "Admin@gmail.com",
                CanLogin=1,
                WhoAreYou="Doctor",
                //ActivationCode = "1234567890",
                IsActive=true,
                IsDeleted=false,
                CreatedBy=1,
                CreatedOn=DateTime.Now,
                LastModifiedBy=1,
                LastModifiedOn=DateTime.Now,
                },
            };
            reg.ForEach(s => context.DBContext_register.Add(s));
            context.SaveChanges();
            //end

            var question = new List<QuestionViewModel>
            {
                new QuestionViewModel
                {
               
                UserId=2,
                QuestionCategoryId = 1,
                Question = "What is Cancer  shape treatment.",
                QuestionDescription="",
                SearchTags = "How can knowing the mutations that cause a patient's cancer shape treatment?",
                AcceptingAnswers =false,               
                IsActive=true,
                IsDeleted=false,
                CreatedBy=1,
                CreatedOn=DateTime.Now,
                LastModifiedBy=1,
                LastModifiedOn=DateTime.Now,
                },
               
            };
            question.ForEach(s => context.DBContext_question.Add(s));
            context.SaveChanges();
            //end

            var answer = new List<AnswerViewModel>
            {
                new AnswerViewModel
                {
                QuestionId=1,
                Answer="Scientists have spent decades learning the precise genetic mutations that can cause a cell to start dividing uncontrollably. Cancer patients can now have their tumors sequenced to identify the genetic root of their disease and we have targeted treatments for many of these genetic alterations but we are still very much in the early years of making the most of genomic data.",
                IsActive=true,
                IsDeleted=false,
                CreatedBy=1,
                CreatedOn=DateTime.Now,
                LastModifiedBy=1,
                LastModifiedOn=DateTime.Now,
                },
            };
            answer.ForEach(s => context.DBContext_answer.Add(s));
         
            //Address demo user delete when no need
            var address = new List<AddressViewModel>
            {
                new AddressViewModel
                {

                PrimaryKeyId=2,
                PrimaryObjectId="tblUsers",
                Street1 = "Garden View Chamber",
                Street2 = "Kala Ghoda Circle",
                Street3 = "Sayajigunj",
                City = "Vadodara",
                State="Gujarat",
                CountryId = 56,
                Zipcode = "390005",
                latitute = "22.308556",
                longitute="73.1879301",
                IsPrimary=true,
                IsActive=true,
                IsDeleted=false,
                CreatedBy=1,
                CreatedOn=DateTime.Now,
                LastModifiedBy=1,
                LastModifiedOn=DateTime.Now,
                },
                new AddressViewModel
                {

                PrimaryKeyId=2,
                PrimaryObjectId="tblUsers",
                Street1 = "Shree Dutt House",
                Street2 = " Behind Badamdi Baug",
                Street3 = " Dandia Bazar",
                City = "Vadodara",
                State="Gujarat",
                CountryId = 56,
                Zipcode = "390001",
                latitute = "22.308556",
                longitute="73.1879301",
                IsPrimary=false,
                IsActive=true,
                IsDeleted=false,
                CreatedBy=1,
                CreatedOn=DateTime.Now,
                LastModifiedBy=1,
                LastModifiedOn=DateTime.Now,

                },
                  new AddressViewModel
                {

                PrimaryKeyId=1,
                PrimaryObjectId="tblUsers",
                Street1 = "ABB Company",
                Street2 = "Maneja",
                Street3 = "Makarpura",
                City = "Vadodara",
                State="Gujarat",
                CountryId = 56,
                Zipcode = "390001",
                latitute = "22.237149",
                longitute="73.183037",
                IsPrimary=true,
                IsActive=true,
                IsDeleted=false,
                CreatedBy=1,
                CreatedOn=DateTime.Now,
                LastModifiedBy=1,
                LastModifiedOn=DateTime.Now,

                },
                   new AddressViewModel
                {

                PrimaryKeyId=1,
                PrimaryObjectId="tblUsers",
                Street1 = "OpenEyes",
                Street2 = "Vadodara central",
                Street3 = "Sarabhai Road",
                City = "Vadodara",
                State="Gujarat",
                CountryId = 56,
                Zipcode = "390023",
                latitute = "22.31767",
                longitute="73.168519",
                IsPrimary=false,
                IsActive=true,
                IsDeleted=false,
                CreatedBy=1,
                CreatedOn=DateTime.Now,
                LastModifiedBy=1,
                LastModifiedOn=DateTime.Now,

                },
            };
            address.ForEach(s => context.DBContext_address.Add(s));
            context.SaveChanges();
            //end
            //============================================================================
            //Category

            var category = new List<LookupCategoryViewModel>
            {
                new LookupCategoryViewModel{
                    Category="Gender",IsActive=true,IsDeleted=false,CreatedBy=1, CreatedOn=DateTime.Now, LastModifiedBy=1,LastModifiedOn=DateTime.Now,
                },
            };
            category.ForEach(s => context.DBContext_lookupcategory.Add(s));
            context.SaveChanges();

            //Details
            var details = new List<LookupDetailViewModel>
            {
                new LookupDetailViewModel{
                    LookupCategoryId=1,Key=1,Value="Female",SortOrder=1,IsActive=true,IsDeleted=false,CreatedBy=1, CreatedOn=DateTime.Now, LastModifiedBy=1,LastModifiedOn=DateTime.Now,
                },
                 new LookupDetailViewModel{
                    LookupCategoryId=1,Key=2,Value="Male",SortOrder=2,IsActive=true,IsDeleted=false,CreatedBy=1, CreatedOn=DateTime.Now, LastModifiedBy=1,LastModifiedOn=DateTime.Now,
                },
            };
            details.ForEach(s => context.DBContext_lookupdetail.Add(s));
            context.SaveChanges();

          


            //Education
            var physicianeducation = new List<PhysicianEducationViewModel>
            {
                new PhysicianEducationViewModel{
                  UserId=1,CurrentEducationInstituteId=1, Degree="MBBS",StartDate=DateTime.Now,OldEducationInstituteId=1,IsDeleted=false,CreatedBy=1, CreatedOn=DateTime.Now, LastModifiedBy=1,LastModifiedOn=DateTime.Now,IsActive=true,

                },
                 new PhysicianEducationViewModel{
                  UserId=2,CurrentEducationInstituteId=2, Degree="MBBS",StartDate=DateTime.Now,OldEducationInstituteId=2,IsDeleted=false,CreatedBy=1, CreatedOn=DateTime.Now, LastModifiedBy=1,LastModifiedOn=DateTime.Now,IsActive=true,

                },

            };
            physicianeducation.ForEach(s => context.DBContext_physicianeducation.Add(s));
            context.SaveChanges();


            string register = "Hello " + "%FirstName%" + ",";
            register += ("\n\t\t\tTo activate your link please click here").PadLeft(30);
            register += ("\n\t\t\t" + "%url% " + "\n\t\t" + "Click here to activate your account").PadLeft(30);
            register += "\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tThanks";

            string forgetpassword = "Hello " + "%FirstName%" + ",";
            forgetpassword += ("\n\t\tTo reset your password please click here").PadLeft(30);
            forgetpassword += ("\n\t\t\t" + "%url% " + "\n\t\t" + "Click here to activate your account.").PadLeft(30);
            forgetpassword += "\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tThanks";

            string forgetusername = "Hello " + "%FirstName%" + ",";
            forgetusername += ("\n\t\t\tYour username is"+ "%UserName%").PadLeft(30);
            forgetusername += ("\n\t\t\t" + "%ur").PadLeft(30);
            forgetusername += "\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tThanks";

            var emailtemplate = new List<EmailTemplateViewModel>
            {
                 new EmailTemplateViewModel
                {
                  templateCode = "Register",                              
                  templateContent=register,
                  Subject="Please verify your email address" ,
                  IsActive=true,
                  IsDeleted=false,
                  CreatedBy=1,
                  CreatedOn=DateTime.Now,
                  LastModifiedBy=1,
                  LastModifiedOn=DateTime.Now,

                },

                new EmailTemplateViewModel
                {

                  templateCode = "ForgotPassword",
                  templateContent = forgetpassword,               
                  Subject ="Please click here to reset your password" ,
                  IsActive=true,
                  IsDeleted=false,
                  CreatedBy=1,
                  CreatedOn=DateTime.Now,
                  LastModifiedBy=1,
                  LastModifiedOn=DateTime.Now,

                },
                 new EmailTemplateViewModel
                {

                  templateCode = "ForgotUsername",
                  templateContent = forgetusername,
                  Subject ="Forget  Username" ,
                  IsActive=true,
                  IsDeleted=false,
                  CreatedBy=1,
                  CreatedOn=DateTime.Now,
                  LastModifiedBy=1,
                  LastModifiedOn=DateTime.Now,

                },
            };
            emailtemplate.ForEach(s => context.DBContext_emailtemplate.Add(s));
            context.SaveChanges();

            //hospital
            var hospital = new List<HospitalsViewModel>
            {
                new HospitalsViewModel
                {
                    HospitalName="Shreeji Hospital",
                    NumberOfBeds=2,
                    NumberOfBranches=2,
                    NumberOfDoctors=2,
                    Specialities="Maternity hospital",
                    IsActive=true,
                    CreatedOn=DateTime.Now,
                    LastModifiedOn=DateTime.Now
                },
                 new HospitalsViewModel
                {
                    HospitalName="Center for sight",
                    NumberOfBeds=2,
                    NumberOfBranches=2,
                    NumberOfDoctors=2,
                    Specialities="Eye hospital",
                    IsActive=true,
                    CreatedOn=DateTime.Now,
                    LastModifiedOn=DateTime.Now
                }

            };
            hospital.ForEach(s => context.DBContext_hospital.Add(s));
            context.SaveChanges();

        }
    }
}