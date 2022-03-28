using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneClownDeamon
{
    public class Differencial
    {

        public void Use(Paths paths, Configuration config, int folderCount, int destID)
        {
            Logger logger = new Logger();
            FileManager filman = new FileManager();
            MetadataManager mdman = new MetadataManager(config);
            DateTime dateTime = DateTime.Now;
            string error = "none";
            List<SnapShotRow> usedFull = mdman.GetSnapshot(1, destID);
            List<SnapShotRow> newSnapshot = filman.CreateSnapshot(config.paths[destID].source);
            List<SnapShotRow> diffToBeUsed = new List<SnapShotRow>();
            mdman.SetSnapshot(newSnapshot, destID);

            for (int d = 0; d < newSnapshot.Count; d++)
            {
                bool found = false;
                for (int f = 0; f < usedFull.Count; f++)
                {
                    Console.WriteLine(usedFull[f].path + "    " + newSnapshot[d].path);
                    Console.WriteLine(usedFull[f].modifyTime + "    " + newSnapshot[d].modifyTime + "     " + usedFull[f].modifyTime.Subtract(newSnapshot[d].modifyTime).TotalSeconds + "      " + (usedFull[f].modifyTime.Subtract(newSnapshot[d].modifyTime).TotalSeconds < -2));
                    if (usedFull[f].path == newSnapshot[d].path && usedFull[f].modifyTime.Subtract(newSnapshot[d].modifyTime).TotalSeconds > -2 && usedFull[f].isFile)
                    {
                        found = true;
                        Console.WriteLine("found");
                    }
                }
                if (!found)
                {
                    diffToBeUsed.Add(newSnapshot[d]);
                    Console.WriteLine("added");
                }
            }
            Console.WriteLine("---------------------");
            string backupFolder = $"/DiffBackup_{folderCount}";
            try
            {
                foreach (var snapShotRow in diffToBeUsed)
                {
                    if (!snapShotRow.isFile)
                    {
                        
                        filman.MkDir(paths.dest + backupFolder + $"_{config.name}/" + snapShotRow.path.Remove(0,paths.source.Length).Replace('\\', '/'));
                    }

                }
                foreach (var snapShotRow in diffToBeUsed)
                {
                    if (snapShotRow.isFile)
                    {
                        filman.MkFile(paths.dest + backupFolder + $"_{config.name}/" + snapShotRow.path.Remove(0, paths.source.Length).Replace('\\', '/'));
                    }
                }

            }
            catch (Exception e)
            {
                error = e.Message;
            }
            logger.Log(paths, dateTime, error, backupFolder);
        }
    }
}

