using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace MyNote.RecentFile
{
    internal class Class1
    {

        public static void xx()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Recent);
            var files = Directory.EnumerateFiles(path);
            var dirs= Directory.EnumerateFileSystemEntries(path);
            //var xs = Directory.(path);
        }
    }
}
