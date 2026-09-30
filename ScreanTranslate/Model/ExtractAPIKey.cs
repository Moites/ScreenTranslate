using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreanTranslate.Model
{
    public class ExtractAPIKey
    {
        public static string Key()
        {
            string path =  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "API_key.txt");
            return File.ReadAllText(path);
        }
    }
}
