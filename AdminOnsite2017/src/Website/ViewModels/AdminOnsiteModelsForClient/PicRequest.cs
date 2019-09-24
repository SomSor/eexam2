using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.ViewModels.AdminOnsiteModelsForClient
{
    public class PicRequest
    {
        public string FileName { get; set; }
        public byte[] bytes { get; set; }
    }
}
