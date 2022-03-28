using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneClownDeamon
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Configuration> usersConfigs { get; set; }
        public DateTime lastBackup { get; set; }
        public string MAC { get; set; }
        public string IP { get; set; }
    }
}
