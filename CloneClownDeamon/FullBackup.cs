using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneClownDeamon
{
    public class FullBackup
    {
        public List<SnapShotRow> ssRows { get; set; }
        public FullBackup()
        {
            
        }
        
        public void Use(Paths paths, Configuration config, int folderCount, int destID)
        {
            Logger logger = new Logger();
            FileManager filman = new FileManager();
            DateTime dateTime = DateTime.Now;
            string error = "none";

            string backupFolder = $"FullBackup_{folderCount}";
            try
            {
                filman.CopyFolder(paths.source, $"{paths.dest}/{backupFolder}_{config.name}");
                ssRows = filman.CreateSnapshot(config.paths[destID].source);

            }
            catch (Exception e)
            {
                error = e.Message;
            }
            logger.Log(paths, dateTime, error, backupFolder);
        }
    }
}
