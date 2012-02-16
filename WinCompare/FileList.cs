namespace WinCompare
{
    using System.Collections.Generic;

    public class FileList
    {
        private readonly IHash hash;

        public FileList(IHash hash)
        {
            this.hash = hash;
            Files = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Files { get; private set; }

        public void Add(string file, string folderToRemove)
        {
            var newFilename = file.Replace(folderToRemove, "").TrimStart('\\');
            var hashedValue = hash.GetHash(file);
            Files.Add(newFilename, hashedValue);
        }
    }
}