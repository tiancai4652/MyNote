using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNote.Settings
{
    public class CacheConfig
    {
        public DateTime CreateTime { get; set; }
        public string? CurrentFile { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
