using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClient.Models
{
    public class SendResultArgs : EventArgs
    {
        public bool IsTimeUp { get; set; }
    }
}
