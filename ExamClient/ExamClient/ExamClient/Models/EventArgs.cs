using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClient.Models
{
    public class VoiceEventArgs : EventArgs
    {
        public Queue<Uri> Voices { get; set; }
    }
}
