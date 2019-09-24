using ExamClient.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClient.Models
{
    public class ActiveResponse : MVVMBase
    {
        public DateTime ActiveDateTime { get; set; }
        public DateTime ActiveThruTime { get; set; }
        public bool IsActive { get; set; }
    }
}
