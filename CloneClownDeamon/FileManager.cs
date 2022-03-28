using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneClownDeamon
{
    public class FileManager
    {
        private List<SnapShotRow> ssRows { get; set; }
        public void CopyFile(string sourcePath, string destPath, string sourceFileName, string destFileName)
        {
            if (!File.Exists(sourcePath + '/' + sourceFileName))
                throw new FileNotFoundException(sourcePath + '/' + sourceFileName);

            File.Copy(sourcePath + '/' + sourceFileName, destPath + '/' + destFileName, false);
        }
        public void CopyFile(string sourcePathWithFileName, string destPathWithFileName)
        {
            if (!File.Exists(sourcePathWithFileName))
                throw new FileNotFoundException(sourcePathWithFileName);
         
            File.Copy(sourcePathWithFileName, destPathWithFileName, false);
        }
        public void CopyFolder(string sourcePath, string destPath)
        {
            if (!Directory.Exists(sourcePath))
                throw new DirectoryNotFoundException(sourcePath);

            CopySubFolders(new DirectoryInfo(sourcePath), new DirectoryInfo(destPath));
        }
        private void CopySubFolders(DirectoryInfo sourcePath, DirectoryInfo destPath)
        {
            Directory.CreateDirectory(destPath.FullName);

            foreach (FileInfo file in sourcePath.GetFiles())
            {
                file.CopyTo(Path.Combine(destPath.FullName, file.Name), false);
            }
            foreach (DirectoryInfo sourceSubDirs in sourcePath.GetDirectories())
            {
                DirectoryInfo nextDestSubDir = destPath.CreateSubdirectory(sourceSubDirs.Name);
                CopySubFolders(sourceSubDirs, nextDestSubDir);
            }

        }
        public List<SnapShotRow> CreateSnapshot(string path)
        {
            ssRows = new List<SnapShotRow>();
            CreateSubSnapshot(new DirectoryInfo(path));
            return ssRows;
        }
        private void CreateSubSnapshot(DirectoryInfo path)
        {
            ssRows.Add(new SnapShotRow(path.FullName, default, false));

            foreach (FileInfo file in path.GetFiles())
            {
                ssRows.Add(new SnapShotRow(Path.Combine(path.FullName, file.Name), file.LastWriteTime, true));
            }
            foreach (DirectoryInfo sourceSubDirs in path.GetDirectories())
            {
                DirectoryInfo subPath = new DirectoryInfo(Path.Combine(path.FullName, sourceSubDirs.Name));
                CreateSubSnapshot(subPath);
            }
        }

        public void Delete(string path)
        {
            try
            {
                DirectoryInfo pathInfo = new DirectoryInfo(path);
                pathInfo.Delete(true);
            }
            catch (Exception) 
            { 
                FileInfo pathInfo = new FileInfo(path);
                pathInfo.Delete();
            }
            
        }
        public void Delete(string path, bool onlyDeleteContent)
        {
            if (onlyDeleteContent)
            {
                DirectoryInfo pathInfo = new DirectoryInfo(path);
                pathInfo.Delete(true);
                pathInfo.Create();
            }
            else
            {
                Delete(path);
            }
        }
        public void MkDir(string path, string name)
        {
            Directory.CreateDirectory(path+'/'+name);
        }
        public void MkFile(string path, string name)
        {
            File.Create(path + '/' + name).Close();
        }
        public void MkDir(string path)
        {
            Directory.CreateDirectory(path);
        }
        public void MkFile(string path)
        {
            File.Create(path).Close();
        }
    }
}
