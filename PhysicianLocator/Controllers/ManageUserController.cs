using PhysicianLocator.DAL;
using PhysicianLocator.Models;
using System.Web.Mvc;
using System.Linq;
using System;
using System.IO;
using System.Web;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using GoogleMaps.LocationServices;
using System.Configuration;
using System.Data.SqlClient;

namespace PhysicianLocator.Controllers
{
    public class ManageUserController : Controller
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(ManageUserController));  //Declaring Log4Net
        LocatorContext context = new LocatorContext();
        // GET: ManageUser
        [CheckSessionOut]
        public ActionResult EditUserProfile(int id)
        {
            int userid = Convert.ToInt32(Session["UserID"]);
            string whoareyou = Convert.ToString(Session["WhoAreYou"]);
            AdminUserRegViewModel model = new AdminUserRegViewModel();
            model.AdminId = userid;
            //registration retrive***********************************************************
            DocumentViewModel query_document = (from document in context.DBContext_document
                                                where document.CreatedBy == userid && document.PrimaryObjectId == "tblUsers"
                                                select document).FirstOrDefault();
            RegistrationViewModel query_registration = (from register in context.DBContext_register where register.UserId == userid select register).SingleOrDefault();
            if (query_registration != null)
            {
                model.FirstName = query_registration.FirstName;
                model.LastName = query_registration.LastName;
                model.CellPhone = query_registration.CellPhone;
                model.DateOfBirth = query_registration.DateOfBirth;
            }
            if (query_document != null)
            {

                model.documentName = query_document.DocumentName;
                model.documentpath = query_document.DocumentName;
                ViewBag.contentpath = query_document.DocumentPath;
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex) { logger.Error(ex.ToString()); }
            }
            else
            {
                model.documentName = "vee";
                model.documentpath = "~/Images/2/vee.png";
                ViewBag.contentpath = model.documentpath;
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex) { logger.Error(ex.ToString()); }
            }
            //address*************************************************************************************
            AddressViewModel query_address1 = (from address in context.DBContext_address where address.PrimaryKeyId == userid && address.IsPrimary select address).SingleOrDefault();

            if (query_address1 != null)
            {
                CountryViewModels query_country = (from country in context.DBContext_country where country.CountryId == query_address1.CountryId select country).SingleOrDefault();
                model.AddressViewModel1.Street1 = query_address1.Street1;
                model.AddressViewModel1.Street2 = query_address1.Street2;
                model.AddressViewModel1.Street3 = query_address1.Street3;
                model.AddressViewModel1.City = query_address1.City;
                model.AddressViewModel1.State = query_address1.State;
                model.AddressViewModel1.Zipcode = query_address1.Zipcode;
                model.AddressViewModel1.CountryId = query_address1.CountryId;
                model.CountryName1 = query_country.CountryName;
            }
            AddressViewModel query_address2 = (from address in context.DBContext_address where address.PrimaryKeyId == userid && !address.IsPrimary select address).SingleOrDefault();

            if (query_address2 != null)
            {
                CountryViewModels query_country = (from country in context.DBContext_country where country.CountryId == query_address2.CountryId select country).SingleOrDefault();
                model.AddressViewModel2.Street1 = query_address2.Street1;
                model.AddressViewModel2.Street2 = query_address2.Street2;
                model.AddressViewModel2.Street3 = query_address2.Street3;
                model.AddressViewModel2.City = query_address2.City;
                model.AddressViewModel2.State = query_address2.State;
                model.AddressViewModel2.Zipcode = query_address2.Zipcode;
                model.CountryName2 = query_country.CountryName;
            }         
                return View(model);              
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult EditUserProfile(int id, AdminUserRegViewModel model, HttpPostedFileBase uploadFile)
        {
            model.AdminId = id;
            var tempDob = Request["kendoDOB"];
            DateTime dateOfBirth = Convert.ToDateTime(tempDob);
            model.DateOfBirth = dateOfBirth;
            int value = id;
            DocumentViewModel document_model = new DocumentViewModel();
            //Registration*****************************************************************************
            RegistrationViewModel query_registration = (from register in context.DBContext_register where register.UserId == value select register).SingleOrDefault();
            DocumentViewModel query_document = (from document in context.DBContext_document
                                                where document.CreatedBy == value && document.PrimaryObjectId == "tblUsers"
                                                select document).SingleOrDefault();
            if (query_registration != null)
            {

                query_registration.FirstName = model.FirstName;
                query_registration.LastName = model.LastName;
                query_registration.CellPhone = model.CellPhone;
                query_registration.DateOfBirth = model.DateOfBirth;
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex) { logger.Error(ex.ToString()); }

                if (Request.Files.Count > 0)
                {
                    if (uploadFile != null && uploadFile.ContentLength > 0)
                    {
                        var delete_document = context.DBContext_document.Where(u => u.PrimaryObjectId == "tblUsers" && u.CreatedBy == value);
                        if (query_document != null)
                        {
                            context.DBContext_document.RemoveRange(delete_document);
                            context.SaveChanges();
                        }
                        string _FileName = Path.GetFileName(uploadFile.FileName);

                        int lastUserId = context.DBContext_register.Max(item => item.UserId);
                        document_model.CreatedBy = id;
                        var path = "~/Images/" + id + "/Personal/" + _FileName;
                        {
                            document_model.DocumentName = path;
                            Directory.CreateDirectory(Server.MapPath("~/Images/" + id + "/Personal"));
                            var _path = Path.Combine(Server.MapPath("~/Images/" + id + "/Personal"), _FileName);
                            uploadFile.SaveAs(_path);
                            document_model.DocumentPath = path;
                            var table = "tblUsers";
                            document_model.PrimaryObjectId = table;
                            document_model.PrimaryKeyId = lastUserId;
                            ViewBag.contentpath = document_model.DocumentPath;
                            var document_record = new DocumentViewModel()
                            {
                                DocumentName = document_model.DocumentName,
                                DocumentPath = document_model.DocumentPath,
                                PrimaryKeyId = document_model.PrimaryKeyId,
                                IsActive = document_model.IsActive,
                                IsDeleted = false,
                                CreatedBy = document_model.CreatedBy,
                                CreatedOn = DateTime.Now,
                                LastModifiedBy = 0,
                                LastModifiedOn = DateTime.Now,
                            };
                            context.DBContext_document.Add(document_model);
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        if (query_document != null)
                        {
                            query_document.CreatedOn = document_model.CreatedOn;
                            ViewBag.contentpath = query_document.DocumentPath;
                            try
                            {
                                context.SaveChanges();
                            }
                            catch (Exception ex) { logger.Error(ex.ToString()); }
                        }
                        else
                        {
                            model.documentName = "vee";
                            model.documentpath = "~/Images/2/vee.png";
                            ViewBag.contentpath = model.documentpath;
                            try
                            {
                                context.SaveChanges();
                            }
                            catch (Exception ex) { logger.Error(ex.ToString()); }
                        }
                    }

                    ViewBag.Message = "File Uploaded Successfully!!";
                }

            }
            //AddressViewModel**********************************************************************************

            // Query the database for the rows to be deleted.
            var user = context.DBContext_address.Where(u => u.PrimaryKeyId == value).FirstOrDefault();
            var objectname = "tblUsers";
            if (user != null)
            {
                var deleteOrderDetails = context.DBContext_address.Where(a => a.PrimaryKeyId == value);
                context.DBContext_address.RemoveRange(deleteOrderDetails);
                context.SaveChanges();

                string markers1 = model.AddressViewModel1.Street1;               
                markers1 += string.Format(" {0},", model.AddressViewModel1.Street2);
                markers1 += string.Format(" {0},", model.AddressViewModel1.Street3);
                markers1 += string.Format(" {0}", model.AddressViewModel1.Zipcode);
                model.AddressViewModel1.PrimaryKeyId = query_registration.UserId;
                var locationService1 = new GoogleLocationService();
                var point1 = locationService1.GetLatLongFromAddress(markers1);
                var latitude1 = point1.Latitude;
                var longitude1 = point1.Longitude;
                model.AddressViewModel1.latitute = Convert.ToString(latitude1);
                model.AddressViewModel1.longitute = Convert.ToString(longitude1);

                

                var tempCountry = Request["kendoCountry"];
                model.AddressViewModel1.CountryId = Convert.ToInt32(tempCountry);
                model.CountryName1 = tempCountry;
                var IsprimaryAddressViewModel1 = true;
                model.AddressViewModel1.IsPrimary = IsprimaryAddressViewModel1;
                model.AddressViewModel1.PrimaryObjectId = objectname;
                var record_address1 = new AddressViewModel()
                {
                    PrimaryKeyId = model.AddressViewModel1.PrimaryKeyId,
                    PrimaryObjectId = model.AddressViewModel1.PrimaryObjectId,
                    Street1 = model.AddressViewModel1.Street1,
                    Street2 = model.AddressViewModel1.Street2,
                    Street3 = model.AddressViewModel1.Street3,
                    City = model.AddressViewModel1.City,
                    State = model.AddressViewModel1.State,
                    CountryId = model.AddressViewModel1.CountryId,
                    Zipcode = model.AddressViewModel1.Zipcode,
                    latitute=model.AddressViewModel1.latitute,
                    longitute=model.AddressViewModel1.longitute,
                    IsPrimary = model.AddressViewModel1.IsPrimary,
                    IsActive = model.AddressViewModel1.IsActive,
                    IsDeleted = false,
                    CreatedBy = 0,
                    CreatedOn = DateTime.Now,
                    LastModifiedBy = 0,
                    LastModifiedOn = DateTime.Now,
                };
                context.DBContext_address.Add(model.AddressViewModel1);

                string markers2 = model.AddressViewModel2.Street1;
                markers2 += string.Format(" {0},", model.AddressViewModel2.Street2);
                markers2 += string.Format(" {0},", model.AddressViewModel2.Street3);
                markers2 += string.Format(" {0}", model.AddressViewModel2.Zipcode);
                model.AddressViewModel2.PrimaryKeyId = query_registration.UserId;
                var locationService2 = new GoogleLocationService();
                var point2 = locationService2.GetLatLongFromAddress(markers2);
                var latitude2 = point2.Latitude;
                var longitude2 = point2.Longitude;
                model.AddressViewModel2.latitute = Convert.ToString(latitude2);
                model.AddressViewModel2.longitute = Convert.ToString(longitude2);

                model.AddressViewModel2.PrimaryKeyId = query_registration.UserId;
                var tempCountry2 = Request["kendoCountry2"];
                model.AddressViewModel2.CountryId = Convert.ToInt32(tempCountry2);
                var IsprimaryAddressViewModel2 = false;
                model.AddressViewModel2.IsPrimary = IsprimaryAddressViewModel2;
                model.AddressViewModel2.PrimaryObjectId = objectname;
                var record_address2 = new AddressViewModel()
                {
                    PrimaryKeyId = model.AddressViewModel2.PrimaryKeyId,
                    PrimaryObjectId = model.AddressViewModel2.PrimaryObjectId,
                    Street1 = model.AddressViewModel2.Street1,
                    Street2 = model.AddressViewModel2.Street2,
                    Street3 = model.AddressViewModel2.Street3,
                    City = model.AddressViewModel2.City,
                    State = model.AddressViewModel2.State,
                    CountryId = model.AddressViewModel2.CountryId,
                    Zipcode = model.AddressViewModel2.Zipcode,
                    latitute = model.AddressViewModel2.latitute,
                    longitute = model.AddressViewModel2.longitute,
                    IsPrimary = model.AddressViewModel2.IsPrimary,
                    IsActive = model.AddressViewModel2.IsActive,
                    IsDeleted = false,
                    CreatedBy = 0,
                    CreatedOn = DateTime.Now,
                    LastModifiedBy = 0,
                    LastModifiedOn = DateTime.Now,
                };
                context.DBContext_address.Add(model.AddressViewModel2);
                context.SaveChanges();
            }
            else
            {
                if (model.AddressViewModel1.Street1 != null && model.AddressViewModel1.Street2 != null && model.AddressViewModel1.Street3 != null && model.AddressViewModel1.City != null && model.AddressViewModel1.State != null)
                {

                    string markers1 = model.AddressViewModel1.Street1;
                    markers1 += string.Format(" {0},", model.AddressViewModel1.Street2);
                    markers1 += string.Format(" {0},", model.AddressViewModel1.Street3);
                    markers1 += string.Format(" {0}", model.AddressViewModel1.Zipcode);
                    model.AddressViewModel1.PrimaryKeyId = query_registration.UserId;
                    var locationService1 = new GoogleLocationService();
                    var point1 = locationService1.GetLatLongFromAddress(markers1);
                    var latitude1 = point1.Latitude;
                    var longitude1 = point1.Longitude;
                    model.AddressViewModel1.latitute = Convert.ToString(latitude1);
                    model.AddressViewModel1.longitute = Convert.ToString(longitude1);

                    model.AddressViewModel1.PrimaryKeyId = query_registration.UserId;
                    var tempCountry = Request["kendoCountry"];
                    model.CountryName2 = tempCountry;
                    model.AddressViewModel1.CountryId = Convert.ToInt32(tempCountry);
                    var IsprimaryAddressViewModel1 = true;
                  
                    model.AddressViewModel1.PrimaryObjectId = objectname;
                    model.AddressViewModel1.IsPrimary = IsprimaryAddressViewModel1;
                    var record_address1 = new AddressViewModel()
                    {
                        PrimaryKeyId = model.AddressViewModel1.PrimaryKeyId,
                        PrimaryObjectId=model.AddressViewModel1.PrimaryObjectId,
                        Street1 = model.AddressViewModel1.Street1,
                        Street2 = model.AddressViewModel1.Street2,
                        Street3 = model.AddressViewModel1.Street3,
                        City = model.AddressViewModel1.City,
                        State = model.AddressViewModel1.State,
                        CountryId = model.AddressViewModel1.CountryId,
                        Zipcode = model.AddressViewModel1.Zipcode,
                        IsPrimary = model.AddressViewModel1.IsPrimary,
                        IsActive = model.AddressViewModel1.IsActive,
                        latitute=model.AddressViewModel1.latitute,
                        longitute=model.AddressViewModel1.longitute,
                        IsDeleted = false,
                        CreatedBy = 0,
                        CreatedOn = DateTime.Now,
                        LastModifiedBy = 0,
                        LastModifiedOn = DateTime.Now,
                    };
                    context.DBContext_address.Add(model.AddressViewModel1);
               
                    model.AddressViewModel2.PrimaryKeyId = query_registration.UserId;
                    var tempCountry2 = Request["kendoCountry2"];
                    model.AddressViewModel2.CountryId = Convert.ToInt32(tempCountry);
                    var IsprimaryAddressViewModel2 = false;
                    model.AddressViewModel2.IsPrimary = IsprimaryAddressViewModel2;
                    string markers2 = model.AddressViewModel2.Street1;
                    markers2 += string.Format(" {0},", model.AddressViewModel2.Street2);
                    markers2 += string.Format(" {0},", model.AddressViewModel2.Street3);
                    markers2 += string.Format(" {0}", model.AddressViewModel2.Zipcode);
                    model.AddressViewModel1.PrimaryKeyId = query_registration.UserId;
                    var locationService2 = new GoogleLocationService();
                    var point2 = locationService2.GetLatLongFromAddress(markers2);
                    var latitude2 = point2.Latitude;
                    var longitude2 = point2.Longitude;
                    model.AddressViewModel2.latitute = Convert.ToString(latitude2);
                    model.AddressViewModel2.longitute = Convert.ToString(longitude2);
                    var objectname2 = "tblUsers";
                    model.AddressViewModel2.PrimaryObjectId = objectname2;
                    var record_address2 = new AddressViewModel()
                    {
                        PrimaryKeyId = model.AddressViewModel2.PrimaryKeyId,
                        PrimaryObjectId = model.AddressViewModel2.PrimaryObjectId,
                        Street1 = model.AddressViewModel2.Street1,
                        Street2 = model.AddressViewModel2.Street2,
                        Street3 = model.AddressViewModel2.Street3,
                        City = model.AddressViewModel2.City,
                        State = model.AddressViewModel2.State,
                        CountryId = model.AddressViewModel2.CountryId,
                        Zipcode = model.AddressViewModel2.Zipcode,
                        latitute=model.AddressViewModel2.latitute,
                        longitute=model.AddressViewModel2.longitute,
                        IsPrimary = model.AddressViewModel2.IsPrimary,
                        IsActive = model.AddressViewModel2.IsActive,
                        IsDeleted = false,
                        CreatedBy = 0,
                        CreatedOn = DateTime.Now,
                        LastModifiedBy = 0,
                        LastModifiedOn = DateTime.Now,
                    };
                    context.DBContext_address.Add(model.AddressViewModel2);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("ViewUserProfile", "ManageUser", new { @id = id });
        }
        public ActionResult PatientEditUserProfile(int id)
        {
            int userid = Convert.ToInt32(Session["UserID"]);
            string whoareyou = Convert.ToString(Session["WhoAreYou"]);
            //int value = id; instead of this passing session value
            AdminUserRegViewModel model = new AdminUserRegViewModel();
            model.AdminId = userid;
            //registration retrive***********************************************************
            DocumentViewModel query_document = (from document in context.DBContext_document
                                                where document.CreatedBy == userid && document.PrimaryObjectId == "tblUsers"
                                                select document).FirstOrDefault();
            RegistrationViewModel query_registration = (from register in context.DBContext_register where register.UserId == userid select register).SingleOrDefault();
            if (query_registration != null)
            {
                model.FirstName = query_registration.FirstName;
                model.LastName = query_registration.LastName;
                model.CellPhone = query_registration.CellPhone;
                model.DateOfBirth = query_registration.DateOfBirth;
            }
            if (query_document != null)
            {
                model.documentName = query_document.DocumentName;
                model.documentpath = query_document.DocumentName;
                ViewBag.contentpath = query_document.DocumentPath;
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex) { logger.Error(ex.ToString()); }
            }
            else
            {
                model.documentName = "vee";
                model.documentpath = "~/Images/2/vee.png";
                ViewBag.contentpath = model.documentpath;
                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex) { logger.Error(ex.ToString()); }
            }
            //address*************************************************************************************
            AddressViewModel query_address1 = (from address in context.DBContext_address where address.PrimaryKeyId == userid && address.IsPrimary select address).SingleOrDefault();

            if (query_address1 != null)
            {
                CountryViewModels query_country = (from country in context.DBContext_country where country.CountryId == query_address1.CountryId select country).SingleOrDefault();

                model.AddressViewModel1.Street1 = query_address1.Street1;
                model.AddressViewModel1.Street2 = query_address1.Street2;
                model.AddressViewModel1.Street3 = query_address1.Street3;
                model.AddressViewModel1.City = query_address1.City;
                model.AddressViewModel1.State = query_address1.State;
                model.AddressViewModel1.Zipcode = query_address1.Zipcode;
                model.AddressViewModel1.CountryId = query_address1.CountryId;
                model.CountryName1 = query_country.CountryName;
            }
            AddressViewModel query_address2 = (from address in context.DBContext_address where address.PrimaryKeyId == userid && !address.IsPrimary select address).SingleOrDefault();

            if (query_address2 != null)
            {
                CountryViewModels query_country = (from country in context.DBContext_country where country.CountryId == query_address2.CountryId select country).SingleOrDefault();
                model.AddressViewModel2.Street1 = query_address2.Street1;
                model.AddressViewModel2.Street2 = query_address2.Street2;
                model.AddressViewModel2.Street3 = query_address2.Street3;
                model.AddressViewModel2.City = query_address2.City;
                model.AddressViewModel2.State = query_address2.State;
                model.AddressViewModel2.Zipcode = query_address2.Zipcode;
                model.CountryName2 = query_country.CountryName;
            }
            return View(model);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult PatientEditUserProfile(int id, AdminUserRegViewModel model, HttpPostedFileBase uploadFile)
        {

            model.AdminId = id;

            var tempDob = Request["kendoDOB"];
            DateTime dateOfBirth = Convert.ToDateTime(tempDob);
            model.DateOfBirth = dateOfBirth;
            int value = id;
            DocumentViewModel document_model = new DocumentViewModel();

            //Registration*****************************************************************************
            RegistrationViewModel query_registration = (from register in context.DBContext_register where register.UserId == value select register).SingleOrDefault();
            DocumentViewModel query_document = (from document in context.DBContext_document
                                                where document.CreatedBy == value && document.PrimaryObjectId == "tblUsers"
                                                select document).SingleOrDefault();

            if (query_registration != null)
            {

                query_registration.FirstName = model.FirstName;
                query_registration.LastName = model.LastName;
                query_registration.CellPhone = model.CellPhone;
                query_registration.DateOfBirth = model.DateOfBirth;

                try
                {
                    context.SaveChanges();
                }
                catch (Exception ex) { logger.Error(ex.ToString()); }

                if (Request.Files.Count > 0)
                {


                    if (uploadFile != null && uploadFile.ContentLength > 0)
                    {
                        var delete_document = context.DBContext_document.Where(u => u.PrimaryObjectId == "tblUsers" && u.CreatedBy == value);
                        if (query_document != null)
                        {
                            context.DBContext_document.RemoveRange(delete_document);
                            context.SaveChanges();
                        }
                        string _FileName = Path.GetFileName(uploadFile.FileName);

                        int lastUserId = context.DBContext_register.Max(item => item.UserId);
                        document_model.CreatedBy = id;
                        var path = "~/Images/" + id + "/Personal/" + _FileName;
                        {
                            document_model.DocumentName = path;
                            Directory.CreateDirectory(Server.MapPath("~/Images/" + id + "/Personal"));
                            var _path = Path.Combine(Server.MapPath("~/Images/" + id + "/Personal"), _FileName);
                            uploadFile.SaveAs(_path);
                            document_model.DocumentPath = path;
                            var table = "tblUsers";
                            document_model.PrimaryObjectId = table;
                            document_model.PrimaryKeyId = lastUserId;
                            ViewBag.contentpath = document_model.DocumentPath;
                            var document_record = new DocumentViewModel()
                            {
                                DocumentName = document_model.DocumentName,
                                DocumentPath = document_model.DocumentPath,
                                PrimaryKeyId = document_model.PrimaryKeyId,
                                IsActive = document_model.IsActive,
                                IsDeleted = false,
                                CreatedBy = document_model.CreatedBy,
                                CreatedOn = DateTime.Now,
                                LastModifiedBy = 0,
                                LastModifiedOn = DateTime.Now,
                            };
                            context.DBContext_document.Add(document_model);
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        if (query_document != null)
                        {

                            query_document.CreatedOn = document_model.CreatedOn;
                            ViewBag.contentpath = query_document.DocumentPath;

                            try
                            {
                                context.SaveChanges();
                            }
                            catch (Exception ex) { logger.Error(ex.ToString()); }
                        }
                        else
                        {
                            model.documentName = "vee";
                            model.documentpath = "~/Images/2/vee.png";
                            ViewBag.contentpath = model.documentpath;
                            try
                            {
                                context.SaveChanges();
                            }
                            catch (Exception ex) { logger.Error(ex.ToString()); }
                        }
                    }

                    ViewBag.Message = "File Uploaded Successfully!!";
                }

            }
            //AddressViewModel**********************************************************************************

            // Query the database for the rows to be deleted.
            var user = context.DBContext_address.Where(u => u.PrimaryKeyId == value).FirstOrDefault();
            var objectname = "tblUsers";
            if (user != null)
            {
                var deleteOrderDetails = context.DBContext_address.Where(a => a.PrimaryKeyId == value);
                context.DBContext_address.RemoveRange(deleteOrderDetails);
                context.SaveChanges();

                string markers1 = model.AddressViewModel1.Street1;
                markers1 += string.Format(" {0},", model.AddressViewModel1.Street2);
                markers1 += string.Format(" {0},", model.AddressViewModel1.Street3);
                markers1 += string.Format(" {0}", model.AddressViewModel1.Zipcode);
                model.AddressViewModel1.PrimaryKeyId = query_registration.UserId;
                var locationService1 = new GoogleLocationService();
                var point1 = locationService1.GetLatLongFromAddress(markers1);
                var latitude1 = point1.Latitude;
                var longitude1 = point1.Longitude;
                model.AddressViewModel1.latitute = Convert.ToString(latitude1);
                model.AddressViewModel1.longitute = Convert.ToString(longitude1);

                var tempCountry = Request["kendoCountry"];
                model.AddressViewModel1.CountryId = Convert.ToInt32(tempCountry);
                model.CountryName1 = tempCountry;
                var IsprimaryAddressViewModel1 = true;
                model.AddressViewModel1.IsPrimary = IsprimaryAddressViewModel1;
                model.AddressViewModel1.PrimaryObjectId = objectname;
                var record_address1 = new AddressViewModel()
                {
                    PrimaryKeyId = model.AddressViewModel1.PrimaryKeyId,
                    PrimaryObjectId = model.AddressViewModel1.PrimaryObjectId,
                    Street1 = model.AddressViewModel1.Street1,
                    Street2 = model.AddressViewModel1.Street2,
                    Street3 = model.AddressViewModel1.Street3,
                    City = model.AddressViewModel1.City,
                    State = model.AddressViewModel1.State,
                    CountryId = model.AddressViewModel1.CountryId,
                    Zipcode = model.AddressViewModel1.Zipcode,
                    latitute = model.AddressViewModel1.latitute,
                    longitute = model.AddressViewModel1.longitute,
                    IsPrimary = model.AddressViewModel1.IsPrimary,
                    IsActive = model.AddressViewModel1.IsActive,
                    IsDeleted = false,
                    CreatedBy = 0,
                    CreatedOn = DateTime.Now,
                    LastModifiedBy = 0,
                    LastModifiedOn = DateTime.Now,
                };
                context.DBContext_address.Add(model.AddressViewModel1);

                string markers2 = model.AddressViewModel2.Street1;
                markers2 += string.Format(" {0},", model.AddressViewModel2.Street2);
                markers2 += string.Format(" {0},", model.AddressViewModel2.Street3);
                markers2 += string.Format(" {0}", model.AddressViewModel2.Zipcode);
                model.AddressViewModel2.PrimaryKeyId = query_registration.UserId;
                var locationService2 = new GoogleLocationService();
                var point2 = locationService2.GetLatLongFromAddress(markers2);
                var latitude2 = point2.Latitude;
                var longitude2 = point2.Longitude;
                model.AddressViewModel2.latitute = Convert.ToString(latitude2);
                model.AddressViewModel2.longitute = Convert.ToString(longitude2);

                model.AddressViewModel2.PrimaryKeyId = query_registration.UserId;
                var tempCountry2 = Request["kendoCountry2"];
                model.AddressViewModel2.CountryId = Convert.ToInt32(tempCountry2);
                var IsprimaryAddressViewModel2 = false;
                model.AddressViewModel2.IsPrimary = IsprimaryAddressViewModel2;
                model.AddressViewModel2.PrimaryObjectId = objectname;
                var record_address2 = new AddressViewModel()
                {
                    PrimaryKeyId = model.AddressViewModel2.PrimaryKeyId,
                    PrimaryObjectId = model.AddressViewModel2.PrimaryObjectId,
                    Street1 = model.AddressViewModel2.Street1,
                    Street2 = model.AddressViewModel2.Street2,
                    Street3 = model.AddressViewModel2.Street3,
                    City = model.AddressViewModel2.City,
                    State = model.AddressViewModel2.State,
                    CountryId = model.AddressViewModel2.CountryId,
                    Zipcode = model.AddressViewModel2.Zipcode,
                    latitute = model.AddressViewModel2.latitute,
                    longitute = model.AddressViewModel2.longitute,
                    IsPrimary = model.AddressViewModel2.IsPrimary,
                    IsActive = model.AddressViewModel2.IsActive,
                    IsDeleted = false,
                    CreatedBy = 0,
                    CreatedOn = DateTime.Now,
                    LastModifiedBy = 0,
                    LastModifiedOn = DateTime.Now,
                };
                context.DBContext_address.Add(model.AddressViewModel2);
                context.SaveChanges();
            }
            else
            {
                if (model.AddressViewModel1.Street1 != null && model.AddressViewModel1.Street2 != null && model.AddressViewModel1.Street3 != null && model.AddressViewModel1.City != null && model.AddressViewModel1.State != null)
                {

                    string markers1 = model.AddressViewModel1.Street1;
                    markers1 += string.Format(" {0},", model.AddressViewModel1.Street2);
                    markers1 += string.Format(" {0},", model.AddressViewModel1.Street3);
                    markers1 += string.Format(" {0}", model.AddressViewModel1.Zipcode);
                    model.AddressViewModel1.PrimaryKeyId = query_registration.UserId;
                    var locationService1 = new GoogleLocationService();
                    var point1 = locationService1.GetLatLongFromAddress(markers1);
                    var latitude1 = point1.Latitude;
                    var longitude1 = point1.Longitude;
                    model.AddressViewModel1.latitute = Convert.ToString(latitude1);
                    model.AddressViewModel1.longitute = Convert.ToString(longitude1);

                    model.AddressViewModel1.PrimaryKeyId = query_registration.UserId;
                    var tempCountry = Request["kendoCountry"];
                    model.CountryName2 = tempCountry;
                    model.AddressViewModel1.CountryId = Convert.ToInt32(tempCountry);
                    var IsprimaryAddressViewModel1 = true;

                    model.AddressViewModel1.PrimaryObjectId = objectname;
                    model.AddressViewModel1.IsPrimary = IsprimaryAddressViewModel1;
                    var record_address1 = new AddressViewModel()
                    {
                        PrimaryKeyId = model.AddressViewModel1.PrimaryKeyId,
                        PrimaryObjectId = model.AddressViewModel1.PrimaryObjectId,
                        Street1 = model.AddressViewModel1.Street1,
                        Street2 = model.AddressViewModel1.Street2,
                        Street3 = model.AddressViewModel1.Street3,
                        City = model.AddressViewModel1.City,
                        State = model.AddressViewModel1.State,
                        CountryId = model.AddressViewModel1.CountryId,
                        Zipcode = model.AddressViewModel1.Zipcode,
                        IsPrimary = model.AddressViewModel1.IsPrimary,
                        IsActive = model.AddressViewModel1.IsActive,
                        latitute = model.AddressViewModel1.latitute,
                        longitute = model.AddressViewModel1.longitute,
                        IsDeleted = false,
                        CreatedBy = 0,
                        CreatedOn = DateTime.Now,
                        LastModifiedBy = 0,
                        LastModifiedOn = DateTime.Now,
                    };
                    context.DBContext_address.Add(model.AddressViewModel1);

                    model.AddressViewModel2.PrimaryKeyId = query_registration.UserId;
                    var tempCountry2 = Request["kendoCountry2"];
                    model.AddressViewModel2.CountryId = Convert.ToInt32(tempCountry);
                    var IsprimaryAddressViewModel2 = false;
                    model.AddressViewModel2.IsPrimary = IsprimaryAddressViewModel2;
                    string markers2 = model.AddressViewModel2.Street1;
                    markers2 += string.Format(" {0},", model.AddressViewModel2.Street2);
                    markers2 += string.Format(" {0},", model.AddressViewModel2.Street3);
                    markers2 += string.Format(" {0}", model.AddressViewModel2.Zipcode);
                    model.AddressViewModel1.PrimaryKeyId = query_registration.UserId;
                    var locationService2 = new GoogleLocationService();
                    var point2 = locationService2.GetLatLongFromAddress(markers2);
                    var latitude2 = point2.Latitude;
                    var longitude2 = point2.Longitude;
                    model.AddressViewModel2.latitute = Convert.ToString(latitude2);
                    model.AddressViewModel2.longitute = Convert.ToString(longitude2);
                    var objectname2 = "tblUsers";
                    model.AddressViewModel2.PrimaryObjectId = objectname2;
                    var record_address2 = new AddressViewModel()
                    {
                        PrimaryKeyId = model.AddressViewModel2.PrimaryKeyId,
                        PrimaryObjectId = model.AddressViewModel2.PrimaryObjectId,
                        Street1 = model.AddressViewModel2.Street1,
                        Street2 = model.AddressViewModel2.Street2,
                        Street3 = model.AddressViewModel2.Street3,
                        City = model.AddressViewModel2.City,
                        State = model.AddressViewModel2.State,
                        CountryId = model.AddressViewModel2.CountryId,
                        Zipcode = model.AddressViewModel2.Zipcode,
                        latitute = model.AddressViewModel2.latitute,
                        longitute = model.AddressViewModel2.longitute,
                        IsPrimary = model.AddressViewModel2.IsPrimary,
                        IsActive = model.AddressViewModel2.IsActive,
                        IsDeleted = false,
                        CreatedBy = 0,
                        CreatedOn = DateTime.Now,
                        LastModifiedBy = 0,
                        LastModifiedOn = DateTime.Now,
                    };
                    context.DBContext_address.Add(model.AddressViewModel2);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("PatientViewUserProfile", "ManageUser", new { @id = id });
        }
        [CheckSessionOut]
        public ActionResult ViewUserProfile(int id)
        {
            AdminUserRegViewModel model = new AdminUserRegViewModel();
            int value = id;
            Session["UserID"] = id.ToString();
            string whoareyou = Convert.ToString(Session["WhoAreYou"]);
           
              
                //registration retrive***********************************************************
                RegistrationViewModel query_registration = (from register in context.DBContext_register where register.UserId == value select register).SingleOrDefault();
                if (query_registration != null)
                {
                    model.FirstName = query_registration.FirstName;
                    model.LastName = query_registration.LastName;
                    model.CellPhone = query_registration.CellPhone;
                    model.DateOfBirth = query_registration.DateOfBirth;
                }
                DocumentViewModel query_document = (from document in context.DBContext_document
                                                    where document.CreatedBy == id && document.PrimaryObjectId == "tblUsers"
                                                    select document).FirstOrDefault();
                if (query_document != null)
                {
                    model.documentName = query_document.DocumentName;
                    model.documentpath = query_document.DocumentName;
                    ViewBag.contentpath = query_document.DocumentPath;
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex) { logger.Error(ex.ToString()); }
                }
                else
                {
                    model.documentName = "vee";
                    model.documentpath = "~/Images/2/vee.png";
                    ViewBag.contentpath = model.documentpath;
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex) { logger.Error(ex.ToString()); }
                }
                //address*************************************************************************************
                AddressViewModel query_address1 = (from address in context.DBContext_address where address.PrimaryKeyId == value && address.IsPrimary select address).SingleOrDefault();

                if (query_address1 != null)
                {
                    CountryViewModels query_country = (from country in context.DBContext_country where country.CountryId == query_address1.CountryId select country).SingleOrDefault();
                    model.AddressViewModel1.Street1 = query_address1.Street1;
                    model.AddressViewModel1.Street2 = query_address1.Street2;
                    model.AddressViewModel1.Street3 = query_address1.Street3;
                    model.AddressViewModel1.City = query_address1.City;
                    model.AddressViewModel1.State = query_address1.State;
                    model.AddressViewModel1.Zipcode = query_address1.Zipcode;
                    model.CountryName1 = query_country.CountryName;
                }
                AddressViewModel query_address2 = (from address in context.DBContext_address where address.PrimaryKeyId == value && !address.IsPrimary select address).SingleOrDefault();

                if (query_address2 != null)
                {
                    CountryViewModels query_country = (from country in context.DBContext_country where country.CountryId == query_address2.CountryId select country).SingleOrDefault();
                    model.AddressViewModel2.Street1 = query_address2.Street1;
                    model.AddressViewModel2.Street2 = query_address2.Street2;
                    model.AddressViewModel2.Street3 = query_address2.Street3;
                    model.AddressViewModel2.City = query_address2.City;
                    model.AddressViewModel2.State = query_address2.State;
                    model.AddressViewModel2.Zipcode = query_address2.Zipcode;
                    model.CountryName2 = query_country.CountryName;
                }
                ViewBag.message = id;            
            
            return View(model);

        }
        [CheckSessionOut]
        public ActionResult PatientViewUserProfile(int id)
        {
            AdminUserRegViewModel model = new AdminUserRegViewModel();
            int value = id;
            Session["UserID"] = id.ToString();
            string whoareyou = Convert.ToString(Session["WhoAreYou"]);
          

                //registration retrive***********************************************************
                RegistrationViewModel query_registration = (from register in context.DBContext_register where register.UserId == value select register).SingleOrDefault();
                if (query_registration != null)
                {
                    model.FirstName = query_registration.FirstName;
                    model.LastName = query_registration.LastName;
                    model.CellPhone = query_registration.CellPhone;
                    model.DateOfBirth = query_registration.DateOfBirth;
                }
                DocumentViewModel query_document = (from document in context.DBContext_document
                                                    where document.CreatedBy == id && document.PrimaryObjectId == "tblUsers"
                                                    select document).FirstOrDefault();
                if (query_document != null)
                {
                    model.documentName = query_document.DocumentName;
                    model.documentpath = query_document.DocumentName;
                    ViewBag.contentpath = query_document.DocumentPath;
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex) { logger.Error(ex.ToString()); }
                }
                else
                {
                    model.documentName = "vee";
                    model.documentpath = "~/Images/2/vee.png";
                    ViewBag.contentpath = model.documentpath;
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex) { logger.Error(ex.ToString()); }
                }
                //address*************************************************************************************
                AddressViewModel query_address1 = (from address in context.DBContext_address where address.PrimaryKeyId == value && address.IsPrimary select address).SingleOrDefault();

                if (query_address1 != null)
                {
                    CountryViewModels query_country = (from country in context.DBContext_country where country.CountryId == query_address1.CountryId select country).SingleOrDefault();
                    model.AddressViewModel1.Street1 = query_address1.Street1;
                    model.AddressViewModel1.Street2 = query_address1.Street2;
                    model.AddressViewModel1.Street3 = query_address1.Street3;
                    model.AddressViewModel1.City = query_address1.City;
                    model.AddressViewModel1.State = query_address1.State;
                    model.AddressViewModel1.Zipcode = query_address1.Zipcode;
                    model.CountryName1 = query_country.CountryName;
                }
                AddressViewModel query_address2 = (from address in context.DBContext_address where address.PrimaryKeyId == value && !address.IsPrimary select address).SingleOrDefault();

                if (query_address2 != null)
                {
                    CountryViewModels query_country = (from country in context.DBContext_country where country.CountryId == query_address2.CountryId select country).SingleOrDefault();
                    model.AddressViewModel2.Street1 = query_address2.Street1;
                    model.AddressViewModel2.Street2 = query_address2.Street2;
                    model.AddressViewModel2.Street3 = query_address2.Street3;
                    model.AddressViewModel2.City = query_address2.City;
                    model.AddressViewModel2.State = query_address2.State;
                    model.AddressViewModel2.Zipcode = query_address2.Zipcode;
                    model.CountryName2 = query_country.CountryName;
                }
                ViewBag.message = id;
            
            return View(model);

        }
      
        [HttpGet]
        [CheckSessionOut]
        public ActionResult Save_education(int id)
        {
            using (LocatorContext dc = new LocatorContext())
            {
                var list = dc.DBContext_physicianeducation.Where(a => a.PhysicianEducationId == id).FirstOrDefault();
                return View(list);
            }
        }

        [HttpPost]
        public ActionResult Save_education(PhysicianEducationViewModel model)
        {
            int userid = Convert.ToInt32(Session["UserID"]);
            if (ModelState.IsValid)
            {
                using (LocatorContext context = new LocatorContext())
                {
                    var isactive = true;
              
                    var tempkendoInstitute = Request["kendoInstitute"];
                    string result = Convert.ToString(tempkendoInstitute);
                    var tempkendoStartDate = Request["kendoStartDate"];
                    string date = Convert.ToString(tempkendoStartDate);
                    //date=  date.Substring(0, 4);
                    string format = "1-1-";
                       format +=string.Format(" {0}", date);
                    tempkendoStartDate = format;
                    DateTime kendoStartDate = Convert.ToDateTime(tempkendoStartDate);
                    model.StartDate = kendoStartDate;
                  
                    model.IsActive = isactive;
                    int lastUserId = context.DBContext_register.Max(item => item.UserId);
                    model.UserId = userid;
                    model.CreatedBy = userid;
                    EducationInstitutesViewModel education = new EducationInstitutesViewModel();
                    education.CreatedBy = userid;
                    education.InstituteName = tempkendoInstitute;
                    var institute = context.DBContext_educationinstitute.Where(u => u.InstituteName == result).FirstOrDefault();
                    int lastcurrentAssignId = context.DBContext_educationinstitute.Max(item => item.EducationInstituteId);
                    education.CurrentAssignId = lastcurrentAssignId + 1;
                    if (institute == null)
                    {
                        var record_educationinstitute = new EducationInstitutesViewModel()
                        {
                            InstituteName = education.InstituteName,
                            ParentEducationInstituteId = 0,
                            IsActive = education.IsActive,
                            CurrentAssignId = education.CurrentAssignId,
                            IsDeleted = education.IsDeleted,
                            CreatedBy = education.CreatedBy,
                            CreatedOn = DateTime.Now,
                            LastModifiedBy = 0,
                            LastModifiedOn = DateTime.Now,
                        };
                        context.DBContext_educationinstitute.Add(education);
                        context.SaveChanges();
                    }
                    tempkendoInstitute = Request["kendoInstitute"];
                    result = Convert.ToString(tempkendoInstitute);
                    var currentassign = from educationinstitute in context.DBContext_educationinstitute
                                        where (educationinstitute.InstituteName.Equals(result))
                                        select new
                                        {
                                            educationinstitute.CurrentAssignId
                                        };
                    model.CurrentEducationInstituteId = Convert.ToInt32(currentassign.FirstOrDefault().CurrentAssignId);

                    var oldassign = from educationinstitute in context.DBContext_educationinstitute
                                    where (educationinstitute.InstituteName.Equals(result))
                                    select new
                                    {
                                        educationinstitute.EducationInstituteId
                                    };
                    model.OldEducationInstituteId = Convert.ToInt32(oldassign.FirstOrDefault().EducationInstituteId);

                    //EDUCATIONAL INSTITUTE----------------------------------

                    var record_physicianeducation = new PhysicianEducationViewModel()
                    {
                        UserId = model.UserId,
                        CurrentEducationInstituteId = model.CurrentEducationInstituteId,
                        Degree = model.Degree,
                        StartDate = model.StartDate,                        
                        IsActive = model.IsActive,
                        OldEducationInstituteId = model.OldEducationInstituteId,
                        IsDeleted = model.IsDeleted,
                        CreatedBy = model.CreatedBy,
                        CreatedOn = DateTime.Now,
                        LastModifiedBy = 0,
                        LastModifiedOn = DateTime.Now,
                    };
                    context.DBContext_physicianeducation.Add(model);
                    context.SaveChanges();
                }
            }
            return RedirectToAction("ViewUserProfile", "ManageUser", new { id = userid });
        }
        [CheckSessionOut]
        public ActionResult GetEducationlist()
        {
            LocatorContext db = new LocatorContext();
            int userid = Convert.ToInt32(Session["UserID"]);
            List<EducationGridModel> questionList = new List<EducationGridModel>();
            IQueryable<PhysicianEducationViewModel> tblregistrations = db.DBContext_physicianeducation;
            DataSourceResult result = new DataSourceResult();
            LocatorContext context = new LocatorContext();
            {
                var meds =from edu in context.DBContext_physicianeducation
                 join ins in context.DBContext_educationinstitute on edu.CurrentEducationInstituteId equals ins.EducationInstituteId
                 where (edu.UserId == userid && edu.IsActive && !edu.IsDeleted)
                 select new EducationGridModel
                 {
                     EduId = edu.PhysicianEducationId,
                     InstituteName = ins.InstituteName,
                     Degree = edu.Degree,
                     StartDate = edu.StartDate,
                    
                 };
                {
                    questionList = meds.ToList();
                }

                return Json(new
                {
                    data = questionList
                }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpGet]
        [CheckSessionOut]
        public ActionResult Edit_education(int id)
        {
            using (LocatorContext dc = new LocatorContext())
            {
                PhysicianEducationViewModel education_query = (from education in context.DBContext_physicianeducation where education.PhysicianEducationId == id select education).SingleOrDefault();
                EducationInstitutesViewModel institute_query = (from institute in context.DBContext_educationinstitute where institute.EducationInstituteId == education_query.CurrentEducationInstituteId select institute).SingleOrDefault();
                ViewBag.Message = institute_query.InstituteName;
                return View(education_query);
            }
        }

        [HttpPost]
        public ActionResult Edit_education(PhysicianEducationViewModel model, int id)
        {
            int userid = Convert.ToInt32(Session["UserID"]);
            if (ModelState.IsValid)
            {
                using (LocatorContext context = new LocatorContext())
                {
                    var isactive = true;

                    var tempkendoInstitute = Request["kendoInstitute"];
                    string result = Convert.ToString(tempkendoInstitute);
                    var tempkendoStartDate = Request["kendoStartDate"];
                    
                    string date = Convert.ToString(tempkendoStartDate);
                    //date=  date.Substring(0, 4);
                    string format = "1-1-";
                    format += string.Format(" {0}", date);
                    tempkendoStartDate = format;
                    DateTime kendoStartDate = Convert.ToDateTime(tempkendoStartDate);
                    model.StartDate = kendoStartDate;
                    model.IsActive = isactive;
                    int lastUserId = context.DBContext_register.Max(item => item.UserId);
                    model.UserId = userid;
                    EducationInstitutesViewModel education = new EducationInstitutesViewModel();
                    education.InstituteName = tempkendoInstitute;
                    var institute = context.DBContext_educationinstitute.Where(u => u.InstituteName == result).FirstOrDefault();
                    int lastcurrentAssignId = context.DBContext_educationinstitute.Max(item => item.EducationInstituteId);
                    education.CurrentAssignId = lastcurrentAssignId + 1;
                    model.CreatedBy = userid;
                    education.CreatedBy = userid;
                    if (institute == null)
                    {

                        var record_educationinstitute = new EducationInstitutesViewModel()
                        {
                            InstituteName = education.InstituteName,
                            ParentEducationInstituteId = 0,
                            IsActive = education.IsActive,
                            CurrentAssignId = education.CurrentAssignId,
                            IsDeleted = education.IsDeleted,
                            CreatedBy = education.CreatedBy,
                            CreatedOn = DateTime.Now,
                            LastModifiedBy = 0,
                            LastModifiedOn = DateTime.Now,

                        };
                        context.DBContext_educationinstitute.Add(education);
                        context.SaveChanges();
                        //status = true;
                    }
                    tempkendoInstitute = Request["kendoInstitute"];
                    result = Convert.ToString(tempkendoInstitute);
                    var currentassign = from educationinstitute in context.DBContext_educationinstitute
                                        where (educationinstitute.InstituteName.Equals(result))
                                        select new
                                        {
                                            educationinstitute.CurrentAssignId
                                        };
                    model.CurrentEducationInstituteId = Convert.ToInt32(currentassign.FirstOrDefault().CurrentAssignId);

                    var oldassign = from educationinstitute in context.DBContext_educationinstitute
                                    where (educationinstitute.InstituteName.Equals(result))
                                    select new
                                    {
                                        educationinstitute.EducationInstituteId
                                    };
                    model.OldEducationInstituteId = Convert.ToInt32(oldassign.FirstOrDefault().EducationInstituteId);

                    //EDUCATIONAL INSTITUTE----------------------------------

                    PhysicianEducationViewModel phy_query = (from physicaineducation in context.DBContext_physicianeducation where physicaineducation.PhysicianEducationId == id select physicaineducation).SingleOrDefault();
                    if (phy_query != null)
                    {

                        phy_query.CurrentEducationInstituteId = model.CurrentEducationInstituteId;
                        phy_query.Degree = model.Degree;
                        phy_query.StartDate = model.StartDate;
                    
                        phy_query.CreatedBy = model.CreatedBy;
                        phy_query.LastModifiedOn = DateTime.Now;
                        try
                        {
                            context.SaveChanges();
                        }
                        catch (Exception ex) { logger.Error(ex.ToString()); }
                    }

                }

            }
            return RedirectToAction("ViewUserProfile", "ManageUser", new
            {
                id = userid
            });

        }
        [CheckSessionOut]
        public ActionResult Delete_education(PhysicianEducationViewModel model, int id)
        {
            using (LocatorContext dc = new LocatorContext())
            {
                PhysicianEducationViewModel education_query = (from education in context.DBContext_physicianeducation where education.PhysicianEducationId == id select education).SingleOrDefault();
                return View(education_query);
            }
        }
        [HttpPost]
        public ActionResult Delete_education(int id)
        {
            PhysicianEducationViewModel model = new PhysicianEducationViewModel();

            var isdelete = true;
            model.IsDeleted = isdelete;
            int userid = Convert.ToInt32(Session["UserID"]);
            model.CreatedBy = userid;
            PhysicianEducationViewModel query_education = (from education in context.DBContext_physicianeducation where education.PhysicianEducationId == id select education).FirstOrDefault();

            if (query_education != null)
            {
                query_education.IsDeleted = model.IsDeleted;
                query_education.LastModifiedBy = userid;
                query_education.CreatedBy = model.CreatedBy;
                query_education.LastModifiedOn = DateTime.Now;

            }
            context.SaveChanges();
            return RedirectToAction("ViewUserProfile", "ManageUser", new
            {
                id = userid
            });


        }

        [HttpGet]
        [CheckSessionOut]
        public ActionResult Save_experience(int id)
        {

            string markers = "[";

            string conString = ConfigurationManager.ConnectionStrings["LocatorContext1"].ConnectionString;
            SqlCommand cmd = new SqlCommand("SELECT * FROM tblHospitals");
            var locationService = new GoogleLocationService();
         
            using (SqlConnection con = new SqlConnection(conString))
            {
                cmd.Connection = con;
                con.Open();

                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                       
                        markers += string.Format(" \"{0}\",", sdr["HospitalName"]);
                         
                            //markers += ",";
                        
                    }
                }
                con.Close();
            }
            markers += "];";
            ViewBag.Markers = markers;
           
           
            using (LocatorContext context = new LocatorContext())
            {
                var list = context.DBContext_experience.Where(a => a.PhysicianExperienceId == id).FirstOrDefault();
                return View(list);
            }
        }
        [HttpPost]
        public ActionResult Save_experience(PhysicianExperienceViewModel model)
        {
            int userid = Convert.ToInt32(Session["UserID"]);
            {
                var tempkendoInstitute = Request["kendoHospitalId"];
                string hospital = Convert.ToString(tempkendoInstitute);
                int lastUserId = context.DBContext_register.Max(item => item.UserId);
                var temJED = Request["kendoJED"];
                DateTime ED = Convert.ToDateTime(temJED);
                model.JoinEndDate = ED;
                var temJSD = Request["kendoJSD"];
                DateTime SD = Convert.ToDateTime(temJSD);
                model.JoinStartDate = SD;
                var hospiatl_query = context.DBContext_hospital.Where(u => u.HospitalName == hospital).FirstOrDefault();
                model.HospitalId = hospiatl_query.HospitalId;
                model.UserId = userid;
                model.CreatedBy = userid;
                var record_experience = new PhysicianExperienceViewModel()
                {
                    UserId = model.UserId,
                    HospitalId = model.HospitalId,
                    JoinStartDate = model.JoinStartDate,
                    JoinEndDate = model.JoinEndDate,
                    ReasonForLeaving = model.ReasonForLeaving,
                    Designation = model.Designation,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    CreatedBy = model.CreatedBy,
                    CreatedOn = model.CreatedOn,
                    LastModifiedBy = 0,
                    LastModifiedOn = model.LastModifiedOn,
                };
                context.DBContext_experience.Add(model);
                context.SaveChanges();
                //}
            }
            //return PartialView("_EducationDetails");
            return RedirectToAction("ViewUserProfile", "ManageUser", new
            {
                @id = userid
            });
        }

        public ActionResult GetExperiencelist()
        {
            int userid = Convert.ToInt32(Session["UserID"]);
            List<ExperienceGridModel> questionList = new List<ExperienceGridModel>();
            DataSourceResult result = new DataSourceResult();
            LocatorContext context = new LocatorContext();
            {

                var meds = from reg in context.DBContext_register
                           join exp in context.DBContext_experience on reg.UserId equals exp.UserId
                           where (exp.UserId == userid && exp.IsActive && !exp.IsDeleted)
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
                return Json(new
                {
                    data = questionList
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        [CheckSessionOut]
        public ActionResult Edit_experience(int id)
        {
            string markers = "[";

            string conString = ConfigurationManager.ConnectionStrings["LocatorContext1"].ConnectionString;
            SqlCommand cmd = new SqlCommand("SELECT * FROM tblHospitals");
            var locationService = new GoogleLocationService();

            using (SqlConnection con = new SqlConnection(conString))
            {
                cmd.Connection = con;
                con.Open();

                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {

                        markers += string.Format(" \"{0}\",", sdr["HospitalName"]);

                        //markers += ",";

                    }
                }
                con.Close();
            }
            markers += "];";
            ViewBag.Markers = markers;

            using (LocatorContext dc = new LocatorContext())
            {
               
                PhysicianExperienceViewModel experience_query = (from experience in context.DBContext_experience where experience.PhysicianExperienceId == id select experience).SingleOrDefault();
                HospitalsViewModel hospital_query = (from hospital in context.DBContext_hospital where hospital.HospitalId == experience_query.HospitalId select hospital).SingleOrDefault();
                ViewBag.Message = hospital_query.HospitalName;
                return View(experience_query);
            }
        }

        [HttpPost]
        public ActionResult Edit_experience(PhysicianExperienceViewModel model, int id)
        {
            int userid = Convert.ToInt32(Session["UserID"]);
            {
                int lastUserId = context.DBContext_register.Max(item => item.UserId);
                var temJED = Request["kendoJED"];
                var isactive = true;
                model.IsActive = isactive;
                DateTime ED = Convert.ToDateTime(temJED);
                model.JoinEndDate = ED;
                var temJSD = Request["kendoJSD"];
                DateTime SD = Convert.ToDateTime(temJSD);
                model.JoinStartDate = SD;
                var hospital = 1;
                model.HospitalId = hospital;
                model.UserId = userid;
                PhysicianExperienceViewModel phyexperience_query = (from physicainexperience in context.DBContext_experience where physicainexperience.PhysicianExperienceId == id select physicainexperience).SingleOrDefault();
                if (phyexperience_query != null)
                {

                    phyexperience_query.Designation = model.Designation;
                    phyexperience_query.ReasonForLeaving = model.ReasonForLeaving;
                    phyexperience_query.JoinStartDate = model.JoinStartDate;
                    phyexperience_query.JoinEndDate = model.JoinEndDate;
                    phyexperience_query.CreatedBy = model.CreatedBy;
                    phyexperience_query.LastModifiedOn = DateTime.Now;
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex) { logger.Error(ex.ToString()); }

                }

            }
            //return PartialView("_EducationDetails");
            return RedirectToAction("ViewUserProfile", "ManageUser", new
            {
                @id = userid
            });

        }
        [CheckSessionOut]
        public ActionResult Delete_experience(int id)
        {
            using (LocatorContext dc = new LocatorContext())
            {
                PhysicianExperienceViewModel experience_query = (from experience in context.DBContext_experience where experience.PhysicianExperienceId == id select experience).SingleOrDefault();
                return View(experience_query);
            }

        }
        [HttpPost]
        public ActionResult Delete_experience(PhysicianExperienceViewModel model, int id)
        {

            var isdelete = true;
            model.IsDeleted = isdelete;
            int userid = Convert.ToInt32(Session["UserID"]);
            model.CreatedBy = userid;
            PhysicianExperienceViewModel experience_query = (from experience in context.DBContext_experience where experience.PhysicianExperienceId == id select experience).SingleOrDefault();

            if (experience_query != null)
            {
                experience_query.IsDeleted = model.IsDeleted;
                experience_query.LastModifiedBy = userid;
                experience_query.CreatedBy = model.CreatedBy;
                experience_query.LastModifiedOn = DateTime.Now;

            }
            context.SaveChanges();
            return RedirectToAction("ViewUserProfile", "ManageUser", new
            {
                id = userid
            });


        }
        [HttpGet]
        [CheckSessionOut]
        public ActionResult Save_HealthHistory()
        {

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckSessionOut]
        public ActionResult Save_HealthHistory(HttpPostedFileBase upload, HealthHistoryViewModel model)
        {

            int userid = Convert.ToInt32(Session["UserID"]);


            DocumentViewModel document_model = new DocumentViewModel();

            int lasthealthhistoryid;

            var query_user = (from user in context.DBContext_register select user).Any();

            var query_healthhstory = (from health in context.DBContext_healthhistory select health).Any();

            if (!query_healthhstory)
            {
                lasthealthhistoryid = 1;
            }
            else
            {
                lasthealthhistoryid = context.DBContext_healthhistory.Max(item => item.HealthHistoryId + 1);
            }

            //int id = 2;
            if (ModelState.IsValid)
            {
                using (LocatorContext context = new LocatorContext())
                {

                    try
                    {
                        if (Request.Files.Count > 0)
                        {

                            if (upload != null && upload.ContentLength > 0)
                            {
                                string _FileName = Path.GetFileName(upload.FileName);
                                //string _path = Path.Combine(Server.MapPath("~/Temp"), _FileName);
                                //uploadFile.SaveAs(_path);
                                var tempkendoStartDate = Request["kendoStartDate"];
                                DateTime kendoStartDate = Convert.ToDateTime(tempkendoStartDate);
                                model.StartDate = kendoStartDate;
                                var tempkendoEndDate = Request["kendoEndDate"];
                                DateTime kendoEndDate = Convert.ToDateTime(tempkendoStartDate);
                                model.EndDate = kendoEndDate;
                                int lastUserId = context.DBContext_register.Max(item => item.UserId);
                                model.CreatedBy = userid;

                                var record_health = new HealthHistoryViewModel()
                                {

                                    Type = model.Type,
                                    Description = model.Type,
                                    StartDate = model.StartDate,
                                    EndDate = model.EndDate,
                                    IsActive = model.IsActive,
                                    IsDeleted = false,
                                    CreatedBy = model.CreatedBy,
                                    CreatedOn = DateTime.Now,
                                    LastModifiedBy = 0,
                                    LastModifiedOn = DateTime.Now,
                                };
                                context.DBContext_healthhistory.Add(model);
                                {
                                    document_model.DocumentName = _FileName;
                                    Directory.CreateDirectory(Server.MapPath("~/Images/" + userid + "/Personal"));
                                    var _path = Path.Combine(Server.MapPath("~/Images/" + userid + "/Personal"), _FileName);
                                    //var _path = Path.Combine(Server.MapPath("~/Images/" + id + "/Personal/"), _FileName);
                                    upload.SaveAs(_path);
                                    document_model.DocumentPath = _path;
                                    var table = "tblHealthHistories";
                                    document_model.PrimaryObjectId = table;
                                    document_model.PrimaryKeyId = lasthealthhistoryid;
                                    var document_record = new DocumentViewModel()
                                    {
                                        DocumentName = document_model.DocumentName,
                                        DocumentPath = document_model.DocumentPath,
                                        PrimaryKeyId = document_model.PrimaryKeyId,
                                        IsActive = model.IsActive,
                                        IsDeleted = false,
                                        CreatedBy = model.CreatedBy,
                                        CreatedOn = DateTime.Now,
                                        LastModifiedBy = 0,
                                        LastModifiedOn = DateTime.Now,
                                    };
                                    context.DBContext_document.Add(document_model);
                                    context.SaveChanges();
                                }
                                context.SaveChanges();
                            }
                            ViewBag.Message = "File Uploaded Successfully!!";

                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                        throw;
                    }

                }


            }
            return RedirectToAction("ViewUserProfile", "ManageUser", new
            {
                @id = userid
            });
            //return PartialView("_EducationDetails");
            //return View();

        }
        public ActionResult GetHealthHistorylist()
        {
            LocatorContext db = new LocatorContext();

            int userid = Convert.ToInt32(Session["UserID"]);
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


                return Json(new
                {
                    data = healthList
                }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpGet]
        [CheckSessionOut]
        public ActionResult Edit_HealthHistory(int id)
        {

            using (LocatorContext dc = new LocatorContext())
            {
                HealthHistoryViewModel health_query = (from health in context.DBContext_healthhistory where health.HealthHistoryId == id select health).SingleOrDefault();
                return View(health_query);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit_HealthHistory(HttpPostedFileBase upload, HealthHistoryViewModel model, int id)
        {

            int userid = Convert.ToInt32(Session["UserID"]);


            DocumentViewModel document_model = new DocumentViewModel();

            int lasthealthhistoryid;

            var query_user = (from user in context.DBContext_register select user).Any();

            var query_healthhstory = (from health in context.DBContext_healthhistory select health).Any();

            if (!query_healthhstory)
            {
                lasthealthhistoryid = 1;
            }
            else
            {
                lasthealthhistoryid = context.DBContext_healthhistory.Max(item => item.HealthHistoryId + 1);
            }

            //int id = 2;
            if (ModelState.IsValid)
            {
                using (LocatorContext context = new LocatorContext())
                {

                    try
                    {
                        if (Request.Files.Count > 0)
                        {

                            if (upload != null && upload.ContentLength > 0)
                            {
                                string _FileName = Path.GetFileName(upload.FileName);
                                //string _path = Path.Combine(Server.MapPath("~/Temp"), _FileName);
                                //uploadFile.SaveAs(_path);
                                var tempkendoStartDate = Request["kendoStartDate"];
                                DateTime kendoStartDate = Convert.ToDateTime(tempkendoStartDate);
                                model.StartDate = kendoStartDate;
                                var tempkendoEndDate = Request["kendoEndDate"];
                                DateTime kendoEndDate = Convert.ToDateTime(tempkendoStartDate);
                                model.EndDate = kendoEndDate;
                                int lastUserId = context.DBContext_register.Max(item => item.UserId);
                                model.CreatedBy = userid;

                                HealthHistoryViewModel health_query = (from health in context.DBContext_healthhistory where health.HealthHistoryId == id select health).SingleOrDefault();
                                DocumentViewModel document_query = (from document in context.DBContext_document
                                                                    where document.PrimaryKeyId == userid && document.PrimaryObjectId == "tblHealthHistories"
                                                                    select document).SingleOrDefault();


                                if (health_query != null)
                                {

                                    health_query.Type = model.Type;
                                    health_query.Description = model.Description;
                                    health_query.StartDate = model.StartDate;
                                    health_query.EndDate = model.EndDate;
                                    try
                                    {
                                        context.SaveChanges();
                                        TempData["Data"] = "Demerged Successful";
                                        return View("../Shared/Error");
                                    }
                                    catch (Exception ex) { logger.Error(ex.ToString()); }

                                    //}
                                    ModelState.Clear();
                                    //return Json(ModelState.ToDataSourceResult());

                                    //return Json(new[] { registrationViewModel }.ToDataSourceResult(request, ModelState));
                                }
                                context.DBContext_healthhistory.Add(model);
                                {
                                    document_model.DocumentName = _FileName;
                                    Directory.CreateDirectory(Server.MapPath("~/Images/" + userid + "/Personal"));
                                    var _path = Path.Combine(Server.MapPath("~/Images/" + userid + "/Personal"), _FileName);
                                    //var _path = Path.Combine(Server.MapPath("~/Images/" + id + "/Personal/"), _FileName);
                                    upload.SaveAs(_path);
                                    document_model.DocumentPath = _path;
                                    var table = "tblHealthHistories";
                                    document_model.PrimaryObjectId = table;
                                    document_model.PrimaryKeyId = lasthealthhistoryid;
                                    var document_record = new DocumentViewModel()
                                    {
                                        DocumentName = document_model.DocumentName,
                                        DocumentPath = document_model.DocumentPath,
                                        PrimaryKeyId = document_model.PrimaryKeyId,
                                        IsActive = model.IsActive,
                                        IsDeleted = false,
                                        CreatedBy = model.CreatedBy,
                                        CreatedOn = DateTime.Now,
                                        LastModifiedBy = 0,
                                        LastModifiedOn = DateTime.Now,
                                    };
                                    context.DBContext_document.Add(document_model);
                                    context.SaveChanges();
                                }
                                context.SaveChanges();
                            }
                            ViewBag.Message = "File Uploaded Successfully!!";

                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.ToString());
                        throw;
                    }

                }


            }
            return RedirectToAction("ViewUserProfile", "ManageUser", new
            {
                @id = userid
            });
            //return PartialView("_EducationDetails");
            //return View();

        }
        [CheckSessionOut]
        public ActionResult Delete_HealthHistory(PhysicianEducationViewModel model, int id)
        {
            using (LocatorContext dc = new LocatorContext())
            {
                //var v = dc.DBContext_physicianeducation.Where(a => a.PhysicianEducationId == id).FirstOrDefault();
                HealthHistoryViewModel health_query = (from health in context.DBContext_healthhistory where health.HealthHistoryId == id select health).SingleOrDefault();

                return View(health_query);

            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_HealthHistory(int id)
        {
            PhysicianEducationViewModel model = new PhysicianEducationViewModel();

            var isdelete = true;
            model.IsDeleted = isdelete;
            int userid = Convert.ToInt32(Session["UserID"]);
            model.CreatedBy = userid;
            HealthHistoryViewModel health_query = (from health in context.DBContext_healthhistory where health.HealthHistoryId == id select health).SingleOrDefault();

            if (health_query != null)
            {
                health_query.IsDeleted = model.IsDeleted;
                health_query.LastModifiedBy = userid;
                health_query.CreatedBy = model.CreatedBy;
                health_query.LastModifiedOn = DateTime.Now;

            }
            context.SaveChanges();
            return RedirectToAction("EditUserProfile", "ManageUser", new
            {
                id = userid
            });


        }
    }


}