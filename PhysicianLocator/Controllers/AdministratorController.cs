using PhysicianLocator.DAL;
using PhysicianLocator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebGrease.Css.Extensions;
using System.Data.SqlClient;

using System.Data;
using System.Web.Caching;
using System.Threading;

namespace PhysicianLocator.Controllers
{
    public class AdministratorController : Controller
    {
        SqlCacheDependency dep = null;
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AdministratorController));  //Declaring Log4Net
        // GET: Administrator
        [CheckSessionOut]
        public ActionResult MergeInstitute()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MergeInstitute(MergeInstitute model)
        {
            if (ModelState.IsValid)
            {
                EducationInstitutesViewModel education = new EducationInstitutesViewModel();
                var context = new LocatorContext();
                var tempSearch = Request["required"];
                var result = model.AssignName;
                education.InstituteName = result;
                string s = Convert.ToString(tempSearch);
                if (tempSearch != null && tempSearch != string.Empty)
                {
                    string[] a = tempSearch.Split(',');

                    var institute = context.DBContext_educationinstitute.Where(u => u.InstituteName == result).FirstOrDefault();
                    int lastcurrentAssignId = context.DBContext_educationinstitute.Max(item => item.EducationInstituteId);
                    education.CurrentAssignId = lastcurrentAssignId + 1;
                    var isactive = true;


                    if (institute == null)
                    {
                        education.IsActive = isactive;
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
                        for (int i = 0; i < a.Length; i++)
                        {
                            var temp = a[i];
                            int assign = context.DBContext_educationinstitute.Max(item => item.EducationInstituteId);
                            var currentassign = from educationinstitute in context.DBContext_educationinstitute
                                                where (educationinstitute.InstituteName.Equals(result))
                                                select new
                                                {
                                                    educationinstitute.CurrentAssignId
                                                };
                            education.CurrentAssignId = Convert.ToInt32(currentassign.FirstOrDefault().CurrentAssignId);
                            EducationInstitutesViewModel query = (from ord in context.DBContext_educationinstitute where ord.InstituteName == temp select ord).SingleOrDefault();
                            query.CurrentAssignId = education.CurrentAssignId;
                            query.IsActive = false;

                            try
                            {
                                context.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex.ToString());
                            }
                            PhysicianEducationViewModel phy_query = (from physicaineducation in context.DBContext_physicianeducation where physicaineducation.OldEducationInstituteId == education.EducationInstituteId select physicaineducation).SingleOrDefault();
                            if (phy_query != null)
                            {
                                phy_query.CurrentEducationInstituteId = education.CurrentAssignId;

                            }
                            try
                            {
                                context.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex.ToString());
                                // Provide for exceptions.
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < a.Length; i++)
                        {
                            var temp = a[i];
                            var currentassign = from educationinstitute in context.DBContext_educationinstitute
                                                where (educationinstitute.InstituteName.Equals(result))
                                                select new
                                                {
                                                    educationinstitute.EducationInstituteId
                                                };

                            education.CurrentAssignId = Convert.ToInt32(currentassign.FirstOrDefault().EducationInstituteId);
                            EducationInstitutesViewModel query_isactive = (from ord in context.DBContext_educationinstitute where ord.InstituteName == result select ord).SingleOrDefault();


                            EducationInstitutesViewModel query = (from ord in context.DBContext_educationinstitute where ord.InstituteName == temp select ord).SingleOrDefault();
                            query.CurrentAssignId = education.CurrentAssignId;
                            query.IsActive = false;
                            query_isactive.IsActive = true;
                            query_isactive.CurrentAssignId = query_isactive.EducationInstituteId;

                            try
                            {
                                context.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex.ToString());
                                // Provide for exceptions.
                            }
                            var phy_query = (from physicaineducation in context.DBContext_physicianeducation where physicaineducation.OldEducationInstituteId == query.EducationInstituteId select physicaineducation);




                            if (phy_query != null)
                            {
                                phy_query.ForEach(u => u.CurrentEducationInstituteId = education.CurrentAssignId);
                                //phy_query.CurrentEducationInstituteId = education.CurrentAssignId;
                            }

                            try
                            {
                                context.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                logger.Error(ex.ToString());
                                // Provide for exceptions.
                            }
                            ViewBag.message = "successfully";
                        }
                    }
                }

                ModelState.Clear();
            }

            return View(model);
        }
        [CheckSessionOut]
        public ActionResult DemergeInstitute()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DemergeInstitute(MergeInstitute model)
        {
            List<PhysicianEducationViewModel> questionList = new List<PhysicianEducationViewModel>();
            EducationInstitutesViewModel education = new EducationInstitutesViewModel();
            var context = new LocatorContext();
            var tempSearch = Request["required"];
            string s = Convert.ToString(tempSearch);
            if (tempSearch != null && tempSearch != string.Empty)
            {
                string[] a = tempSearch.Split(',');

                for (int i = 0; i < a.Length; i++)
                {
                    var temp = a[i];
                    int assign = context.DBContext_educationinstitute.Max(item => item.EducationInstituteId);
                    EducationInstitutesViewModel edu_query = (from ord in context.DBContext_educationinstitute where ord.InstituteName == temp select ord).SingleOrDefault();
                    var phy_query = (from physicaineducation in context.DBContext_physicianeducation where physicaineducation.CurrentEducationInstituteId == edu_query.CurrentAssignId select physicaineducation);
                    if (phy_query != null)
                    {
                        questionList = phy_query.ToList();
                        foreach (var item in phy_query) //retrieve each item and assign to model
                        {
                            item.CurrentEducationInstituteId = item.OldEducationInstituteId;
                        }
                    }
                    edu_query.CurrentAssignId = edu_query.EducationInstituteId;
                    edu_query.IsActive = true;
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
            }
            ViewBag.message = "successfully";
            return View();
        }

     
        public ActionResult ManageInstitute()
        {
            return View();
        }
        public ActionResult GetInstituteList()
        {
            LocatorContext db = new LocatorContext();
            int userid = Convert.ToInt32(Session["UserID"]);
            List<EducationInstitutesViewModel> questionList = new List<EducationInstitutesViewModel>();
            //IQueryable<PhysicianEducationViewModel> tblregistrations = db.DBContext_physicianeducation;

            LocatorContext context = new LocatorContext();
            {
              
                var meds = (context.DBContext_educationinstitute.Where(a => !a.IsDeleted)).ToList();

                {
                    questionList = meds.ToList();
                }

                return Json(new
                {
                    data = questionList
                }, JsonRequestBehavior.AllowGet);


            }
        }
   
        public ActionResult InstituteCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InstituteCreate(EducationInstitutesViewModel model)
        {
             model = new EducationInstitutesViewModel();
            int userid = Convert.ToInt32(Session["UserID"]);
            model.CreatedBy = userid;
            var instituteName = Request["kendoInstitute"];
            model.InstituteName = instituteName;
           
                using (LocatorContext context = new LocatorContext())
                {
                    var user = context.DBContext_educationinstitute.Where(u => u.InstituteName == model.InstituteName).FirstOrDefault();
                    if (user == null)
                    {
                        
                        int lastcurrentAssignId = context.DBContext_educationinstitute.Max(item => item.EducationInstituteId);
                        model.CurrentAssignId = lastcurrentAssignId;
                        var record = new EducationInstitutesViewModel()
                        {
                            InstituteName = model.InstituteName,
                            CurrentAssignId=model.CurrentAssignId,
                            IsActive = model.IsActive,
                            CreatedBy = model.CreatedBy,
                            CreatedOn = DateTime.Now,
                            LastModifiedBy = 0,
                            LastModifiedOn = DateTime.Now,
                        };
                        context.DBContext_educationinstitute.Add(model);
                        context.SaveChanges();
                        TempData["Data"] = "New institute name inserted";
                        return RedirectToAction("ManageInstitute", "Administrator");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Institute name already present");
                        TempData["Data"] = "Institute name already present";
                        return View("../Shared/Error");
                    }
                }
            
          

            return RedirectToAction("ManageInstitute", "Administrator");
        }

        [CheckSessionOut]
        public ActionResult InstituteEdit(int id)
        {
            var result = HttpContext.Cache["CountriesEdit"];
            if (result == null)
            {
                using (LocatorContext context = new LocatorContext())
                {
                    var query_country = context.DBContext_educationinstitute.Find(id);

                    try
                    {
                        dep = new SqlCacheDependency("salcachedependencyMvc", "tblMSTEducationInstitutes");
                    }
                    catch (DatabaseNotEnabledForNotificationException exDBDis)
                    {
                        try
                        {
                            SqlCacheDependencyAdmin.EnableNotifications("LocatorContext1");
                        }
                        catch (UnauthorizedAccessException exPerm)
                        {
                            logger.Error(exPerm.ToString());
                            TempData["Data"] = "Error.";
                            return View("../Shared/Error");
                        }
                    }
                    catch (TableNotEnabledForNotificationException exTabDis)
                    {
                        try
                        {
                            logger.Error(exTabDis.ToString());
                            SqlCacheDependencyAdmin.EnableTableForNotifications("LocatorContext1", "tblMSTEducationInstitutes");
                        }
                        catch (SqlException exc)
                        {
                            logger.Error(exc.ToString());
                            TempData["Data"] = "Error.";
                            return View("../Shared/Error");
                        }
                    }
                    finally
                    {
                        //HttpContext.Cache.Insert("CountriesData", query_country, dep, DateTime.Now.AddSeconds(60), Cache.NoSlidingExpiration);
                        HttpContext.Cache.Insert("CountriesEdit", query_country, dep);
                        ViewBag.Message = "Database";
                        context.Dispose();
                    }
                    return View(query_country);
                }
            }
            else
            {
                ViewBag.Message = "Cache";
            }
            return View(result);
            

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InstituteEdit(EducationInstitutesViewModel model, int id)
        {
            model = new EducationInstitutesViewModel();
            int userid = Convert.ToInt32(Session["UserID"]);
            model.CreatedBy = userid;
            var instituteName = Request["kendoInstitute"];
            model.InstituteName = instituteName;
            LocatorContext context = new LocatorContext();
         
            
                EducationInstitutesViewModel query_institute = (from institute in context.DBContext_educationinstitute where institute.EducationInstituteId == id select institute).FirstOrDefault();
                query_institute.InstituteName = model.InstituteName;
                query_institute.IsActive = model.IsActive;
              
                query_institute.LastModifiedBy = userid;
                context.SaveChanges();
                Thread.Sleep(3000);
            
            ModelState.Clear();
            return RedirectToAction("ManageInstitute", "Administrator");
        }


        [CheckSessionOut]
        public ActionResult InstituteDelete(int id)
        {
            return View();
        }
        [CheckSessionOut]
        [HttpPost]
        public ActionResult InstituteDelete(EducationInstitutesViewModel model, int id)
        {
            LocatorContext context = new LocatorContext();
            var isdelete = true;
            model.IsDeleted = isdelete;
            int userid = Convert.ToInt32(Session["UserID"]);
            EducationInstitutesViewModel query_institute = (from institute in context.DBContext_educationinstitute where institute.EducationInstituteId == id select institute).FirstOrDefault();
            if (query_institute != null)
            {
                query_institute.IsDeleted = model.IsDeleted;
                query_institute.LastModifiedBy = userid;
               
            }
            context.SaveChanges();
            return RedirectToAction("ManageInstitute", "Administrator");
        }

    }
}
