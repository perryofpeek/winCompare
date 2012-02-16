namespace WinCompare
{
    public class ExtraFileResult : IDiffResult
    {
        public ExtraFileResult(string name, string hash)
        {
            this.Name = name;
            this.Hash = hash;
        }

        public string Name { get; set; }

        public string Hash { get; set; }
    }
}