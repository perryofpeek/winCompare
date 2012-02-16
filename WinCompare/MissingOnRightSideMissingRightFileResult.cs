namespace WinCompare
{
    public class MissingOnRightSideResult : IDiffResult
    {
        public MissingOnRightSideResult(string name, string hash)
        {
            this.Name = name;
            this.Hash = hash;
        }

        public string Name { get; set; }

        public string Hash { get; set; }
    }
}