using ExamClient.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClient.Models
{
    public class ClientSheetRequest : MVVMBase
    {
        public string SheetId { get; set; }
        public string ClientId { get; set; }
    }
}
