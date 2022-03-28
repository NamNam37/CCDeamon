using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CloneClownDeamon
{
    public class MetadataManager
    {
        private string configName { get; set; }
        private List<Paths> configPaths { get; set; }
        private string metadataPath { get; set; }
        private string metadataPathSS { get; set; }
        private string metadataPathID { get; set; }
        private string metadataPathFC { get; set; }
        public MetadataManager(Configuration config)
        {
            this.configPaths = config.paths;
            this.configName = config.name;
            this.metadataPath = $"../../../";
            this.metadataPathFC = metadataPath + $"{configName}/fileCount.txt";
            this.metadataPathID = metadataPath + $"{configName}/IDs.txt";
            this.metadataPathSS = metadataPath + $"{configName}/snapshots/";
        }
        public void InitMetadata()
        {
            FileManager filman = new FileManager();
            if (!File.Exists(metadataPathFC) || GetFileCount().Count == 0)
            {
                
                filman.MkDir(metadataPath, configName);
                filman.MkFile(metadataPathFC);

                using (StreamWriter writer = new StreamWriter(metadataPathFC))
                {
                    foreach (var paths in configPaths)
                    {
                        writer.WriteLine($"0;{paths.dest}");
                    }
                }
            }
            if (!Directory.Exists(metadataPathSS))
            {
                filman.MkDir(metadataPathSS);
            }
            /*if (!File.Exists(metadataPathID))
            {
                filman.MkFile(metadataPathID);
                SetDestIDs(configPaths);
            }*/
        }
        public List<DirsFolderCount> GetFileCount()
        {
            List<DirsFolderCount> data = new List<DirsFolderCount>();
            
            using (StreamReader reader = new StreamReader(metadataPathFC))
            {
                while (!reader.EndOfStream)
                {
                    string[] line = reader.ReadLine().Split(';');
                    DirsFolderCount lineFolderCount = new DirsFolderCount(0,"");
                    if (line.Length != 0)
                    {
                        try
                        {
                            lineFolderCount = new DirsFolderCount(int.Parse(line[0]), line[1]);
                        }
                        catch
                        {
                            throw new FileLoadException("Metadata are corrupted");
                        }
                        
                    }
                    data.Add(lineFolderCount);
                }
            }
            return data;
        }
        public void SetFileCount(string dest, int value)
        {
            bool found = false;
            List<DirsFolderCount> data = GetFileCount();
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].dest == dest)
                {
                    data[i].folderCount = value;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                data.Add(new DirsFolderCount(0, dest));
            }
            
            
            using (StreamWriter writer = new StreamWriter(metadataPathFC))
            {
                foreach (var item in data)
                {
                    writer.WriteLine($"{item.folderCount};{item.dest}");
                   
                }

            }
        }
        public List<SnapShotRow> GetSnapshot(int backupID, int destID)
        {
            List<SnapShotRow> data = new List<SnapShotRow>();
            using (StreamReader reader = new StreamReader($"{metadataPathSS}{backupID}_{destID}.txt"))
            {
                while (!reader.EndOfStream)
                {
                    string[] line = reader.ReadLine().Split(';');
                    SnapShotRow linessRow = new SnapShotRow("", default, false);
                    if (line.Length != 0)
                    {
                        try
                        {
                            linessRow = new SnapShotRow(line[0], DateTime.Parse(line[1]), bool.Parse(line[2]));
                        }
                        catch
                        {
                            throw new FileLoadException("Metadata are corrupted");
                        }

                    }
                    data.Add(linessRow);
                }
            }
            return data;
        }
        public void SetSnapshot(List<SnapShotRow> ssRows, int backupID)
        {
            FileManager filman = new FileManager();

            List<DirsFolderCount> fileCount = GetFileCount();

            if (!File.Exists($"{metadataPathSS}{fileCount[backupID].folderCount}_{backupID}.txt") && ssRows != null)
            {
                filman.MkFile($"{metadataPathSS}{fileCount[backupID].folderCount}_{backupID}.txt");
                using (StreamWriter writer = new StreamWriter($"{metadataPathSS}{fileCount[backupID].folderCount}_{backupID}.txt"))
                {
                    for (int q = 0; q < ssRows.Count; q++)
                    {
                        writer.WriteLine($"{ssRows[q].path};{ssRows[q].modifyTime};{ssRows[q].isFile}");
                    }
                }
            }
        }
        /*private void SetDestIDs(List<Paths> paths)
        {
            
            using (StreamWriter writer = new StreamWriter(metadataPathID))
            {
                for (int i = 0; i < paths.Count; i++)
                {
                    writer.WriteLine(paths[i].dest);
                }
            }
        }
        public List<string> GetDestIDs()
        {
            List<string> data = new List<string>();

            using (StreamReader reader = new StreamReader(metadataPathID))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    data.Add(line);
                }
            }
            return data;
        }*/

    }
}
