using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianLocator.Models
{
    public class TempDetailModel
    {

        public int Lookupdetailid {get;set;}
    //LookupCategoryId  
    public string Category { get; set; }

    //SortOrder
    public int SortOrder { get; set; }

    //Key
    public int Key { get; set; }

    //Value

    public string Value { get; set; }

    //IsActive
    public bool IsActive { get; set; }

    //IsDeleted
    public bool IsDeleted { get; set; }

    //CreatedBy
    public int CreatedBy { get; set; }

    //CreatedOn
    public DateTime CreatedOn { get; set; }

    //LastModifiedBy
    public int LastModifiedBy { get; set; }

    //LastModifiedOn
    public DateTime LastModifiedOn { get; set; }

    public TempDetailModel()
    {
        CreatedOn = DateTime.Now;
        LastModifiedOn = DateTime.Now;
        IsActive = true;
        IsDeleted = false;

    }
}
}