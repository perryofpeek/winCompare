namespace WinCompare
{
    using System.Collections.Generic;

    public interface IHasher
    {
        Dictionary<string, string> GetFileHashList();

        void ExamineDir(string strDir, string rootDir);
    }
}