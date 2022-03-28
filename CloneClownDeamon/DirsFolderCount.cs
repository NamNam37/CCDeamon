using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneClownDeamon
{
    public class DirsFolderCount
    {
        public int folderCount { get; set; }
        public string dest { get; set; }
        public DirsFolderCount(int folderCount, string dest)
        {
            this.folderCount = folderCount;
            this.dest = dest;
        }
    }
}
