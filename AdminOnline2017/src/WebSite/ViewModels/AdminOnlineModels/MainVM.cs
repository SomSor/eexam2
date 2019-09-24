using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnlineModels
{
    public class MainVM
    {
        public List<TestRegistration> Testregistrations { get; set; }
        public List<DateTime> AppointDates { get; set; }
    }
}
