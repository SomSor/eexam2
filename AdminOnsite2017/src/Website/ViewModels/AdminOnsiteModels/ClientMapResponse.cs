using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnsiteModels
{
    public class ClientMapResponse
    {
        public string _id { get; set; }
        public string UUID { get; set; }
        public string ClientId { get; set; }
        public MessageRespone Message { get; set; }
    }
}
