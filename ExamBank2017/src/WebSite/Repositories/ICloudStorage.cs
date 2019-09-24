using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Repositories
{
    public interface ICloudStorage
    {
        CloudStorageAccount GetCloudStorageAcc();
    }
}
