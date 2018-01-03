using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhysicianLocator.DAL;
using PhysicianLocator.Models;
using System.Data.Entity;

namespace PhysicianLocator.Controllers
{
    public class HospitalController : Controller
    {

        public JsonResult HospitalName()
        {
            var northwind = new LocatorContext();
            return Json(northwind.DBContext_hospital.Select(c => new { HospitalName = c.HospitalName, IsActive = c.IsActive }).Where(u => u.IsActive), JsonRequestBehavior.AllowGet);
        }
        // GET: Hospital
        [HttpGet]
        public ActionResult InsertHospital()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsertHospital(HospitalsViewModel model)
        {
            var isactive = true;
            if (ModelState.IsValid)
            {
                using (LocatorContext context = new LocatorContext())
                {
                    model.IsActive = isactive;
                    var record = new HospitalsViewModel()
                    {
                        HospitalName = model.HospitalName,
                        NumberOfBeds = model.NumberOfBeds,
                        NumberOfBranches = model.NumberOfBranches,
                        NumberOfDoctors = model.NumberOfDoctors,
                        Specialities = model.Specialities,
                        IsActive = model.IsActive,
                        IsDeleted = model.IsDeleted,
                        CreatedBy = 0,
                        CreatedOn = DateTime.Now,
                        LastModifiedBy = 0,
                        LastModifiedOn = DateTime.Now,
                    };
                    context.DBContext_hospital.Add(model);
                    context.SaveChanges();
                }
            }
            return View(model);
        }
        public ActionResult ManageHospital()
        {
            return View();
        }
        public ActionResult GetHospitallist()
        {
                LocatorContext context = new LocatorContext();
            var hospital = context.DBContext_hospital.Where(u => u.IsActive).ToList();
            return Json(new { data = hospital }, JsonRequestBehavior.AllowGet);
        
        }
        [HttpGet]
        public ActionResult Edit_hospital(int id)
        {
            AdminHospitalViewModel model = new AdminHospitalViewModel();
            LocatorContext context = new LocatorContext();
           
            HospitalsViewModel hospital = context.DBContext_hospital.Where(u => u.HospitalId == id).FirstOrDefault();
            if (hospital !=null)
            {
                model.HospitalId = hospital.HospitalId;
                model.HospitalName = hospital.HospitalName;
                model.NumberOfBeds = hospital.NumberOfBeds;
                model.NumberOfBranches = hospital.NumberOfBranches;
                model.NumberOfDoctors = hospital.NumberOfDoctors;
                model.Specialities = hospital.Specialities;
             
            }
          

            AddressViewModel query_address = (from address in context.DBContext_address where address.PrimaryKeyId == id && address.IsPrimary select address).SingleOrDefault();

            if (query_address != null)
            {
                CountryViewModels query_country = (from country in context.DBContext_country where country.CountryId == query_address.CountryId select country).SingleOrDefault();

                model.AddressViewModel1.Street1 = query_address.Street1;
                model.AddressViewModel1.Street2 = query_address.Street2;
                model.AddressViewModel1.Street3 = query_address.Street3;
                model.AddressViewModel1.City = query_address.City;
                model.AddressViewModel1.State = query_address.State;
                model.AddressViewModel1.Zipcode = query_address.Zipcode;
                model.AddressViewModel1.CountryId = query_address.CountryId;
                model.CountryName1 = query_country.CountryName;
            }
            return View(model);
        }
        [HttpPost]
        public JsonResult Edit_hospital(HospitalsViewModel model)
        {
            //HospitalsViewModel model = new HospitalsViewModel();
            LocatorContext context = new LocatorContext();

            HospitalsViewModel hospital = context.DBContext_hospital.Where(u => u.HospitalId == model.HospitalId).FirstOrDefault();
            if (hospital != null)
            {
                hospital.HospitalName = model.HospitalName;
                hospital.NumberOfBeds = model.NumberOfBeds;
                hospital.NumberOfBranches = model.NumberOfBranches;
                hospital.NumberOfDoctors = model.NumberOfDoctors;
                hospital.Specialities = model.Specialities;
                hospital.LastModifiedOn = DateTime.Now;
            }
            context.SaveChanges();

            //return View(model);
            string message = "Success";
            return Json(message, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult Edit_hospitalAddress(int id)
        {
            LocatorContext context = new LocatorContext();
            AddressViewModel model = new AddressViewModel();
        
            AddressViewModel query_address = (from address in context.DBContext_address where address.PrimaryKeyId == id && address.IsPrimary select address).SingleOrDefault();

            if (query_address != null)
            {
                CountryViewModels query_country = (from country in context.DBContext_country where country.CountryId == query_address.CountryId select country).SingleOrDefault();

                model.Street1 = query_address.Street1;
                model.Street2 = query_address.Street2;
                model.Street3 = query_address.Street3;
                model.City = query_address.City;
                model.State = query_address.State;
                model.Zipcode = query_address.Zipcode;
                model.CountryId = query_address.CountryId;
                //model.CountryName = query_country.CountryName;
            }
            return View(model);
        }
        [HttpPost]
        public JsonResult Edit_hospitalAddress(HospitalsViewModel model)
        {
            //HospitalsViewModel model = new HospitalsViewModel();
            LocatorContext context = new LocatorContext();

            HospitalsViewModel hospital = context.DBContext_hospital.Where(u => u.HospitalId == model.HospitalId).FirstOrDefault();
            if (hospital != null)
            {
                hospital.HospitalName = model.HospitalName;
                hospital.NumberOfBeds = model.NumberOfBeds;
                hospital.NumberOfBranches = model.NumberOfBranches;
                hospital.NumberOfDoctors = model.NumberOfDoctors;
                hospital.Specialities = model.Specialities;
                hospital.LastModifiedOn = DateTime.Now;
            }
            context.SaveChanges();

            //return View(model);
            string message = "Success";
            return Json(message, JsonRequestBehavior.AllowGet);

        }


        //public JsonResult Edit_hospital(int HospitalId, string HospitalName)
        //{
        //    LocatorContext context = new LocatorContext();
        //    var result = "success";
        //    try
        //    {
        //        //define the model  
        //        HospitalsViewModel model = new HospitalsViewModel();
        //        model.HospitalId = HospitalId;
        //        model.HospitalName = HospitalName;

        //        //for insert recored..  
        //         context.Entry(model).State = EntityState.Modified;


        //        context.SaveChanges();


        //    }
        //    catch (Exception ex)
        //    {
        //         result = "failed";

        //    }
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult ChangeUser(AdminHospitalViewModel model)
        {
            // Update model to your db  
            LocatorContext context = new LocatorContext();

            HospitalsViewModel hospital = context.DBContext_hospital.Where(u => u.HospitalId == model.HospitalId).FirstOrDefault();
            if (hospital != null)
            {
                hospital.HospitalName = model.HospitalName;
                hospital.NumberOfBeds = model.NumberOfBeds;
                hospital.NumberOfBranches = model.NumberOfBranches;
                hospital.NumberOfDoctors = model.NumberOfDoctors;
                hospital.Specialities = model.Specialities;
                hospital.LastModifiedOn = DateTime.Now;
                try
                {
                    context.Entry(hospital).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (Exception ex) { }
            }
                   
            //string message = "Success";
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetHospitallist()
        //{
        //    LocatorContext context = new LocatorContext();
        //    var employees = context.DBContext_hospital.Where(u => u.IsActive).ToList();
        //    return Json(new { data = employees }, JsonRequestBehavior.AllowGet);

        //}
        //public ActionResult GetHospitallist()
        //{
        //    LocatorContext context = new LocatorContext();
        //    var employees = context.DBContext_hospital.Where(u => u.IsActive).ToList();
        //    return Json(new { data = employees }, JsonRequestBehavior.AllowGet);

        //}

    }
}