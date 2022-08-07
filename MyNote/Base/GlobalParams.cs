using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Base
{
    public class GlobalParams
    {
        public static string AppDataFolder
        {
            get
            {
                var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\MyNote";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                return folder;
            }
        }

        public static string ConfigFile
        {
            get
            {
                return AppDataFolder + "\\" + "cahe.config";
            }
        }

        static string _CurrentFile=String.Empty;
        public static string CurrentFile
        {
            get
            {
                if (string.IsNullOrEmpty(_CurrentFile))
                {
                    _CurrentFile = GlobalParams.AppDataFolder + "\\" + Guid.NewGuid().ToString("N");
                    if (!File.Exists(_CurrentFile))
                    {
                        File.Create(_CurrentFile).Close();
                    }
                }
                return _CurrentFile;
            }
            set
            {
                _CurrentFile = value;
            }
        }
    }
}
