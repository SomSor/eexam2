using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite
{
    public class WebUrlUtil
    {
        public static string Combine(string dir, string path)
        {
            dir = dir ?? string.Empty;
            path = path ?? string.Empty;

            dir = dir.TrimEnd('/').Trim();
            path = path.TrimStart('/').Trim();

            var combined = string.Format("{0}/{1}", dir, path);
            return combined.TrimStart('/');
        }
    }
}
