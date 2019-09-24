using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TheS.ExamBank.Parsers
{
    public class FileHelper : IFile
    {
        public void DeleteFile(string path)
        {
            File.Delete(path);
        }
        //public void DeleteFile(string path)
        //{
        //    throw new NotImplementedException();
        //}
    }
}