namespace WinCompare
{
    using System;
    using System.Collections.Generic;

    public class FolderHasher
    {
        private readonly IHasher hasher;

        private readonly bool debug;

        public FolderHasher(IHasher hasher, bool debug)
        {
            this.hasher = hasher;
            this.debug = debug;
        }

        public TimeSpan HashTime { get; private set; }

        public void Process1()
        {
            Results = Process(folder);
        }

        public Dictionary<string, string> Process(string folder)
        {
            var startTime = DateTime.Now;
            hasher.ExamineDir(folder, folder);
            HashTime = DateTime.Now.Subtract(startTime);
            if (this.debug)
            {
                foreach (var file in hasher.GetFileHashList())
                {
                    Console.WriteLine(string.Format("{0} [{1}]", file.Key, file.Value));
                }
            }
            return hasher.GetFileHashList();
        }

        private string folder;

        public Dictionary<string, string> Results { get; private set; }

        public void SetFolder(string folderPath)
        {
            this.folder = folderPath;
        }
    }
}