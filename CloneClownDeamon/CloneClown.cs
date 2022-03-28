using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneClownDeamon
{
    public class CloneClown
    {
        public void Start()
        {
            Configuration demo = new Configuration();
            demo.SetDemoConfig();

            Backup backup = new Backup(demo);
            while (true)
            {
                ConsoleKey input = Console.ReadKey(true).Key;
                Console.WriteLine("Backup In Progress");
                if (input == ConsoleKey.B)
                    backup.Use();
                Console.WriteLine("Done");
            }
        }
    }
}
