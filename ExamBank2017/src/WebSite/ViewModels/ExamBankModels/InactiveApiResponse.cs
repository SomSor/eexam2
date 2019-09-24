using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.ExamBankModels
{
    public class InactiveApiResponse
    {
        public enum ResponseCode
        {
            success,
            noinactivesubject,
            notacceptallquestion,
            existingexamsuite,
            nooption,
        }
        public string Message { get; set; }
        public ResponseCode Code { get; set; }
    }
}
