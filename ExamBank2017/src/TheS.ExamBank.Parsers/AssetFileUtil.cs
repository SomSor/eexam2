using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Ionic.Zip;

namespace TheS.ExamBank.Parsers
{
    public class AssetFileUtil
    {
        
        public static IEnumerable<string> ReadZipEntries(Stream zipStream)
        {
            List<string> fileList = null;

            using (ZipFile zip = ZipFile.Read(zipStream))
            {
                fileList = new List<string>(zip.Count);
                foreach (ZipEntry e in zip)
                {
                    fileList.Add(e.FileName);
                }
            }

            return fileList;
        }

        public static IEnumerable<Asset> GetAssets(Stream zipStream, IEnumerable<string> assets)
        {
            List<Asset> extracted = null;

            using (ZipFile zip = ZipFile.Read(zipStream))
            {
                extracted = new List<Asset>(zip.Count);

                foreach (ZipEntry e in zip)
                {
                    var fname = e.FileName.ToLower(); //HACK file extension capitalize has different e.g. .PNG <-> .png
                    var asset = assets.FirstOrDefault(it => fname.EndsWith(it.ToLower()));

                    if (asset != null)
                    {
                        var destPath = Path.GetTempFileName();
                        using (var des = File.OpenWrite(destPath))
                        {
                            e.Extract(des);
                        }
                        extracted.Add(new Asset
                        {
                            AssetName = asset,
                            Path = destPath,
                        });
                    }
                }
            }

            return extracted;
        }

        public static IEnumerable<Asset> GetLocalAssets(string dir, IEnumerable<string> assets)
        {
            var assetSourcePaths = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
            List<Asset> extracted = new List<Asset>(assetSourcePaths.Length);

            foreach (var asset in assets)
            {
                var convPath = asset.Replace('/', Path.DirectorySeparatorChar);
                var srcPath = assetSourcePaths.FirstOrDefault(it => it.EndsWith(convPath));

                if (srcPath != null)
                {
                    var destPath = Path.GetTempFileName();
                    File.Copy(srcPath, destPath, true);
                    extracted.Add(new Asset
                    {
                        AssetName = asset,
                        Path = destPath,
                    });
                }
            }

            return extracted;
        }
        
        public static void CleanUpAssets(IEnumerable<Asset> assets, IFile file)
        {
            foreach (var asset in assets)
            {
                var path = asset.Path;
                try
                {
                    if (File.Exists(path))
                    {
                        file.DeleteFile(path);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                    // Ignore any exception
                }
            }
        }

        public class Asset
        {
            public string AssetName { get; set; }
            public string Path { get; set; }
        }
    }
}
