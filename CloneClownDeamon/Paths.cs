using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneClownDeamon
{
    public class Paths
    {
        public string source { get; set; }
        public string dest { get; set; }
        public Paths(string source, string dest)
        {
            this.source = source;
            this.dest = dest;
        }
    }
}
