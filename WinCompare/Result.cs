namespace WinCompare
{
    using System.Collections.Generic;

    public class Result
    {
        public Result()
        {
            Diff = new List<FileData>();
        }

        public bool AreEqual()
        {
            return this.Diff.Count == 0;
        }

        public List<FileData> Diff { get; set; }
    }
}