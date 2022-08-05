using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Settings
{
    public class AutoSaveConfig
    {
        public static int SaveInternalMs { get; set; } = 5*1000;
        public static int SaveSteps { get; set; } = 10;
    }
}
