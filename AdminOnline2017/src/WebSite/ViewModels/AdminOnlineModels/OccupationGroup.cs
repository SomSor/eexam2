using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnlineModels
{
    public class OccupationGroup
    {
        public string _id { get; set; }
        public string Name { get; set; }
        public List<SubjectGroup> SubjectGroups { get; set; }
    }
}
