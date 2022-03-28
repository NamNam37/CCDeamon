using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneClownDeamon
{
    public class Logger
    {
        private int logNum { get; set; }
        public void Log(Paths paths, DateTime dateTime, string e, string backupName)
        {
            if (!Directory.Exists("../../../logs"))
            {
                new FileManager().MkDir("../../../","logs");
            }
            logNum = Directory.GetFiles("../../../logs").Length;
            using (StreamWriter writer = new StreamWriter($"../../../logs/{backupName}_{logNum}_log.txt"))
            {
                writer.WriteLine(backupName);
                writer.WriteLine("Source: " + paths.source);
                writer.WriteLine("Destination: " + paths.dest);
                writer.WriteLine($"Time: {dateTime.Hour}:{dateTime.Minute}:{dateTime.Second}");
                writer.WriteLine($"Date: { dateTime.Day}/{ dateTime.Month}/{ dateTime.Year}");
                writer.WriteLine("Error: " + e);
            }

        }
    }
}
