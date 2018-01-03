using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianLocator.Models
{
    public class UserRegistrationViewModel
    {
        public RegistrationViewModel RegistrationViewModel { get; set; }
        public AddressViewModel AddressViewModel1 { get; set; }
        public AddressViewModel AddressViewModel2 { get; set; }
        public PhysicianReferencesViewModel PhysicianReferencesViewModel { get; set; }
        public PhysicianEducationViewModel PhysicianEducationViewModel { get; set; }
        public EducationInstitutesViewModel EducationInstitutesViewModel { get; set; }
        public PhysicianExperienceViewModel PhysicianExperienceViewModel { get; set; }
    }
}