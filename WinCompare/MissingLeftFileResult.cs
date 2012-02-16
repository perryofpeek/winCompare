namespace WinCompare
{
    public class MissingOnLeftSideResult : IDiffResult
    {
       public MissingOnLeftSideResult(string name, string hash)
        {
           Name = name;
           Hash = hash;           
        }

        public string Name { get; set; }

        public string Hash { get; set; }       
    }
}