using PhysicianLocator.DAL;
using System;
using System.Linq;
using System.Web.Mvc;
using PhysicianLocator.Models;

namespace PhysicianLocator.Controllers
{

    public class ManageEmailController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ManageEmailController));  //Declaring Log4Net
        LocatorContext context = new LocatorContext();
        // GET: EmailManipulation
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetEmailTemplateList()
        {            
            {
                var meds = context.DBContext_emailtemplate.Where(a=>a.IsActive).ToList();

                return Json(new
                {
                    data = meds
                }, JsonRequestBehavior.AllowGet);

            }
       
        }

        public ActionResult Save_email()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Save_email(EmailTemplateViewModel model,int id)
        {
            var queryResult = context.DBContext_emailtemplate.Where(u => u.templateCode == model.templateCode).FirstOrDefault();
            //var kendoEditor = Request["emaileditor"];
            //model.templateContent = kendoEditor;
            if (ModelState.IsValid)
            {

                if (queryResult == null)
                {
                    var record = new EmailTemplateViewModel()
                    {
                        templateCode = model.templateCode,
                        templateContent = model.templateContent,
                        From = model.From,
                        Cc = model.Cc,
                        Bcc = model.Bcc,
                        Subject=model.Subject,
                        IsActive = model.IsActive,
                        IsDeleted = model.IsDeleted,
                        CreatedBy = 0,
                        CreatedOn = DateTime.Now,
                        LastModifiedBy = 0,
                        LastModifiedOn = DateTime.Now,
                    };
                }
                context.DBContext_emailtemplate.Add(model);
                context.SaveChanges();
        }
            else
            {
                ModelState.AddModelError("", "This Email is already registered");
                TempData["Data"] = "This Email is already registered";
                return View("../Shared/Error");
    }
            return RedirectToAction("Index", "ManageEmail"/*, new {@id = userid}*/);
        }

        public ActionResult Edit_email(int id)
        {

            EmailTemplateViewModel email_query = (from email in context.DBContext_emailtemplate where email.templateid == id select email).SingleOrDefault();
                return View(email_query);
            
        }
        [HttpPost]
        public ActionResult Edit_email(EmailTemplateViewModel model,int id)
        {
            EmailTemplateViewModel email_query = (from email in context.DBContext_emailtemplate where email.templateid == id select email).SingleOrDefault();

            if (ModelState.IsValid)
            {
                //var kendoEditor = Request["emaileditor"];
                //model.templateContent = kendoEditor;

                if (email_query != null)
                {

                    email_query.templateContent = model.templateContent;
                    email_query.From = model.From;
                    email_query.Cc = model.Cc;
                    email_query.Bcc = model.Bcc;
                    email_query.Subject = model.Subject;
                    email_query.CreatedBy = model.CreatedBy;
                    email_query.LastModifiedOn = DateTime.Now;
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex) { logger.Error(ex.ToString()); }
                }
            }

            return RedirectToAction("Index", "ManageEmail"/*, new {@id = userid}*/);
        }
    }
}