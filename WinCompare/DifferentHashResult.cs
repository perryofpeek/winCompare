namespace WinCompare
{
    public class DifferentHashResult : IDiffResult
    {
        public DifferentHashResult(string name, string leftHash, string rightHash)
        {
            this.Name = name;
            this.LeftHash = leftHash;
            this.RightHash = rightHash;
        }

        public string Name { get; set; }

        public string LeftHash { get; set; }

        public string RightHash { get; set; }   
    }
}