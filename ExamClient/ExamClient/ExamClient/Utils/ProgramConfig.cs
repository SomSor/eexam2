using ExamClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamClient.Utils
{
    public class ProgramConfig
    {
        public static Config ReadConfigFile()
        {
            //Configuration = null;
            string[] files = new string[0];
            try
            {
                var folder = @"C:\PCTest\";
                files = Directory.GetFiles(folder, string.Format("CONFIG.{0}", "zip"));
            }
            catch (DirectoryNotFoundException)
            {
                return null;
            }
            if (files.Length > 0 && File.Exists(files[0]))
            {
                string configData = File.ReadAllText(files[0]);
                //Configuration = JsonConvert.DeserializeObject<Config>(configData);
                return JsonConvert.DeserializeObject<Config>(configData);
            }
            else
            {
                return null;
            }
        }
    }
}
