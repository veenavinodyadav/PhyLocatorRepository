using PhysicianLocator.DAL;
using PhysicianLocator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace PhysicianLocator.Controllers
{
    public class AdministratorController : Controller
    {
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
            List<EducationInstitutesViewModel> list = new List<EducationInstitutesViewModel>();
            using (LocatorContext dc = new LocatorContext())
            {
                var query = dc.DBContext_educationinstitute;
                list = query.ToList();
                return Json(new
                {
                    data = list
                }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}
