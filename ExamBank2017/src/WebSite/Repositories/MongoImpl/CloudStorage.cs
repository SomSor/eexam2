using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WebSite.Repositories.MongoImpl
{
    public class CloudStorage : ICloudStorage
    {
        private CloudStorageAccount cloudAcc;

        public CloudStorage(CloudStorageAccount cloudAcc)
        {
            this.cloudAcc = cloudAcc;
        }

        public CloudStorageAccount GetCloudStorageAcc()
        {
            return this.cloudAcc;
        }
    }
}
