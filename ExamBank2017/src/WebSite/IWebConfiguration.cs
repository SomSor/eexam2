using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite
{
    public interface IWebConfiguration
    {
        string AspNetDbConnectionString { get; set; }
        string MongoDbConnectionString { get; set; }
        string MongoDatabaseName { get; set; }
        string StorageConnectionString { get; set; }
        string BlobName { get; set; }
    }
}
