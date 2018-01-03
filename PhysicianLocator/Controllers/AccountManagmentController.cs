using PhysicianLocator.DAL;
using PhysicianLocator.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PhysicianLocator.Controllers
{    
    public class AccountManagmentController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(AccountManagmentController));  //Declaring Log4Net
        int i = 0;
        // GET: AccountManagment      
        public ActionResult Index()
        {
            return View();
        }     
        public ActionResult Register()
        {
            return View();
        }
        // POST: /Account/Register
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Register(RegistrationViewModel model)
        {            
            string pwd = "";
            var roleId = 3;
            var isactive = true;
            model.RoleId = Convert.ToInt32(roleId);
            string activationCode = "";
            CommonFunction commonFunction = new CommonFunction();
            activationCode = commonFunction.MakePassword(10);
            model.ActivationCode = activationCode;
            int a = model.GenderId;
            if (ModelState.IsValid)
            {
                using (LocatorContext context = new LocatorContext())
                {
                    var user = context.DBContext_register.Where(u => u.UserName == model.UserName).FirstOrDefault();
                    pwd = model.Password;
                    pwd = commonFunction.Encrypt(pwd);
                    model.Password = pwd;
                    model.IsActive = isactive;
                    var tempDob = Request["kendoDOB"];
                    var whoAreYou = "Patient";
                    model.WhoAreYou = whoAreYou;
                    DateTime dateOfBirth = Convert.ToDateTime(tempDob);
                    model.DateOfBirth = dateOfBirth;
                    if (user == null)
                    {
                        if (model.GenderId == 1)
                        {
                            var meds = from lookupcategory in context.DBContext_lookupcategory
                                       join lookupdetail in context.DBContext_lookupdetail
                                       on lookupcategory.LookupCategoryId equals lookupdetail.LookupCategoryId
                                       where (lookupdetail.Value.Equals("Female"))
                                       select new
                                       {
                                           lookupdetail.LookupDetailId
                                       };
                            model.GenderId = Convert.ToInt32(meds.FirstOrDefault().LookupDetailId);
                            //model.GenderId = 1;
                        }
                        else
                        {
                            var meds = from lookupcategory in context.DBContext_lookupcategory
                                       join lookupdetail in context.DBContext_lookupdetail
                                       on lookupcategory.LookupCategoryId equals lookupdetail.LookupCategoryId
                                       where (lookupdetail.Value.Equals("Male"))
                                       select new
                                       {
                                           lookupdetail.LookupDetailId
                                       };
                            model.GenderId = Convert.ToInt32(meds.FirstOrDefault().LookupDetailId);
                            //model.GenderId = 1;
                        }
                        var user1 = context.DBContext_register.Where(u => u.EmailAddress == model.EmailAddress).FirstOrDefault();
                        if (user1 == null)
                        {
                            var record = new RegistrationViewModel()
                            {
                                RoleId = model.RoleId,
                                UserName = model.UserName,
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                EmailAddress = model.EmailAddress,
                                Password = model.Password,
                                GenderId = model.GenderId,
                                DateOfBirth = model.DateOfBirth,
                                CellPhone = model.CellPhone,
                                ActivationCode = model.ActivationCode,
                                WhoAreYou = model.WhoAreYou,
                                CanLogin = 0,
                                IsActive = model.IsActive,
                                IsDeleted = model.IsDeleted,
                                CreatedBy = 0,
                                CreatedOn = DateTime.Now,
                                LastModifiedBy = 0,
                                LastModifiedOn = DateTime.Now,
                            };
                            context.DBContext_register.Add(model);
                            context.SaveChanges();
                                     
                          
                            string url = this.Url.Action("VerifyCode", "AccountManagment", new { ActivationCode = activationCode }, this.Request.Url.Scheme);
                            string templatecode = "Register";
                            EmailTemplateViewModel email_query = context.DBContext_emailtemplate.Where(u => u.templateCode == templatecode).FirstOrDefault();
                            string tempContent = email_query.templateContent;                                                           
                            if (tempContent != null)
                            {
                                if (tempContent.Contains("%UserName%"))
                                {
                                    tempContent = tempContent.Replace("%UserName%", model.UserName);
                                }
                                if (tempContent.Contains("%FirstName%"))
                                {
                                    tempContent = tempContent.Replace("%FirstName%", model.FirstName);
                                }
                                if (tempContent.Contains("%LastName%"))
                                {
                                    tempContent = tempContent.Replace("%LastName%", model.LastName);
                                }
                                if (tempContent.Contains("%EmailAddress%"))
                                {
                                    tempContent = tempContent.Replace("%EmailAddress%", model.EmailAddress);
                                }
                                if (tempContent.Contains("%Password%"))
                                {
                                    tempContent = tempContent.Replace("%Password%", model.Password);
                                }
                                if (tempContent.Contains("%url%"))
                                {
                                    tempContent = tempContent.Replace("%url%", url);
                                }
                            }
                            
                            string path = "C:/Users/developer1/Desktop/textfile.txt";
                            bool IsEmailSend = commonFunction.SendActivationEmail(model.EmailAddress, email_query.Subject, tempContent, email_query.Bcc, email_query.Cc,path,templatecode);                          
                            commonFunction.EmailLogInsert(model.EmailAddress,IsEmailSend,templatecode);


                            ViewBag.message = "successfully";
                            TempData["Data"] = "Thank you for registration.An email has been sent out with more detail to you.See you soon.";
                            return View("../Shared/Error");
                        }
                        else
                        {
                            ModelState.AddModelError("", "This Email is already registered");
                            TempData["Data"] = "This Email is already registered";
                            return View("../Shared/Error");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Username Already registered");
                        TempData["Data"] = "Username Already registered";
                        return View("../Shared/Error");
                    }
                }
            }
            ModelState.Clear();
            return View(model);
        }

        public ActionResult DoctorRegistration()
        {
            return View();
        }
        // POST: /Account/Register
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult DoctorRegistration(RegistrationViewModel model)
        {
            string pwd = "";
            var roleId = 1;
            var isactive = true;
            model.RoleId = Convert.ToInt32(roleId);
            string activationCode = "";
            CommonFunction commonFunction = new CommonFunction();
            activationCode = commonFunction.MakePassword(10);
            model.ActivationCode = activationCode;
            int a = model.GenderId;
            var whoAreYou = "Doctor";
            model.WhoAreYou = whoAreYou;
            if (ModelState.IsValid)
            {
                using (LocatorContext context = new LocatorContext())
                {
                    var user = context.DBContext_register.Where(u => u.UserName == model.UserName).FirstOrDefault();
                    pwd = model.Password;
                    pwd = commonFunction.Encrypt(pwd);
                    model.Password = pwd;
                    model.IsActive = isactive;
                    var tempDob = Request["kendoDOB"];
                    DateTime dateOfBirth = Convert.ToDateTime(tempDob);
                    model.DateOfBirth = dateOfBirth;
                    if (user == null)
                    {
                        if (model.GenderId == 1)
                        {
                            var meds = from lookupcategory in context.DBContext_lookupcategory
                                       join lookupdetail in context.DBContext_lookupdetail
                                       on lookupcategory.LookupCategoryId equals lookupdetail.LookupCategoryId
                                       where (lookupdetail.Value.Equals("Female"))
                                       select new
                                       {
                                           lookupdetail.LookupDetailId
                                       };
                            model.GenderId = Convert.ToInt32(meds.FirstOrDefault().LookupDetailId);
                            //model.GenderId = 1;
                        }
                        else
                        {
                            var meds = from lookupcategory in context.DBContext_lookupcategory
                                       join lookupdetail in context.DBContext_lookupdetail
                                       on lookupcategory.LookupCategoryId equals lookupdetail.LookupCategoryId
                                       where (lookupdetail.Value.Equals("Male"))
                                       select new
                                       {
                                           lookupdetail.LookupDetailId
                                       };
                            model.GenderId = Convert.ToInt32(meds.FirstOrDefault().LookupDetailId);
                            //model.GenderId = 1;
                        }
                        var user1 = context.DBContext_register.Where(u => u.EmailAddress == model.EmailAddress).FirstOrDefault();
                        if (user1 == null)
                        {
                            var record = new RegistrationViewModel()
                            {
                                RoleId = model.RoleId,
                                UserName = model.UserName,
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                EmailAddress = model.EmailAddress,
                                Password = model.Password,
                                GenderId = model.GenderId,
                                DateOfBirth = model.DateOfBirth,
                                CellPhone = model.CellPhone,
                                IMAI=model.IMAI,
                                Ref1_ContactNo=model.Ref1_ContactNo,
                                Ref1_Email=model.Ref1_Email,
                                Ref1_Name=model.Ref1_Name,
                                Ref2_Name=model.Ref2_Name,
                                Ref2_Email=model.Ref2_Email,
                                Ref2_ContactNo=model.Ref2_ContactNo,
                                ActivationCode = model.ActivationCode,
                                WhoAreYou=model.WhoAreYou,
                                CanLogin = 0,
                                IsActive = model.IsActive,
                                IsDeleted = model.IsDeleted,
                                CreatedBy = 0,
                                CreatedOn = DateTime.Now,
                                LastModifiedBy = 0,
                                LastModifiedOn = DateTime.Now,
                            };
                            context.DBContext_register.Add(model);
                            context.SaveChanges();


                            string url = this.Url.Action("VerifyCode", "AccountManagment", new { ActivationCode = activationCode }, this.Request.Url.Scheme);
                            string templatecode = "Register";
                            EmailTemplateViewModel email_query = context.DBContext_emailtemplate.Where(u => u.templateCode == templatecode).FirstOrDefault();
                            string tempContent = email_query.templateContent;
                            if (tempContent != null)
                            {
                                if (tempContent.Contains("%UserName%"))
                                {
                                    tempContent = tempContent.Replace("%UserName%", model.UserName);
                                }
                                if (tempContent.Contains("%FirstName%"))
                                {
                                    tempContent = tempContent.Replace("%FirstName%", model.FirstName);
                                }
                                if (tempContent.Contains("%LastName%"))
                                {
                                    tempContent = tempContent.Replace("%LastName%", model.LastName);
                                }
                                if (tempContent.Contains("%EmailAddress%"))
                                {
                                    tempContent = tempContent.Replace("%EmailAddress%", model.EmailAddress);
                                }
                                if (tempContent.Contains("%Password%"))
                                {
                                    tempContent = tempContent.Replace("%Password%", model.Password);
                                }
                                if (tempContent.Contains("%url%"))
                                {
                                    tempContent = tempContent.Replace("%url%", url);
                                }
                            }

                            string path = "C:/Users/developer1/Desktop/textfile.txt";
                            bool IsEmailSend = commonFunction.SendActivationEmail(model.EmailAddress, email_query.Subject, tempContent, email_query.Bcc, email_query.Cc, path, templatecode);
                            commonFunction.EmailLogInsert(model.EmailAddress, IsEmailSend, templatecode);


                            ViewBag.message = "successfully";
                            TempData["Data"] = "Thank you for registration.An email has been sent out with more detail to you.See you soon.";
                            return View("../Shared/Error");
                        }
                        else
                        {
                            ModelState.AddModelError("", "This Email is already registered");
                            TempData["Data"] = "This Email is already registered";
                            return View("../Shared/Error");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Username Already registered");
                        TempData["Data"] = "Username Already registered";
                        return View("../Shared/Error");
                    }
                }
            }
            ModelState.Clear();
            return View(model);
        }

        public ActionResult VerifyCode(RegistrationViewModel register)
        {
            string AC = !string.IsNullOrEmpty(Request.QueryString["ActivationCode"]) ? Request.QueryString["ActivationCode"] : Guid.Empty.ToString();
            string FN = !string.IsNullOrEmpty(Request.QueryString["First_Name"]) ? Request.QueryString["First_Name"] : Guid.Empty.ToString();
            string value = null;
            if (register.ActivationCode == null)
            {
                TempData["Data"] = "Your link has been expired.";
                return View("../Shared/Error");
            }
            else
            {
                using (LocatorContext context = new LocatorContext())
                {
                    var user = context.DBContext_register.Where(u => u.ActivationCode == AC && u.CanLogin == 0).FirstOrDefault();
                    if (user == null)
                    {                      
                        return RedirectToAction("Login", "AccountManagment");
                    }
                    else
                    {
                        register = context.DBContext_register.Find(user.UserId);
                        register.CanLogin = 1;
                        context.Entry(register).State = EntityState.Modified;
                        register.ActivationCode = value;
                        context.SaveChanges();
                        ViewBag.message = "Please complete your profile.";
                        TempData["Data"] = "You have successfully activated your account.Please login and complete your profile";
                        return View("../Shared/Error");
                    }
                }
            }
        }
        public JsonResult InstitueName()
        {
            var northwind = new LocatorContext();
            return Json(northwind.DBContext_educationinstitute.Select(c => new { InstitueName = c.InstituteName, IsActive = c.IsActive }).Where(u => u.IsActive), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DisableInstitueName()
        {
            var northwind = new LocatorContext();
            return Json(northwind.DBContext_educationinstitute.Select(c => new { InstitueName = c.InstituteName, IsActive = c.IsActive }).Where(u => !u.IsActive), JsonRequestBehavior.AllowGet);

        }
        public JsonResult CountryDropDown()
        {
            var northwind = new LocatorContext();
            return Json(northwind.DBContext_country.Select(c => new { ID = c.CountryId, CountryName = c.CountryName, CreatedOn = c.CreatedOn, CreatedBy = c.CreatedBy, IsActive = c.IsActive, IsDeleted = c.IsDeleted }).Where(u => u.IsActive.Equals(true)), JsonRequestBehavior.AllowGet);

            //return Json(northwind.DBContext_country.Select(c => new { ID = c.CountryId, CountryName = c.CountryName, CreatedOn = c.CreatedOn, CreatedBy = c.CreatedBy, IsActive = c.IsActive, LastModifiedBy = c.LastModifiedBy, IsDeleted = c.IsDeleted , LastModifiedOn = c.LastModifiedOn }).Where(u => u.IsActive.Equals("True")), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]      
        public ActionResult Login(LoginViewModel model, string usr)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string password = " ";
                    CommonFunction commonfunction = new CommonFunction();
                    password = model.LoginPassword;
                    password = commonfunction.Encrypt(password);
                    LoginHistoryViewModel history = new LoginHistoryViewModel();
                    using (LocatorContext context = new LocatorContext())
                    {
                        var user = context.DBContext_register.Where(u => u.UserName == model.LoginUserName && u.Password == password).FirstOrDefault();
                        if (user != null)
                        {
                            RegistrationViewModel register = new RegistrationViewModel();
                            register = context.DBContext_register.Find(user.UserId);
                            //var Isactive = aud.DBContext_register.Where(u => u.IsUserActive == 1).FirstOrDefault();
                            if (register.CanLogin == 0)
                            {
                                TempData["Data"] = "Please Check Your mail to activate your registration.";
                                return View("../Shared/Error");
                            }
                            else
                            {
                                ViewBag.message1 = model.LoginUserName;
                                int userid;
                                userid = Convert.ToInt32(user.UserId);
                                history.UserId = userid;
                                var record = new LoginHistoryViewModel()
                                {
                                    UserId = history.UserId,
                                    LoginTime = history.LoginTime,
                                };
                                context.DBContext_loginhistory.Add(record);
                                context.SaveChanges();
                                TempData["Data"] = "Login Successful.";
                                SessionPersist.Username = user.UserName;
                                ViewBag.SessionUsername = user.UserName.ToString();
                                //RoleViewModel roleviewmodel = new RoleViewModel();
                                var role = context.DBContext_register.Where(u => u.RoleId == register.RoleId).FirstOrDefault();
                                if (user != null)
                                {
                                    Session["UserName"] = user.UserName.ToString();
                                    Session["UserID"] = user.UserId.ToString();
                                    Session["WhoAreYou"] = user.WhoAreYou.ToString();
                                    // System.Web.HttpContext.Current.Session["UserName"].ToString();
                                }                            
                                return RedirectToAction("Index", "QuestionAnswer");
                            }
                        }
                        else
                        {
                            TempData["Data"] = "Username or Password is incorrect.Please try again";
                            return View("../Shared/Error");
                        }
                    }
                    //ModelState.Clear();
                }
                return View(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View();

        }    
        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Abandon();
            Session["UserName"] = string.Empty;
            SessionPersist.Username = string.Empty;
            return RedirectToAction("Login", "AccountManagment");
        }     
        public ActionResult ForgotPassword()
        {
            return View();
        }
        // POST: /Account/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (LocatorContext context = new LocatorContext())
                {
                    RegistrationViewModel reg_query = context.DBContext_register.Where(u => u.UserName == model.ForgotPassword_userName && u.EmailAddress == model.ForgotPassword_email).FirstOrDefault();

                    var user = context.DBContext_register.Where(u => u.UserName == model.ForgotPassword_userName && u.EmailAddress == model.ForgotPassword_email).FirstOrDefault();
                    if (user == null)
                    {
                        ModelState.AddModelError("", "Username or Email is incorrect");
                        TempData["Data"] = "Username or Email is incorrect.Please try again";
                        return View("../Shared/Error");
                    }
                    else
                    {
                        string activationCode = "";
                        CommonFunction commonfunction = new CommonFunction();
                        activationCode = commonfunction.MakePassword(10);
                        RegistrationViewModel register = new RegistrationViewModel();
                        register = context.DBContext_register.Find(user.UserId);
                        register.ActivationCode = activationCode;
                        context.Entry(register).State = EntityState.Modified;
                        context.SaveChanges();

                        //string body = "Hello " + register.FirstName + ",";
                        //body += "<br /><br />To reset your password please click here";
                        //body += "<br /><a href = '" + this.Url.Action("ResetPassword", "AccountManagment", new { ActivationCode = activationCode }, this.Request.Url.Scheme) + "'>Click here to activate your account.</a>";
                        //body += "<br /><br />Thanks";
                       
                        string url = this.Url.Action("ResetPassword", "AccountManagment", new { ActivationCode = activationCode }, this.Request.Url.Scheme);
                        string templatecode = "ForgotPassword";                       
                        EmailTemplateViewModel email_query = context.DBContext_emailtemplate.Where(u => u.templateCode == templatecode).FirstOrDefault();
                        string tempContent = email_query.templateContent;
                        if (tempContent != null)
                        {
                            if (tempContent.Contains("%UserName%"))
                            {
                                tempContent = tempContent.Replace("%UserName%", reg_query.UserName);
                            }
                            if (tempContent.Contains("%FirstName%"))
                            {
                                tempContent = tempContent.Replace("%FirstName%", reg_query.FirstName);
                            }
                            if (tempContent.Contains("%LastName%"))
                            {
                                tempContent = tempContent.Replace("%LastName%", reg_query.LastName);
                            }
                            if (tempContent.Contains("%EmailAddress%"))
                            {
                                tempContent = tempContent.Replace("%EmailAddress%", reg_query.EmailAddress);
                            }
                            if (tempContent.Contains("%Password%"))
                            {
                                tempContent = tempContent.Replace("%Password%", reg_query.Password);
                            }
                            if (tempContent.Contains("%url%"))
                            {
                                tempContent = tempContent.Replace("%url%", url);
                            }
                        }
                        CommonFunction commonFunction = new CommonFunction();
                        string path = "C:/Users/developer1/Desktop/textfile.txt";
                        bool IsEmailSend = commonFunction.SendActivationEmail(reg_query.EmailAddress, email_query.Subject, tempContent, email_query.Bcc, email_query.Cc, path, templatecode);
                        commonFunction.EmailLogInsert(reg_query.EmailAddress, IsEmailSend, templatecode);
                        TempData["Data"] = "To reset password link is sended to your account.Please check your mail ID";
                        return View("../Shared/Error");
                    }
                }
            }
            ModelState.Clear();
            return View(new ForgotPasswordViewModel());
        }
        [CheckSessionOutAttribute]
        public ActionResult ResetPassword()
        {
            string activationCode = !string.IsNullOrEmpty(Request.QueryString["ActivationCode"]) ? Request.QueryString["ActivationCode"] : Guid.Empty.ToString();
            using (LocatorContext context = new LocatorContext())
            {
                var user = context.DBContext_register.Where(u => u.ActivationCode == activationCode).FirstOrDefault();
                if (user == null)
                {
                    TempData["Data"] = "Reset Error";
                    return View("../Shared/Error");
                }
                else
                {
                    return View();
                }
            }
        }
        // POST: /Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            string activationCode = !string.IsNullOrEmpty(Request.QueryString["ActivationCode"]) ? Request.QueryString["ActivationCode"] : Guid.Empty.ToString();
            // string username = !string.IsNullOrEmpty(Request.QueryString["User_Name"]) ? Request.QueryString["User_Name"] : Guid.Empty.ToString();
            string value = null;
            try
            {
                if (ModelState.IsValid)
                {
                    using (LocatorContext context = new LocatorContext())
                    {
                        var user = context.DBContext_register.Where(u => u.ActivationCode == activationCode).FirstOrDefault();
                        if (user == null)
                        {
                            TempData["Data"] = "Reset Error";
                            return View("../Shared/Error");
                        }
                        else
                        {
                            ViewBag.message = "Login  successful";
                            RegistrationViewModel register = new RegistrationViewModel();
                            register = context.DBContext_register.Find(user.UserId);
                            register.ActivationCode = activationCode;
                            register.Password = model.Reset_Password;
                            CommonFunction commonFunction = new CommonFunction();
                            register.Password = commonFunction.Encrypt(model.Reset_Password);
                            register.ActivationCode = value;
                            context.Entry(register).State = EntityState.Modified;
                            context.SaveChanges();
                            TempData["Data"] = "Reset Success";
                            return View("../Shared/Error");
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
            return View(model);
        }
        [CheckSessionOutAttribute]
        public ActionResult ForgotUsername()
        {
            return View();
        }

        // POST: /Account/ForgotUsername
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ForgotUsername(ForgotUsernameViewModel model)
        {
            LocatorContext context = new LocatorContext();
            RegistrationViewModel  query = context.DBContext_register.Where(u => u.EmailAddress == model.ForgotUsername_email).FirstOrDefault();

            RegistrationViewModel register = new RegistrationViewModel();
            string username = "";
            if (ModelState.IsValid)
            {
                using (LocatorContext aud = new LocatorContext())
                {
                    var user = aud.DBContext_register.Where(u => u.EmailAddress == model.ForgotUsername_email).FirstOrDefault();
                    if (user == null)
                    {
                        ModelState.AddModelError("", "incorrect");
                        TempData["Data"] = "Email is incorrect.Please try again";
                        return View("../Shared/Error");
                    }
                    else
                    {
                        username = user.UserName.ToString();

                        //string body = "Hello " + username + ",";
                        //body += "<br /><br />Your UserName is " + username + ".";
                        //body += "<br /><br />Thanks";


                        string templatecode = "ForgotUsername";
                        EmailTemplateViewModel email_query = context.DBContext_emailtemplate.Where(u => u.templateCode == templatecode).FirstOrDefault();
                        string tempContent = email_query.templateContent;
                        if (tempContent != null)
                        {
                            if (tempContent.Contains("%UserName%"))
                            {
                                tempContent = tempContent.Replace("%UserName%", query.UserName);
                            }
                            if (tempContent.Contains("%FirstName%"))
                            {
                                tempContent = tempContent.Replace("%FirstName%", query.FirstName);
                            }
                            if (tempContent.Contains("%LastName%"))
                            {
                                tempContent = tempContent.Replace("%LastName%", query.LastName);
                            }
                            if (tempContent.Contains("%EmailAddress%"))
                            {
                                tempContent = tempContent.Replace("%EmailAddress%", query.EmailAddress);
                            }
                            if (tempContent.Contains("%Password%"))
                            {
                                tempContent = tempContent.Replace("%Password%", query.Password);
                            }
                           
                        }
                        CommonFunction commonFunction = new CommonFunction();
                        string path = "C:/Users/developer1/Desktop/textfile.txt";
                        bool IsEmailSend = commonFunction.SendActivationEmail(query.EmailAddress, email_query.Subject, tempContent, email_query.Bcc, email_query.Cc, path, templatecode);
                        commonFunction.EmailLogInsert(query.EmailAddress, IsEmailSend, templatecode);
                        TempData["Data"] = "Please check your mail.UserName is send to yuor mail.";
                        return View("../Shared/Error");
                    }
                }
            }
            ModelState.Clear();
            return View(model);
        }
    }
}






