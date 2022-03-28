using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneClownDeamon
{
    public class Backup
    {
        FileManager filman = new FileManager();
        Configuration configuration { get; set; }
        private string configName { get; set; }
        private List<Paths> paths { get; set; }
        private int foldersMax { get; set; }
        private int rollbackMax { get; set; }
        private Type type { get; set; }

        private List<DirsFolderCount> folderCounts = new List<DirsFolderCount>();
        private enum Type
        {
            Full,
            Differencial,
            Incremental
        }
        public Backup(Configuration configuration) 
        {
            this.configuration = configuration;
            this.paths = configuration.paths;
            this.configName = configuration.name;
            this.rollbackMax = configuration.rollbackMax;
            this.foldersMax = configuration.foldersMax;
            int t = (int)configuration.type;
            type = (Type)t;

            MetadataManager mdmng = new MetadataManager(configuration);
            mdmng.InitMetadata();
            foreach (var item in mdmng.GetFileCount())
            {
                folderCounts.Add(item);
            }

        }
        public void Use()
        {
            MetadataManager mdmng = new MetadataManager(configuration);
            switch (type)
            {
                case Type.Full:
                    for (int i = 0; i < paths.Count; i++)
                    {
                        folderCounts[i].folderCount++;
                        mdmng.SetFileCount(paths[i].dest, folderCounts[i].folderCount);

                        FullBackup fb = new FullBackup();
                        fb.Use(paths[i], configuration, folderCounts[i].folderCount, i);
                    }
                    break;
                case Type.Differencial:
                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (folderCounts[i].folderCount == 0)
                        {
                            folderCounts[i].folderCount++;
                            mdmng.SetFileCount(paths[i].dest, folderCounts[i].folderCount);

                            FullBackup fb = new FullBackup();
                            fb.Use(paths[i], configuration, folderCounts[i].folderCount, i);
                            mdmng.SetSnapshot(fb.ssRows, i);
                        } else
                        {
                            folderCounts[i].folderCount++;
                            mdmng.SetFileCount(paths[i].dest, folderCounts[i].folderCount);

                            Differencial db = new Differencial();
                            db.Use(paths[i], configuration, folderCounts[i].folderCount, i);
                        }
                    }
                    break;
                case Type.Incremental:
                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (folderCounts[i].folderCount == 0)
                        {
                            folderCounts[i].folderCount++;
                            mdmng.SetFileCount(paths[i].dest, folderCounts[i].folderCount);

                            FullBackup fb = new FullBackup();
                            fb.Use(paths[i], configuration, folderCounts[i].folderCount, i);
                            mdmng.SetSnapshot(fb.ssRows, i);
                        } else
                        {

                        }
                    }
                    break;
                default:
                    break;
            }
        }
        

        

    }
}
