using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite
{
    public class WebConfiguration : IWebConfiguration
    {
        public string AspNetDbConnectionString { get; set; }
        public string MongoDbConnectionString { get; set; }
        public string MongoDatabaseName { get; set; }
        public string StorageConnectionString { get; set; }
        public string BlobName { get; set; }
    }
}
