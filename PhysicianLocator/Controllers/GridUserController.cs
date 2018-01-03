using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using PhysicianLocator.Models;
using PhysicianLocator.DAL;
using System.Device.Location;
using System.Configuration;
using System.Data.SqlClient;
using GoogleMaps.LocationServices;

namespace PhysicianLocator.Controllers
{
    public class GridUserController : Controller
    {
        private LocatorContext db = new LocatorContext();
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(GridUserController));  //Declaring Log4Net
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserRegistration_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<RegistrationViewModel> tblregistrations = db.DBContext_register;
            LocatorContext context = new LocatorContext();
            List<AdminUserRegViewModel> questionList = new List<AdminUserRegViewModel>();
            //var meds = from user in context.DBContext_register
            //           join address in context.DBContext_address
            //           on user.UserId equals address.PrimaryKeyId

            //           select new AdminUserRegViewModel
            //           {
            //               AdminId= user.UserId,
            //               FirstName = user.FirstName,
            //               LastName = user.LastName,
            //               EmailAddress = user.EmailAddress,
            //               City = address.City,
            //               State = address.State
            //           };
            var meds = from user in context.DBContext_register
                       select new AdminUserRegViewModel
                       {
                           AdminId = user.UserId,
                           FirstName = user.FirstName,
                           LastName = user.LastName,
                           EmailAddress = user.EmailAddress,
                           CellPhone = user.CellPhone,
                           DateOfBirth = user.DateOfBirth,
                           WhoAreYou=user.WhoAreYou                          
                       };
            questionList = meds.ToList();
            ViewBag.list = questionList;
            DataSourceResult result = questionList.ToDataSourceResult(request);
            return Json(result);
        }
        public ActionResult read_address()
        {       
            string markers = "[";
            string conString = ConfigurationManager.ConnectionStrings["LocatorContext1"].ConnectionString;
            SqlCommand cmd = new SqlCommand("SELECT * FROM tblAddress");
            using (SqlConnection con = new SqlConnection(conString))
            {
                cmd.Connection = con;
                con.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        markers += "[";
                        markers += string.Format(" '{0}',", sdr["Street1"]);
                        markers += string.Format(" '{0}',", sdr["Street2"]);
                        markers += string.Format(" '{0}',", sdr["Street3"]);
                        markers += string.Format(" '{0}'", sdr["State"]);
                        markers += "],";
                    }
                }
                con.Close();
            }

            markers += "];";
            ViewBag.Markers = markers;
            //var address = "ABB,maneja,vadodara,gujarat";
            //var address1 = "vadodara central,sarabhai Road,vadodara,gujarat";

            //var locationService = new GoogleLocationService();
            //var point = locationService.GetLatLongFromAddress(address);
            //var point1 = locationService.GetLatLongFromAddress(address1);

            //var latitude = point.Latitude;
            //var longitude = point.Longitude;
            //var latitude1 = point1.Latitude;
            //var longitude1 = point1.Longitude;

            //var sCoord = new GeoCoordinate(latitude, longitude);
            //var eCoord = new GeoCoordinate(latitude1, longitude1);

            //var res = sCoord.GetDistanceTo(eCoord);
            //res = res / 1000;

            return View();
        }
        public ActionResult distance()
        {
            var address = "ABB,maneja,vadodara,gujarat";
            ViewBag.Address = address;
            string markers = "[";          
            string conString = ConfigurationManager.ConnectionStrings["LocatorContext1"].ConnectionString;
            SqlCommand cmd = new SqlCommand("SELECT * FROM tblAddress");
            var locationService = new GoogleLocationService();
            var point = locationService.GetLatLongFromAddress(address);
            var latitude = point.Latitude;
            var longitude = point.Longitude;
            var sCoord = new GeoCoordinate(latitude, longitude);
            using (SqlConnection con = new SqlConnection(conString))
            {
                cmd.Connection = con;
                con.Open();

                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {              
                        var lat =Convert.ToDouble(sdr["latitute"]);
                        var lng= Convert.ToDouble(sdr["longitute"]);
                        var eCoord = new GeoCoordinate(lat, lng);
                        var res = sCoord.GetDistanceTo(eCoord);
                        res = res / 1000;
                        if (res <= 5)
                        {
                            markers += "[";
                            markers += string.Format(" '{0}',", sdr["Street1"]);
                            markers += string.Format(" '{0}',", sdr["Street2"]);
                            markers += string.Format(" '{0}',", sdr["Street3"]);
                            markers += string.Format(" '{0}',", sdr["City"]);
                            markers += string.Format(" '{0}'", sdr["State"]);
                            markers += "],";
                        }
                    }
                }
                con.Close();
            }
            markers += "];";
            ViewBag.Markers = markers;
            //var res = sCoord.GetDistanceTo(eCoord);
            //res = res / 1000;

            return View();
        }
        public ActionResult Education_Read(int? id)
        {
            DataSourceRequest request = new DataSourceRequest();
            List<EducationGridModel> questionList = new List<EducationGridModel>();
            IQueryable<PhysicianEducationViewModel> tblregistrations = db.DBContext_physicianeducation;
            DataSourceResult result = new DataSourceResult();
            LocatorContext context = new LocatorContext();
            {
                var meds = from edu in context.DBContext_physicianeducation
                           join ins in context.DBContext_educationinstitute on edu.CurrentEducationInstituteId equals ins.EducationInstituteId
                           where (edu.UserId == id && edu.IsActive && !edu.IsDeleted)
                           select new EducationGridModel
                           {
                               EduId = edu.PhysicianEducationId,
                               InstituteName = ins.InstituteName,
                               Degree = edu.Degree,
                               StartDate = edu.StartDate,
                               EndDate = edu.EndDate
                           };
                {
                    questionList = meds.ToList();
                }
                result = questionList.ToDataSourceResult(request);
                return Json(result);
            }
        }
        public ActionResult Health_Read(int? id)
        {
            DataSourceRequest request = new DataSourceRequest();
            List<HealthGridModel> healthList = new List<HealthGridModel>();
            IQueryable<HealthHistoryViewModel> tblhealth = db.DBContext_healthhistory;
            DataSourceResult result = new DataSourceResult();
            LocatorContext context = new LocatorContext();
            {

                var meds = from health in context.DBContext_healthhistory
                               //join document in context.DBContext_document on health.CreatedBy equals document.CreatedBy
                           where ( /*(health.CreatedBy == userid && document.PrimaryObjectId == "tblHealthHistories") ||*/ (health.IsActive && !health.IsDeleted))
                           select new HealthGridModel
                           {
                               HealthId = health.HealthHistoryId,
                               Type = health.Type,
                               Description = health.Description,
                               //filename = document.DocumentName,
                               StartDate = health.StartDate,
                               EndDate = health.EndDate
                           };
                {
                    healthList = meds.ToList();
                }
                result = healthList.ToDataSourceResult(request);
                return Json(result);
            }
        }
        public ActionResult Experience_Read([DataSourceRequest]DataSourceRequest request, int? id)
        {
            List<ExperienceGridModel> questionList = new List<ExperienceGridModel>();
            IQueryable<PhysicianExperienceViewModel> tblregistrations = db.DBContext_experience;
            DataSourceResult result = new DataSourceResult();
            LocatorContext context = new LocatorContext();
            {
                var meds = from reg in context.DBContext_register
                           join exp in context.DBContext_experience on reg.UserId equals exp.UserId
                           where (exp.UserId == id && exp.IsActive && !exp.IsDeleted)
                           select new ExperienceGridModel
                           {
                               ExpId = exp.PhysicianExperienceId,
                               Designation = exp.Designation,
                               ReasonForLeaving = exp.ReasonForLeaving,
                               JoinStartDate = exp.JoinStartDate,
                               JoinEndDate = exp.JoinEndDate
                           };
                {
                    questionList = meds.ToList();
                }
                result = questionList.ToDataSourceResult(request);
                return Json(result);
            }
        }
        public ActionResult Edit_education([DataSourceRequest]DataSourceRequest request, EducationGridModel educationgridmodel)
        {
            LocatorContext context = new LocatorContext();
            var tempkendoInstitute3 = Request["kendoInstitute"];
            var tempkendoInstitute = educationgridmodel.InstituteName;
            var isactive = true;

            //EDUCATIONAL INSTITUTE----------------------------------

            string result = Convert.ToString(tempkendoInstitute);
            EducationInstitutesViewModel education = new EducationInstitutesViewModel();
            education.InstituteName = tempkendoInstitute;
            var institute = context.DBContext_educationinstitute.Where(u => u.InstituteName == result).FirstOrDefault();
            int lastcurrentAssignId = context.DBContext_educationinstitute.Max(item => item.EducationInstituteId);
            education.CurrentAssignId = lastcurrentAssignId + 1;
            education.IsActive = isactive;
            if (institute == null)
            {

                var record_educationinstitute = new EducationInstitutesViewModel()
                {
                    InstituteName = education.InstituteName,
                    ParentEducationInstituteId = 0,
                    IsActive = education.IsActive,
                    CurrentAssignId = education.CurrentAssignId,
                    IsDeleted = false,
                    CreatedBy = 0,
                    CreatedOn = DateTime.Now,
                    LastModifiedBy = 0,
                    LastModifiedOn = DateTime.Now,

                };
                context.DBContext_educationinstitute.Add(education);
                context.SaveChanges();
                tempkendoInstitute = educationgridmodel.InstituteName;
            }
            result = Convert.ToString(tempkendoInstitute);
            var currentassign = from educationinstitute in context.DBContext_educationinstitute
                                where (educationinstitute.InstituteName.Equals(result))
                                select new
                                {
                                    educationinstitute.CurrentAssignId
                                };
            educationgridmodel.CurrentEducationInstituteId = Convert.ToInt32(currentassign.FirstOrDefault().CurrentAssignId);
            var tempkendoStartDate = Request["SD"];
            DateTime SD = Convert.ToDateTime(tempkendoStartDate);
            educationgridmodel.StartDate = SD;

            var tempkendoEndDate = Request["ED"];
            DateTime ED = Convert.ToDateTime(tempkendoEndDate);
            educationgridmodel.EndDate = ED;

            var user = db.DBContext_physicianeducation.Where(u => u.UserId == educationgridmodel.EduId).FirstOrDefault();


            //if (ModelState.IsValid)
            //{
            //if (user != null)
            //{
            //   var entity = new EducationGridModel
            //    {
            //        EduId = educationgridmodel.EduId,
            //        CurrentEducationInstituteId = educationgridmodel.CurrentEducationInstituteId,
            //        Degree = educationgridmodel.Degree,                    
            //        StartDate = educationgridmodel.StartDate,
            //        EndDate = educationgridmodel.EndDate,

            //    };

            ////db.DBContext_physicianeducation.Attach(educationgridmodel);
            //db.Entry(educationgridmodel).State = EntityState.Modified;
            ////context.DBContext_address.Add(model.AddressViewModel1);
            ////db.DBContext_address.Add(educationgridmodel);
            //db.SaveChanges();
            //}

            var id = educationgridmodel.EduId;
            PhysicianEducationViewModel phy_query = (from physicaineducation in context.DBContext_physicianeducation where physicaineducation.PhysicianEducationId == id select physicaineducation).SingleOrDefault();
            if (phy_query != null)
            {
                phy_query.CurrentEducationInstituteId = phy_query.OldEducationInstituteId;
                phy_query.CurrentEducationInstituteId = educationgridmodel.CurrentEducationInstituteId;
                phy_query.Degree = educationgridmodel.Degree;
                phy_query.StartDate = educationgridmodel.StartDate;
                phy_query.EndDate = educationgridmodel.EndDate;
                try
                {
                    context.SaveChanges();
                    TempData["Data"] = "Demerged Successful";
                    return View("../Shared/Error");
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                }

            }
            //}
            ModelState.Clear();
            //return Json(ModelState.ToDataSourceResult());
            return Json(educationgridmodel);
            //return Json(new[] { registrationViewModel }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult Edit_experience([DataSourceRequest]DataSourceRequest request, ExperienceGridModel experiencegridmodel)
        {
            LocatorContext context = new LocatorContext();
            var tempkendoStartDate = Request["SD"];
            DateTime SD = Convert.ToDateTime(tempkendoStartDate);
            experiencegridmodel.JoinStartDate = SD;
            var tempkendoEndDate = Request["ED"];
            DateTime ED = Convert.ToDateTime(tempkendoEndDate);
            experiencegridmodel.JoinEndDate = ED;



            var user = db.DBContext_physicianeducation.Where(u => u.UserId == experiencegridmodel.ExpId).FirstOrDefault();

            var id = experiencegridmodel.ExpId;
            PhysicianExperienceViewModel phyexperience_query = (from physicainexperience in context.DBContext_experience where physicainexperience.PhysicianExperienceId == id select physicainexperience).SingleOrDefault();
            if (phyexperience_query != null)
            {

                phyexperience_query.Designation = experiencegridmodel.Designation;
                phyexperience_query.ReasonForLeaving = experiencegridmodel.ReasonForLeaving;
                phyexperience_query.JoinStartDate = experiencegridmodel.JoinStartDate;
                phyexperience_query.JoinEndDate = experiencegridmodel.JoinEndDate;
                try
                {
                    context.SaveChanges();
                    TempData["Data"] = "Demerged Successful";
                    return View("../Shared/Error");
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                }

            }
            //}
            ModelState.Clear();
            //return Json(ModelState.ToDataSourceResult());
            return Json(experiencegridmodel);
            //return Json(new[] { registrationViewModel }.ToDataSourceResult(request, ModelState));
        }
        public ActionResult Edit_health([DataSourceRequest]DataSourceRequest request, HealthGridModel healthgridmodel, HttpPostedFileBase uploadFile)
        {
            LocatorContext context = new LocatorContext();

            DocumentViewModel doc = new DocumentViewModel();

            //educationgridmodel.CurrentEducationInstituteId = Convert.ToInt32(currentassign.FirstOrDefault().CurrentAssignId);
            var tempkendoStartDate = Request["SD"];
            DateTime SD = Convert.ToDateTime(tempkendoStartDate);
            healthgridmodel.StartDate = SD;
            var tempkendoEndDate = Request["ED"];
            DateTime ED = Convert.ToDateTime(tempkendoEndDate);
            healthgridmodel.EndDate = ED;



            var user = db.DBContext_healthhistory.Where(u => u.HealthHistoryId == healthgridmodel.HealthId).FirstOrDefault();

            var id = healthgridmodel.HealthId;
            HealthHistoryViewModel health_query = (from health in context.DBContext_healthhistory where health.HealthHistoryId == id select health).SingleOrDefault();
            DocumentViewModel document_query = (from document in context.DBContext_document where document.PrimaryKeyId == id && document.PrimaryObjectId == "tblHealthHistories" select document).SingleOrDefault();


            if (health_query != null)
            {

                health_query.Type = healthgridmodel.Type;
                health_query.Description = healthgridmodel.Description;
                health_query.StartDate = healthgridmodel.StartDate;
                health_query.EndDate = healthgridmodel.EndDate;
                try
                {
                    context.SaveChanges();
                    TempData["Data"] = "Demerged Successful";
                    return View("../Shared/Error");
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                }

                //}
                ModelState.Clear();
                //return Json(ModelState.ToDataSourceResult());

                //return Json(new[] { registrationViewModel }.ToDataSourceResult(request, ModelState));
            }
            return Json(healthgridmodel);
        }
        public ActionResult Delete_education([DataSourceRequest]DataSourceRequest request, EducationGridModel model)
        {
            LocatorContext context = new LocatorContext();
            var id = model.EduId;
            var deleteOrderDetails = context.DBContext_physicianeducation.Where(a => a.PhysicianEducationId == id);
            if (deleteOrderDetails != null)
            {
                context.DBContext_physicianeducation.RemoveRange(deleteOrderDetails);
                context.SaveChanges();
            }
            return Json(model);
        }
        public ActionResult Delete_experience([DataSourceRequest]DataSourceRequest request, ExperienceGridModel model)
        {
            LocatorContext context = new LocatorContext();
            var id = model.ExpId;
            var deleteOrderDetails = context.DBContext_experience.Where(a => a.PhysicianExperienceId == id);
            if (deleteOrderDetails != null)
            {
                context.DBContext_experience.RemoveRange(deleteOrderDetails);
                context.SaveChanges();
            }
            return Json(model);
        }
        public ActionResult UpdatePerson([DataSourceRequest] DataSourceRequest dsRequest, RegistrationViewModel person)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (LocatorContext context = new LocatorContext())
                    {
                        var user = context.DBContext_register.Where(u => u.UserName == person.UserName).FirstOrDefault();
                        if (user == null)
                        {
                            return View("../Shared/Error");
                        }
                        else
                        {
                            CountryViewModels con = new CountryViewModels();

                            RegistrationViewModel register = new RegistrationViewModel();
                            register = context.DBContext_register.Find(user.UserId);
                            /////////////
                            //var students = (from p in context.DBContext_register
                            //                join f in context.DBContext_country
                            //            on p.CountryId equals f.ID
                            //                select new
                            //                {

                            //                    Name = f.CountryName,

                            //                }).SingleOrDefault();
                            //string dd = students.ToString();
                            //con = context.DBContext_country.Find(students.Name);                 
                            ////////////

                            register.FirstName = person.FirstName;
                            //register.RoleId = person.RoleId;
                            register.UserName = person.UserName;
                            //register.WhoAreYou = person.WhoAreYou;
                            //register.FirstName = person.FirstName;
                            register.LastName = person.LastName;
                            //register.Gender = person.Gender;
                            //register.DOB = person.DOB;
                            //register.ContactNo = person.ContactNo;
                            //register.Email = person.Email;
                            //register.Street1 = person.Street1;
                            //register.Street2 = person.Street2;
                            //register.City = person.City;
                            //register.State = person.State;
                            //register.CountryId = person.CountryId;
                            //register.PostalCode = person.PostalCode;
                            //register.Landmark = person.Landmark;
                            //register.IMAI = person.IMAI;

                            context.Entry(register).State = EntityState.Modified;
                            context.SaveChanges();

                        }
                    }
                }
                ModelState.Clear();
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                //   ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return Json(ModelState.ToDataSourceResult());
        }
        public ActionResult Delete_health([DataSourceRequest]DataSourceRequest request, HealthGridModel healthgridmodel)
        {
            LocatorContext context = new LocatorContext();
            var id = healthgridmodel.HealthId;
            var deleteOrderDetails = context.DBContext_healthhistory.Where(a => a.HealthHistoryId == id);
            if (deleteOrderDetails != null)
            {
                context.DBContext_healthhistory.RemoveRange(deleteOrderDetails);
                context.SaveChanges();
            }
            return Json(healthgridmodel);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
