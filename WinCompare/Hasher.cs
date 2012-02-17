namespace WinCompare
{
    using System.Collections.Generic;
    using System.IO;

    public class Hasher : IHasher
    {
        private readonly List<string> ignoreList;

        private readonly FileList Files;

        public Hasher(IHash hash, List<string> ignoreList)
        {
            this.ignoreList = ignoreList;
            Files = new FileList(hash);
        }

        public Dictionary<string, string> GetFileHashList()
        {
            return Files.Files;
        }

        public void ExamineDir(string strDir, string rootDir)
        {
            foreach (var strDirName in Directory.GetDirectories(strDir))
            {
                var folderName = Path.GetFileName(strDirName);
                if (!ignoreList.Contains(folderName))
                {
                    foreach (var file in Directory.GetFiles(strDirName))
                    {
                        Files.Add(file, rootDir);
                    }
                    ExamineDir(strDirName, rootDir);
                }
            }
            if (strDir == rootDir)
            {
                foreach (var file in Directory.GetFiles(strDir))
                {
                    Files.Add(file, rootDir);
                }                
            }
        }
    }
}