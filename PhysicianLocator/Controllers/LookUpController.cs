using PhysicianLocator.DAL;
using PhysicianLocator.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using System.Data;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using System.Threading;
using System.Data.Entity.Core.EntityClient;

namespace PhysicianLocator.Controllers
{

    public class LookUpController : Controller
    {
        SqlCacheDependency dep = null;
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CommonFunction));  //Declaring Log4Net

        // GET: LookUp
        [CheckSessionOut]
        public ActionResult LookUpCountry()
        {
            return View();
        }
        [CheckSessionOut]
        public ActionResult LookUpCategory()
        {
            return View();
        }
        [CheckSessionOut]
        public ActionResult LkUpCountryView()
        {

            var result = HttpContext.Cache["CountriesData"];
            if (result == null)
            {
                using (LocatorContext context = new LocatorContext())
                {
                    var query_country = (context.DBContext_country.Where(a => !a.IsDeleted)).ToList();
                    try
                    {
                        dep = new SqlCacheDependency("salcachedependencyMvc", "tblMSTCountry");
                    }
                    catch (DatabaseNotEnabledForNotificationException exDBDis)
                    {
                        try
                        {
                            SqlCacheDependencyAdmin.EnableNotifications("LocatorContext");
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
                            SqlCacheDependencyAdmin.EnableTableForNotifications("LocatorContext", "tblMSTCountry");
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
                        HttpContext.Cache.Insert("CountriesData", query_country, dep);
                        ViewBag.Message = "Database";
                        context.Dispose();
                    }
                    
                    return Json(new
                    {
                        data = query_country
                    }, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                ViewBag.Message = "Cache";
            }
            return Json(new
            {
                data = result
            }, JsonRequestBehavior.AllowGet);

           
        }
        [CheckSessionOut]
        public ActionResult LkUpCountryCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LkUpCountryCreate(CountryViewModels model)
        {
            int userid = Convert.ToInt32(Session["UserID"]);
            model.CreatedBy = userid;
            if (ModelState.IsValid)
            {
                using (LocatorContext context = new LocatorContext())
                {
                    var user = context.DBContext_country.Where(u => u.CountryName == model.CountryName).FirstOrDefault();
                    if (user == null)
                    {
                        var record = new CountryViewModels()
                        {
                            CountryName = model.CountryName,
                            IsActive = model.IsActive,
                            CreatedBy = model.CreatedBy,
                            CreatedOn = DateTime.Now,
                            LastModifiedBy = 0,
                            LastModifiedOn = DateTime.Now,
                        };
                        context.DBContext_country.Add(model);
                        context.SaveChanges();
                        TempData["Data"] = "New country name inserted";
                        return RedirectToAction("LookUpCategory", "LookUp");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Country name already present");
                        TempData["Data"] = "Country name already present";
                        return View("../Shared/Error");
                    }
                }
            }
            ModelState.Clear();

            return View(model);
        }
        [CheckSessionOut]
        public ActionResult LkUpCountryEdit(int id)
        {
            var result = HttpContext.Cache["CountriesEdit"];
            if (result == null)
            {
                using (LocatorContext context = new LocatorContext())
                {
                    var query_country = context.DBContext_country.Find(id);
                    try
                    {
                        dep = new SqlCacheDependency("salcachedependencyMvc", "tblMSTCountry");
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
                            SqlCacheDependencyAdmin.EnableTableForNotifications("LocatorContext1", "tblMSTCountry");
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
            //return View( context.DBContext_country.Find(id));
            //return RedirectToAction("ViewUserProfile", "ManageUser", new
            //{
            //    id = userid
            //});

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LkUpCountryEdit(CountryViewModels model, int id)
        {
            int userid = Convert.ToInt32(Session["UserID"]);
            LocatorContext context = new LocatorContext();
            if (ModelState.IsValid)
            {
                CountryViewModels query_country = (from country in context.DBContext_country where country.CountryId == id select country).FirstOrDefault();
                query_country.CountryName = model.CountryName;
                query_country.IsActive = model.IsActive;
                query_country.LastModifiedBy = userid;
                context.SaveChanges();
                Thread.Sleep(3000);
            }
            ModelState.Clear();        
            return RedirectToAction("LookUpCountry", "LookUp");
        }
        [CheckSessionOut]
        public ActionResult LkUpCountryDelete( int id)
        {
            return View();
        }
        [CheckSessionOut]
        [HttpPost]
        public ActionResult LkUpCountryDelete(CountryViewModels model, int id)
        {
            LocatorContext context = new LocatorContext();
            var isdelete = true;
            model.IsDeleted = isdelete;
            int userid = Convert.ToInt32(Session["UserID"]);
            CountryViewModels query_country = (from country in context.DBContext_country where country.CountryId == id select country).FirstOrDefault();
            if (query_country != null)
            {
                query_country.IsDeleted = model.IsDeleted;
                query_country.LastModifiedBy = userid;
                model.CountryName = query_country.CountryName;
                model.CountryId = query_country.CountryId;
            }
            context.SaveChanges();
            return RedirectToAction("LookUpCountry", "LookUp");
        }
        /* for remove
        public ActionResult LkUpCountryDelete(CountryViewModels model,int id)
        {
             model=  context.DBContext_country.Find(id);
           
            context.DBContext_country.Remove(model);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        */
        [CheckSessionOut]
        public ActionResult LkUpCategoryView()
        {
            SqlCacheDependency cat = null;
            var result = HttpContext.Cache["CategoriesData"];
            if (result == null)
            {
                using (LocatorContext context = new LocatorContext())
                {
                    var query_category = (context.DBContext_lookupcategory.Where(a => !a.IsDeleted)).ToList();
                    try
                    {
                        cat = new SqlCacheDependency("salcachedependencyMvc", "tblMSTLookupCategory");
                    }
                    catch (DatabaseNotEnabledForNotificationException exDBDis)
                    {
                        try
                        {
                            SqlCacheDependencyAdmin.EnableNotifications("LocatorContext");
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
                            SqlCacheDependencyAdmin.EnableTableForNotifications("LocatorContext", "tblMSTLookupCategory");
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
                        cat = new SqlCacheDependency("salcachedependencyMvc", "tblMSTLookupCategory");
                        HttpContext.Cache.Insert("CategoriesData", query_category, cat);
                        ViewBag.Message = "Database";
                        context.Dispose();
                    }
                 
                    return Json(new
                    {
                        data = query_category
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                ViewBag.Message = "Cache";
            }
           
            return Json(new
            {
                data = result
            }, JsonRequestBehavior.AllowGet);
        }
        [CheckSessionOut]
        public ActionResult LkUpCategoryCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LkUpCategoryCreate(LookupCategoryViewModel model)
        {
            LocatorContext context = new LocatorContext();
            int userid = Convert.ToInt32(Session["UserID"]);
            model.CreatedBy = userid;
            if (ModelState.IsValid)
            {
                using (context = new LocatorContext())
                {
                    var user = context.DBContext_lookupcategory.Where(u => u.Category == model.Category).FirstOrDefault();

                    if (user == null)
                    {
                        var record = new LookupCategoryViewModel()
                        {
                            Category = model.Category,
                            IsActive = model.IsActive,
                            CreatedBy = model.CreatedBy,
                            CreatedOn = DateTime.Now,
                            LastModifiedBy = 0,
                            LastModifiedOn = DateTime.Now,
                        };
                        context.DBContext_lookupcategory.Add(model);
                        context.SaveChanges();
                        TempData["Data"] = "New Category  inserted";
                        return RedirectToAction("LookUpCategory", "LookUp");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Category name already exist");
                        TempData["Data"] = "This Category already exit";
                        return View("../Shared/Error");
                    }
                }
            }
            ModelState.Clear();
            return View(model);
        }
        [CheckSessionOut]
        public ActionResult LkUpCategoryEdit(int id)
        {
            
            var result = HttpContext.Cache["CategoriesEdit"];
            if (result == null)
            {
                using (LocatorContext context = new LocatorContext())
                {
                    var query_categories = context.DBContext_lookupcategory.Find(id);
                    try
                    {
                        dep = new SqlCacheDependency("salcachedependencyMvc", "tblMSTLookupCategory");
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
                            SqlCacheDependencyAdmin.EnableTableForNotifications("LocatorContext1", "tblMSTCountry");
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
                        HttpContext.Cache.Insert("CategoriesEdit", query_categories, dep);
                        ViewBag.Message = "Database";
                        context.Dispose();
                    }
                    return View(query_categories);
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
        public ActionResult LkUpCategoryEdit(LookupCategoryViewModel model, int id)
        {

            LocatorContext context = new LocatorContext();
            int userid = Convert.ToInt32(Session["UserID"]);
            if (ModelState.IsValid)
            {
                LookupCategoryViewModel query_category = (from category in context.DBContext_lookupcategory where category.LookupCategoryId == id select category).FirstOrDefault();
                query_category.Category = model.Category;
                query_category.IsActive = model.IsActive;
                query_category.LastModifiedBy = userid;
                context.SaveChanges();
                Thread.Sleep(3000);
            }
            ModelState.Clear();
            return RedirectToAction("LookUpCategory", "LookUp");
        }
        [CheckSessionOut]
        public ActionResult LkUpCategoryDelete( int id)
        {
            return View();
        }
        [CheckSessionOut]
        [HttpPost]
        public ActionResult LkUpCategoryDelete(LookupCategoryViewModel model, int id)
        {
            LocatorContext context = new LocatorContext();
            //model = context.DBContext_country.Find(id);
            int userid = Convert.ToInt32(Session["UserID"]);
            var isdelete = true;
            model.IsDeleted = isdelete;

            LookupCategoryViewModel query_category = (from category in context.DBContext_lookupcategory where category.LookupCategoryId == id select category).FirstOrDefault();

            if (query_category != null)
            {
                query_category.IsDeleted = model.IsDeleted;
                query_category.LastModifiedBy = userid;
                model.Category = query_category.Category;

            }


            context.SaveChanges();
            return RedirectToAction("LookUpCategory", "LookUp");
        }
        [CheckSessionOut]
        public ActionResult LkUpDetailsView()
        {


            List<TempDetailModel> List = new List<TempDetailModel>();
            //var meds = from category in context.DBContext_lookupcategory
            //           join detail in context.DBContext_lookupdetail
            //           on category.LookupCategoryId equals detail.LookupCategoryId
            //           where !detail.IsDeleted

            //           select new TempDetailModel
            //           {
            //               Lookupdetailid = detail.LookupDetailId,
            //               Category = category.Category,
            //               SortOrder = detail.SortOrder,
            //               Key = detail.Key,
            //               Value = detail.Value,
            //               CreatedBy = detail.CreatedBy,
            //               CreatedOn = detail.CreatedOn,
            //               IsActive = detail.IsActive,
            //               IsDeleted = detail.IsDeleted,
            //               LastModifiedBy = detail.LastModifiedBy,
            //               LastModifiedOn = detail.LastModifiedOn,
            //           };


            //List = meds.ToList();
            //return View(List);


            SqlCacheDependency cat = null;
            var result = HttpContext.Cache["DetailsData"];
            if (result == null)
            {
                using (LocatorContext context = new LocatorContext())
                {
                    var query_Details = from category in context.DBContext_lookupcategory
                                        join detail in context.DBContext_lookupdetail
                                        on category.LookupCategoryId equals detail.LookupCategoryId
                                        where !detail.IsDeleted

                                        select new TempDetailModel
                                        {
                                            Lookupdetailid = detail.LookupDetailId,
                                            Category = category.Category,
                                            SortOrder = detail.SortOrder,
                                            Key = detail.Key,
                                            Value = detail.Value,
                                            CreatedBy = detail.CreatedBy,
                                            CreatedOn = detail.CreatedOn,
                                            IsActive = detail.IsActive,
                                            IsDeleted = detail.IsDeleted,
                                            LastModifiedBy = detail.LastModifiedBy,
                                            LastModifiedOn = detail.LastModifiedOn,
                                        };


                    List = query_Details.ToList();
                    try
                    {
                        cat = new SqlCacheDependency("salcachedependencyMvc", "tblMSTLookupCategory");
                    }
                    catch (DatabaseNotEnabledForNotificationException exDBDis)
                    {
                        try
                        {
                            SqlCacheDependencyAdmin.EnableNotifications("LocatorContext");
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
                            SqlCacheDependencyAdmin.EnableTableForNotifications("LocatorContext", "tblMSTLookupDetail");
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
                        cat = new SqlCacheDependency("salcachedependencyMvc", "tblMSTLookupDetail");
                        HttpContext.Cache.Insert("DetailsData", List, cat);
                        ViewBag.Message = "Database";
                        context.Dispose();
                    }
                    return View(List);
                }
            }
            else
            {
                ViewBag.Message = "Cache";
            }
            return View(result);
        }
        [CheckSessionOut]
        public ActionResult LkUpDetailsCreate()
        {
            LocatorContext db = new LocatorContext();



            IEnumerable<SelectListItem> item8 = db.DBContext_lookupcategory.Where(u => u.IsActive).Select(
            b => new SelectListItem { Value = b.LookupCategoryId.ToString(), Text = b.Category });
            ViewData["category"] = item8;
            return View(new LookupDetailViewModel());

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LkUpDetailsCreate(LookupDetailViewModel model)
        {
            LocatorContext context = new LocatorContext();
            int userid = Convert.ToInt32(Session["UserID"]);
            var category = Convert.ToInt16(Request.Form["category"]);
            model.LookupCategoryId = category;
            model.CreatedBy = userid;
            if (ModelState.IsValid)
            {
                using (context = new LocatorContext())
                {

                    var record = new LookupDetailViewModel()
                    {

                        LookupCategoryId = model.LookupCategoryId,
                        Key = model.Key,
                        Value = model.Value,
                        SortOrder = model.SortOrder,
                        IsActive = model.IsActive,
                        IsDeleted = model.IsDeleted,
                        CreatedBy = model.CreatedBy,
                        CreatedOn = DateTime.Now,
                        LastModifiedBy = 0,
                        LastModifiedOn = DateTime.Now,
                    };
                    context.DBContext_lookupdetail.Add(model);
                    context.SaveChanges();
                    TempData["Data"] = "New  detail  inserted";
                    return RedirectToAction("LkUpDetailsView");

                }
            }
            ModelState.Clear();


            return View(model);
        }
        [CheckSessionOut]
        public ActionResult LkUpDetailsDelete(LookupDetailViewModel model, int id)
        {
            LocatorContext context = new LocatorContext();
            //model = context.DBContext_country.Find(id);
            int userid = Convert.ToInt32(Session["UserID"]);
            var isdelete = true;
            model.IsDeleted = isdelete;

            LookupDetailViewModel detail_query = (from deatail in context.DBContext_lookupdetail where deatail.LookupDetailId == id select deatail).SingleOrDefault();

            if (detail_query != null)
            {
                detail_query.IsDeleted = model.IsDeleted;
                detail_query.LastModifiedBy = userid;
            }
            context.SaveChanges();
            return RedirectToAction("LkUpDetailsView");
        }
        [CheckSessionOut]
        public ActionResult LkUpDetailsEdit(int id)
        {
            LocatorContext context = new LocatorContext();
            LocatorContext db = new LocatorContext();
            LookupDetailViewModel detail_query = (from deatail in db.DBContext_lookupdetail where deatail.LookupDetailId == id select deatail).SingleOrDefault();

            ViewBag.select = new SelectList(db.DBContext_lookupcategory, "LookupCategoryId", "Category", detail_query.LookupCategoryId);
            return View(context.DBContext_lookupdetail.Find(id));

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LkUpDetailsEdit(LookupDetailViewModel model, int id)
        {
            LocatorContext context = new LocatorContext();

            var category = Convert.ToInt16(Request.Form["select"]);
            model.LookupCategoryId = category;


            LookupDetailViewModel detail_query = (from deatail in context.DBContext_lookupdetail where deatail.LookupDetailId == id select deatail).SingleOrDefault();


            int userid = Convert.ToInt32(Session["UserID"]);
            if (ModelState.IsValid)
            {
                detail_query.LookupCategoryId = model.LookupCategoryId;
                detail_query.SortOrder = model.SortOrder;
                detail_query.IsActive = model.IsActive;
                detail_query.Key = model.Key;
                detail_query.Value = model.Value;
                detail_query.LastModifiedBy = userid;
                context.SaveChanges();
                return RedirectToAction("LkUpDetailsView");
            }
            ModelState.Clear();
            return View(model);
        }
    }
}